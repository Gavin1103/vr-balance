package vr.balance.app.config;

import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Configuration;
import org.springframework.security.authentication.dao.DaoAuthenticationProvider;
import org.springframework.security.crypto.password.PasswordEncoder;
import vr.balance.app.repository.UserRepository;
import vr.balance.app.security.CustomUserDetailsService;
import vr.balance.app.security.PincodeAuthenticationProvider;

/**
 * Configuration class that defines and registers the authentication providers used by Spring Security.
 *
 * <p>This includes:
 * <ul>
 *   <li>{@link DaoAuthenticationProvider} – standard username/password authentication using a {@link CustomUserDetailsService}</li>
 *   <li>{@link PincodeAuthenticationProvider} – custom provider that authenticates users based on a pincode</li>
 * </ul>
 *
 * <p>These providers are injected into the {@link org.springframework.security.authentication.AuthenticationManager}
 * through {@link AuthenticationManagerConfig}.
 */
@Configuration
public class AuthenticationProvidersConfig {

    /**
     * Creates and configures a {@link DaoAuthenticationProvider} bean.
     *
     * <p>This provider supports standard authentication using a username/email and password. It relies
     * on a custom implementation of {@link CustomUserDetailsService} and a {@link PasswordEncoder}.
     *
     * @param userDetailsService the service that loads user details
     * @param passwordEncoder the encoder used to hash and verify passwords
     * @return the configured {@code DaoAuthenticationProvider}
     */
    @Bean
    public DaoAuthenticationProvider daoAuthenticationProvider(CustomUserDetailsService userDetailsService, PasswordEncoder passwordEncoder) {
        DaoAuthenticationProvider provider = new DaoAuthenticationProvider();
        provider.setUserDetailsService(userDetailsService);
        provider.setPasswordEncoder(passwordEncoder);
        return provider;
    }

    /**
     * Creates and returns a {@link PincodeAuthenticationProvider} bean.
     *
     * <p>This custom authentication provider allows users to log in using a username/email and pincode combination.
     * It interacts directly with the {@link UserRepository} to retrieve and verify user credentials.
     *
     * @param userRepository the repository used to fetch users
     * @param passwordEncoder the encoder used to hash and verify pincodes
     * @return the configured {@code PincodeAuthenticationProvider}
     */
    @Bean
    public PincodeAuthenticationProvider pincodeAuthenticationProvider(UserRepository userRepository, PasswordEncoder passwordEncoder) {
        return new PincodeAuthenticationProvider(userRepository, passwordEncoder);
    }
}