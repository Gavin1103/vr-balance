package vr.balance.app.config;

import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Configuration;
import org.springframework.security.authentication.dao.DaoAuthenticationProvider;
import org.springframework.security.crypto.password.PasswordEncoder;
import vr.balance.app.repository.UserRepository;
import vr.balance.app.security.CustomUserDetailsService;
import vr.balance.app.security.PincodeAuthenticationProvider;

@Configuration
public class AuthenticationProvidersConfig {

    @Bean
    public DaoAuthenticationProvider daoAuthenticationProvider(CustomUserDetailsService userDetailsService, PasswordEncoder passwordEncoder) {
        DaoAuthenticationProvider provider = new DaoAuthenticationProvider();
        provider.setUserDetailsService(userDetailsService);
        provider.setPasswordEncoder(passwordEncoder);
        return provider;
    }

    @Bean
    public PincodeAuthenticationProvider pincodeAuthenticationProvider(UserRepository userRepository, PasswordEncoder passwordEncoder) {
        return new PincodeAuthenticationProvider(userRepository, passwordEncoder);
    }
}