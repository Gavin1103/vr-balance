package vr.balance.app.config;

import lombok.RequiredArgsConstructor;
import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Configuration;
import org.springframework.security.authentication.AuthenticationManager;
import org.springframework.security.config.annotation.method.configuration.EnableMethodSecurity;
import org.springframework.security.config.annotation.web.builders.HttpSecurity;
import org.springframework.security.config.annotation.web.configuration.EnableWebSecurity;
import org.springframework.security.config.annotation.web.configurers.AbstractHttpConfigurer;
import org.springframework.security.config.http.SessionCreationPolicy;
import org.springframework.security.web.SecurityFilterChain;
import org.springframework.security.web.authentication.UsernamePasswordAuthenticationFilter;
import vr.balance.app.security.*;

import static org.springframework.security.config.Customizer.withDefaults;

/**
 * Main security configuration for the application.
 *
 * <p>This class sets up Spring Security with:
 * <ul>
 *   <li>JWT-based stateless authentication</li>
 *   <li>Custom authentication entry point and access denied handler</li>
 *   <li>Role-based access control via {@code @PreAuthorize}</li>
 *   <li>Custom user details service and authentication manager</li>
 * </ul>
 *
 * <p>Public endpoints like login and Swagger are explicitly permitted.
 * All other endpoints require authentication.
 *
 * <p>CSRF is disabled, and session management is stateless, as is standard for token-based APIs.
 */
@Configuration
@EnableWebSecurity
@RequiredArgsConstructor
@EnableMethodSecurity(prePostEnabled = true)
public class SecurityConfig {

    private final JwtAuthenticationFilter jwtFilter;
    private final AuthenticationManager authenticationManager;
    private final CustomUserDetailsService userDetailsService;
    private final CustomAuthEntryPoint customAuthEntryPoint;
    private final CustomAccessDeniedHandler customAccessDeniedHandler;

    /**
     * Configures the {@link SecurityFilterChain} for the application.
     *
     * <p>This method defines:
     * <ul>
     *   <li>Disabled CSRF (as the app uses JWT and not cookies)</li>
     *   <li>CORS enabled with default settings (configured elsewhere)</li>
     *   <li>Stateless session management for token-based authentication</li>
     *   <li>Whitelist of publicly accessible endpoints (e.g. login and Swagger)</li>
     *   <li>Custom exception handling for unauthorized or forbidden access</li>
     *   <li>JWT authentication filter added before {@link UsernamePasswordAuthenticationFilter}</li>
     * </ul>
     *
     * @param http the {@link HttpSecurity} object to configure
     * @return the configured {@code SecurityFilterChain}
     * @throws Exception in case of misconfiguration
     */
    @Bean
    public SecurityFilterChain filterChain(HttpSecurity http) throws Exception {
        return http
                .csrf(AbstractHttpConfigurer::disable)
                .cors(withDefaults())
                .sessionManagement(session -> session.sessionCreationPolicy(SessionCreationPolicy.STATELESS))
                .authorizeHttpRequests(auth -> auth
                        .requestMatchers(
                                "/api/auth/login",
                                "/api/auth/login-pincode",
                                "/v3/api-docs/**",
                                "/swagger-ui/**",
                                "/swagger-ui.html",
                                "/api/public/**",
                                "/api/user-stats/public/**",
                                "/api/statistics/public/**",
                                "/api/test/public/**"
                        ).permitAll()
                        .anyRequest().authenticated()
                )
                .exceptionHandling(ex -> ex
                        .authenticationEntryPoint(customAuthEntryPoint)
                        .accessDeniedHandler(customAccessDeniedHandler)
                )
                .authenticationManager(authenticationManager)
                .userDetailsService(userDetailsService)
                .addFilterBefore(jwtFilter, UsernamePasswordAuthenticationFilter.class)
                .build();
    }
}