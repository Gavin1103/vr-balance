package vr.balance.app.config;

import io.swagger.v3.oas.annotations.OpenAPIDefinition;
import io.swagger.v3.oas.annotations.enums.SecuritySchemeType;
import io.swagger.v3.oas.annotations.info.Info;
import io.swagger.v3.oas.annotations.security.SecurityScheme;
import io.swagger.v3.oas.models.OpenAPI;
import io.swagger.v3.oas.models.servers.Server;
import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Configuration;
import org.springframework.context.annotation.Profile;

import java.util.List;

/**
 * Swagger/OpenAPI configuration specifically for the production environment.
 *
 * <p>This configuration:
 * <ul>
 *   <li>Defines metadata such as API title, version, and description using {@link OpenAPIDefinition}</li>
 *   <li>Registers the {@code bearerAuth} security scheme for JWT-based authentication</li>
 *   <li>Sets a custom server URL to match the production deployment endpoint</li>
 *   <li>Is only active when the {@code prod} Spring profile is enabled</li>
 * </ul>
 *
 * <p>The security scheme ensures that Swagger UI supports secured endpoints,
 * allowing you to authorize using a Bearer token.
 *
 * @see io.swagger.v3.oas.annotations.security.SecurityRequirement
 * @see io.swagger.v3.oas.models.OpenAPI
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
@Profile("prod")
public class SwaggerProductionConfig {

    /**
     * Configures the OpenAPI object with the production server URL.
     *
     * <p>This ensures that when viewing Swagger UI in production,
     * the "Server" dropdown is set to the correct base URL of the deployed API.
     *
     * @return the customized {@link OpenAPI} object
     */
    @Bean
    public OpenAPI customOpenAPI() {
        Server server = new Server();
        server.setUrl("https://vrbalance-api.up.railway.app");
        server.setDescription("Production API");

        return new OpenAPI().servers(List.of(server));
    }
}