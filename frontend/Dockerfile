# syntax=docker/dockerfile:1

# ---- Build stage ----
FROM node:22-alpine AS build
WORKDIR /app

# Skip the Cypress binary download triggered by the "prepare" script
ENV CYPRESS_INSTALL_BINARY=0

COPY package.json package-lock.json ./
RUN npm ci

COPY . .

# Vite inlines env vars at build time. Defaults to the same-origin /api proxied by nginx
ARG VITE_API_BASE_URL=/api
RUN if [ -n "$VITE_API_BASE_URL" ]; then \
        echo "VITE_API_BASE_URL=$VITE_API_BASE_URL" > .env.production.local; \
    fi \
    && npm run build

# ---- Runtime stage ----
FROM nginxinc/nginx-unprivileged:1.27-alpine

# Rendered to /etc/nginx/conf.d/default.conf by the image's envsubst entrypoint
COPY nginx.conf.template /etc/nginx/templates/default.conf.template
COPY --from=build /app/dist /usr/share/nginx/html

ENV PORT=8080 \
    API_UPSTREAM=backend:8080
EXPOSE 8080

HEALTHCHECK --interval=30s --timeout=5s --retries=3 \
    CMD wget -qO- "http://127.0.0.1:${PORT}/healthz" > /dev/null || exit 1
