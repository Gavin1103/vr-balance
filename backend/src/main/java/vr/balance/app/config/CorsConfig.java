package vr.balance.app.config;

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
 * <p>During development, the local frontend (`http://localhost:5173`) can be
 * enabled by uncommenting the relevant line. This line should be disabled or
 * removed in production to avoid exposing the API to local environments.
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
    public WebMvcConfigurer corsConfigurer() {
        return new WebMvcConfigurer() {
            @Override
            public void addCorsMappings(CorsRegistry registry) {
                registry.addMapping("/**")
                        .allowedOrigins(
//                                "http://localhost:5173", // dev website (frontend)
                                "https://vrbalance.up.railway.app", // website (frontend)
                                "https://vrbalance-api.up.railway.app" // swagger UI
                        )
                        .allowedMethods("GET", "POST", "PUT", "DELETE", "OPTIONS")
                        .allowedHeaders("*")
                        .allowCredentials(true);
            }
        };
    }
}