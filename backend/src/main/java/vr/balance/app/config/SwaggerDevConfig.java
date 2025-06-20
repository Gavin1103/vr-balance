package vr.balance.app.config;

import io.swagger.v3.oas.annotations.OpenAPIDefinition;
import io.swagger.v3.oas.annotations.enums.SecuritySchemeType;
import io.swagger.v3.oas.annotations.info.Info;
import io.swagger.v3.oas.annotations.security.SecurityScheme;
import org.springframework.context.annotation.Configuration;
import org.springframework.context.annotation.Profile;

/**
 * Swagger/OpenAPI configuration for the development environment.
 *
 * <p>This configuration:
 * <ul>
 *   <li>Defines the API title, version, and description via {@link OpenAPIDefinition}</li>
 *   <li>Registers a {@code bearerAuth} security scheme used for JWT authentication in Swagger UI</li>
 *   <li>Is only active when the {@code dev} Spring profile is enabled</li>
 * </ul>
 *
 * <p>The {@link SecurityScheme} allows protected endpoints to be tested from Swagger UI
 * by adding an Authorization header with a Bearer token.
 *
 * <p>Example usage in a controller:
 * <pre>
 * {@code
 * @SecurityRequirement(name = "bearerAuth")
 * @GetMapping("/me")
 * public ResponseEntity<ApiStandardResponse<UserProfileResponse>> getCurrentUser() { ... }
 * }
 * </pre>
 *
 * @see io.swagger.v3.oas.annotations.security.SecurityRequirement
 */
@Configuration
@OpenAPIDefinition(
        info = @Info(
                title = "VR Balance API",
                version = "1.0",
                description = "API for VR Balance App"
        )
)
@SecurityScheme(
        name = "bearerAuth",
        type = SecuritySchemeType.HTTP,
        scheme = "bearer",
        bearerFormat = "JWT"
)
@Profile("dev")
public class SwaggerDevConfig {
}