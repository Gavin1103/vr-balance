package vr.balance.app.security;

import org.springframework.security.core.authority.SimpleGrantedAuthority;
import org.springframework.security.core.userdetails.UserDetails;
import org.springframework.security.core.userdetails.UserDetailsService;
import org.springframework.security.core.userdetails.UsernameNotFoundException;
import org.springframework.stereotype.Service;
import vr.balance.app.models.User;
import vr.balance.app.repository.UserRepository;

import java.util.List;

/**
 * Custom implementation of {@link UserDetailsService} used by Spring Security
 * to load user-specific data during authentication.
 *
 * <p>This service supports:
 * <ul>
 *   <li>Standard login using email via {@code loadUserByUsername()}</li>
 *   <li>JWT-based authentication using user ID via {@code loadUserById()}</li>
 * </ul>
 *
 * <p>Returns a Spring Security {@link org.springframework.security.core.userdetails.User}
 * containing the user ID as username, hashed password, and granted authority based on user role.
 *
 * @author [Your Name]
 */
@Service
public class CustomUserDetailsService implements UserDetailsService {

    private final UserRepository userRepository;

    /**
     * Constructs the service with a {@link UserRepository} dependency.
     *
     * @param userRepository the repository used to fetch user records
     */
    public CustomUserDetailsService(UserRepository userRepository) {
        this.userRepository = userRepository;
    }

    /**
     * Loads a user by their email address (used for standard login).
     *
     * @param email the email used to identify the user
     * @return {@link UserDetails} for authentication
     * @throws UsernameNotFoundException if no user is found with the given email
     */
    @Override
    public UserDetails loadUserByUsername(String email) throws UsernameNotFoundException {
        User user = userRepository.findByEmail(email)
                .orElseThrow(() -> new UsernameNotFoundException("User not found"));

        return new org.springframework.security.core.userdetails.User(
                user.getId().toString(),
                user.getPassword(),
                List.of(new SimpleGrantedAuthority("ROLE_" + user.getRole().name()))
        );
    }

    /**
     * Loads a user by their user ID (used for JWT authentication).
     *
     * @param userId the ID of the user to load
     * @return {@link UserDetails} for authentication
     * @throws UsernameNotFoundException if no user is found with the given ID
     */
    public UserDetails loadUserById(Long userId) throws UsernameNotFoundException {
        User user = userRepository.findById(userId)
                .orElseThrow(() -> new UsernameNotFoundException("User not found"));

        return new org.springframework.security.core.userdetails.User(
                user.getId().toString(),
                user.getPassword(),
                List.of(new SimpleGrantedAuthority("ROLE_" + user.getRole().name()))
        );
    }
}