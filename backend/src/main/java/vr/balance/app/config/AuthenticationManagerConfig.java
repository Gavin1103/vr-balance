package vr.balance.app.config;

import lombok.RequiredArgsConstructor;
import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Configuration;
import org.springframework.security.authentication.AuthenticationManager;
import org.springframework.security.authentication.ProviderManager;
import org.springframework.security.authentication.dao.DaoAuthenticationProvider;
import vr.balance.app.security.PincodeAuthenticationProvider;

import java.util.List;

@Configuration
@RequiredArgsConstructor
public class AuthenticationManagerConfig {

    private final DaoAuthenticationProvider daoAuthenticationProvider;
    private final PincodeAuthenticationProvider pincodeAuthenticationProvider;

    @Bean
    public AuthenticationManager authenticationManager() {
        return new ProviderManager(
                List.of(
                        pincodeAuthenticationProvider,
                        daoAuthenticationProvider
                ));
    }
}