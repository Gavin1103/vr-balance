package vr.balance.app.security;

import org.springframework.security.authentication.AuthenticationProvider;
import org.springframework.security.authentication.BadCredentialsException;
import org.springframework.security.core.Authentication;
import org.springframework.security.core.authority.SimpleGrantedAuthority;
import org.springframework.security.crypto.password.PasswordEncoder;
import vr.balance.app.models.User;
import vr.balance.app.repository.UserRepository;

import java.util.List;
import java.util.Optional;

public class PincodeAuthenticationProvider implements AuthenticationProvider {

    private final UserRepository userRepository;
    private final PasswordEncoder passwordEncoder;

    public PincodeAuthenticationProvider(UserRepository userRepository, PasswordEncoder passwordEncoder) {
        this.userRepository = userRepository;
        this.passwordEncoder = passwordEncoder;
    }

    @Override
    public Authentication authenticate(Authentication authentication) {
        String identifier = authentication.getName();
        String pincode = authentication.getCredentials().toString();

        Optional<User> optionalUser = userRepository.findByEmail(identifier);
        if (optionalUser.isEmpty()) {
            optionalUser = userRepository.findByUsername(identifier);
        }

        User user = optionalUser.orElseThrow(() ->
                new BadCredentialsException("Invalid username/email or pincode"));

        if (!passwordEncoder.matches(pincode, user.getPincode())) {
            throw new BadCredentialsException("Invalid username/email or pincode");
        }

        return new PincodeAuthenticationToken(
                user.getId().toString(),
                null,
                List.of(new SimpleGrantedAuthority("ROLE_" + user.getRole().name()))
        );
    }

    @Override
    public boolean supports(Class<?> authentication) {
        return PincodeAuthenticationToken.class.isAssignableFrom(authentication);
    }
}