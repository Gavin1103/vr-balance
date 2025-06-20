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

/**
 * Custom {@link AuthenticationProvider} implementation that authenticates users
 * based on a pincode instead of a traditional password.
 *
 * <p>This provider allows users to log in using either their email or username
 * along with a 4-digit pincode. It fetches the user from the database using
 * {@link UserRepository} and verifies the pincode using a {@link PasswordEncoder}.
 *
 * <p>If authentication is successful, a {@link PincodeAuthenticationToken} is returned
 * containing the user's ID and role-based authorities.
 *
 * @see PincodeAuthenticationToken
 */
public class PincodeAuthenticationProvider implements AuthenticationProvider {

    private final UserRepository userRepository;
    private final PasswordEncoder passwordEncoder;

    /**
     * Creates a new instance of {@code PincodeAuthenticationProvider}.
     *
     * @param userRepository  the repository used to find users by email or username
     * @param passwordEncoder the encoder used to verify the hashed pincode
     */
    public PincodeAuthenticationProvider(UserRepository userRepository, PasswordEncoder passwordEncoder) {
        this.userRepository = userRepository;
        this.passwordEncoder = passwordEncoder;
    }

    /**
     * Attempts to authenticate a user using their username/email and pincode.
     *
     * <p>The method first attempts to locate the user by email; if not found,
     * it tries by username. If the user exists and the pincode matches the stored
     * (hashed) pincode, an authenticated {@link PincodeAuthenticationToken} is returned.
     *
     * @param authentication the authentication request object
     * @return an authenticated {@code PincodeAuthenticationToken} if successful
     * @throws BadCredentialsException if authentication fails
     */
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

    /**
     * Indicates whether this provider supports the given authentication type.
     *
     * @param authentication the class of the authentication object
     * @return true if the authentication type is {@link PincodeAuthenticationToken}
     */
    @Override
    public boolean supports(Class<?> authentication) {
        return PincodeAuthenticationToken.class.isAssignableFrom(authentication);
    }
}