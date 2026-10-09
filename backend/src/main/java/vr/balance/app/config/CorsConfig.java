package vr.balance.app.config;

import org.springframework.beans.factory.annotation.Value;
import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Configuration;
import org.springframework.web.servlet.config.annotation.CorsRegistry;
import org.springframework.web.servlet.config.annotation.WebMvcConfigurer;

/**
 * Configuration class for Cross-Origin Resource Sharing (CORS) settings.
 *
 * <p>This configures which external origins are allowed to access the API.
 * It registers a {@link WebMvcConfigurer} bean that customizes CORS mappings
 * for all incoming HTTP requests.
 *
 * <p>Allowed origins are read from {@code app.cors.allowed-origins} (comma-separated),
 * which can be overridden with the {@code CORS_ALLOWED_ORIGINS} environment variable.
 */
@Configuration
public class CorsConfig {

    /**
     * Creates a {@link WebMvcConfigurer} bean to customize CORS mappings.
     *
     * <p>Allows cross-origin requests from the deployed frontend and Swagger UI domains.
     * Supports HTTP methods GET, POST, PUT, DELETE, and OPTIONS, and accepts all headers.
     * Credentials (such as cookies or authorization headers) are allowed.
     *
     * @return the configured {@code WebMvcConfigurer} with custom CORS rules
     */
    @Bean
    public WebMvcConfigurer corsConfigurer(@Value("${app.cors.allowed-origins}") String[] allowedOrigins) {
        return new WebMvcConfigurer() {
            @Override
            public void addCorsMappings(CorsRegistry registry) {
                registry.addMapping("/**")
                        .allowedOrigins(allowedOrigins)
                        .allowedMethods("GET", "POST", "PUT", "DELETE", "OPTIONS")
                        .allowedHeaders("*")
                        .allowCredentials(true);
            }
        };
    }
}