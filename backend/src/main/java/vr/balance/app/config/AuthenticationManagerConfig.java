package vr.balance.app.config;

import lombok.RequiredArgsConstructor;
import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Configuration;
import org.springframework.security.authentication.AuthenticationManager;
import org.springframework.security.authentication.ProviderManager;
import org.springframework.security.authentication.dao.DaoAuthenticationProvider;
import vr.balance.app.security.PincodeAuthenticationProvider;

import java.util.List;

/**
 * Configuration class for setting up the custom {@link AuthenticationManager}.
 *
 * <p>This configuration registers multiple {@link org.springframework.security.authentication.AuthenticationProvider}
 * implementations to support different types of authentication:
 * <ul>
 *     <li>{@link DaoAuthenticationProvider} – standard username/password authentication</li>
 *     <li>{@link PincodeAuthenticationProvider} – custom authentication using a pincode and username/email</li>
 * </ul>
 *
 * <p>The {@code AuthenticationManager} is exposed as a Spring Bean so it can be injected and reused
 * in custom authentication flows such as JWT or controller-based logins.
 */
@Configuration
@RequiredArgsConstructor
public class AuthenticationManagerConfig {

    private final DaoAuthenticationProvider daoAuthenticationProvider;
    private final PincodeAuthenticationProvider pincodeAuthenticationProvider;

    /**
     * Creates and exposes a Spring-managed {@link AuthenticationManager} bean.
     *
     * <p>This manager uses a {@link ProviderManager} that delegates authentication requests to a list
     * of providers. The order matters: the {@link PincodeAuthenticationProvider} is checked first,
     * followed by the {@link DaoAuthenticationProvider}.
     *
     * @return the configured {@code AuthenticationManager}
     */
    @Bean
    public AuthenticationManager authenticationManager() {
        return new ProviderManager(
                List.of(
                        pincodeAuthenticationProvider,
                        daoAuthenticationProvider
                ));
    }
}