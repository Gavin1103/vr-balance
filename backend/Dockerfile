# syntax=docker/dockerfile:1

# ---- Build stage ----
FROM eclipse-temurin:21-jdk AS build
WORKDIR /workspace

# Resolve dependencies first so they are cached between source changes
COPY gradlew settings.gradle.kts build.gradle.kts ./
COPY gradle ./gradle
RUN chmod +x gradlew && ./gradlew dependencies --no-daemon > /dev/null

COPY src ./src
RUN ./gradlew bootJar --no-daemon -x test \
    && cp build/libs/*-SNAPSHOT.jar app.jar \
    && java -Djarmode=tools -jar app.jar extract --layers --launcher --destination extracted

# ---- Runtime stage ----
FROM eclipse-temurin:21-jre-alpine
WORKDIR /app

RUN apk add --no-cache wget \
    && addgroup -S app && adduser -S app -G app

# Layered copy: dependencies change less often than application code
COPY --from=build /workspace/extracted/dependencies/ ./
COPY --from=build /workspace/extracted/spring-boot-loader/ ./
COPY --from=build /workspace/extracted/snapshot-dependencies/ ./
COPY --from=build /workspace/extracted/application/ ./

USER app

ENV PORT=8080 \
    SPRING_PROFILES_ACTIVE=prod \
    JAVA_TOOL_OPTIONS="-XX:MaxRAMPercentage=75.0 -XX:+ExitOnOutOfMemoryError"

EXPOSE 8080

HEALTHCHECK --interval=30s --timeout=5s --start-period=60s --retries=3 \
    CMD wget -qO- "http://127.0.0.1:${PORT}/actuator/health" > /dev/null || exit 1

ENTRYPOINT ["java", "org.springframework.boot.loader.launch.JarLauncher"]
