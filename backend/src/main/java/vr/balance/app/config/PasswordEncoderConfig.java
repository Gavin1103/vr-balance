package vr.balance.app.config;

import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Configuration;
import org.springframework.security.crypto.bcrypt.BCryptPasswordEncoder;
import org.springframework.security.crypto.password.PasswordEncoder;

/**
 * Configuration class that defines a {@link PasswordEncoder} bean using BCrypt.
 *
 * <p>BCrypt is a strong hashing function that includes a salt and is adaptive,
 * making it suitable for securely storing passwords and pincodes.
 *
 * <p>This bean is used throughout the application to hash and verify credentials.
 */
@Configuration
public class PasswordEncoderConfig {

    /**
     * Registers a {@link PasswordEncoder} bean using {@link BCryptPasswordEncoder}.
     *
     * @return a BCrypt-based password encoder
     */
    @Bean
    public PasswordEncoder passwordEncoder() {
        return new BCryptPasswordEncoder();
    }
}