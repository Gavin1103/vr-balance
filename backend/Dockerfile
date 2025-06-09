# Start from an OpenJDK image
FROM eclipse-temurin:21-jdk

# Set working directory
WORKDIR /app

# Copy everything
COPY . .

# Build the JAR
RUN ./gradlew clean bootJar --no-daemon

# Expose port (pas dit aan als je een andere gebruikt)
EXPOSE 8080

# Run the JAR
CMD ["java", "-jar", "build/libs/backend-0.0.1-SNAPSHOT.jar"]