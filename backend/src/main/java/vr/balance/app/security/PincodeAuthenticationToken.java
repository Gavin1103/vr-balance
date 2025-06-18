package vr.balance.app.security;

import org.springframework.security.authentication.AbstractAuthenticationToken;
import org.springframework.security.core.GrantedAuthority;

import java.util.Collection;

/**
 * Custom {@link org.springframework.security.core.Authentication} implementation
 * for authenticating users via a pincode instead of a password.
 *
 * <p>This token is used by {@link PincodeAuthenticationProvider} during the
 * authentication process. It holds the user's identity (username or email)
 * as the {@code principal} and the raw pincode as the {@code credentials}.
 *
 * <p>There are two constructors:
 * <ul>
 *   <li>Unauthenticated: used when the token is first created from login input</li>
 *   <li>Authenticated: used once the user has been successfully authenticated and authorities are assigned</li>
 * </ul>
 *
 * @see PincodeAuthenticationProvider
 */
public class PincodeAuthenticationToken extends AbstractAuthenticationToken {

    private final Object principal;
    private final Object credentials;

    /**
     * Constructs an unauthenticated token with the provided principal and credentials.
     *
     * <p>This constructor is typically used when the authentication request is first made.
     * The token will be passed to the {@link PincodeAuthenticationProvider} for validation.
     *
     * @param principal   the user identifier (e.g. username or email)
     * @param credentials the raw pincode
     */
    public PincodeAuthenticationToken(Object principal, Object credentials) {
        super(null);
        this.principal = principal;
        this.credentials = credentials;
        setAuthenticated(false);
    }

    /**
     * Constructs an authenticated token with granted authorities.
     *
     * <p>This constructor is used after the user has been successfully authenticated.
     * It includes a collection of {@link GrantedAuthority} objects representing the user's roles.
     *
     * @param principal   the authenticated user (e.g. UserDetails)
     * @param credentials the original credentials (can be nullified post-auth)
     * @param authorities the authorities granted to the user
     */
    public PincodeAuthenticationToken(Object principal, Object credentials, Collection<? extends GrantedAuthority> authorities) {
        super(authorities);
        this.principal = principal;
        this.credentials = credentials;
        setAuthenticated(true);
    }

    /**
     * Returns the credentials (pincode) associated with this token.
     *
     * @return the pincode or credential object
     */
    @Override
    public Object getCredentials() {
        return credentials;
    }

    /**
     * Returns the principal (typically username or email) associated with this token.
     *
     * @return the principal
     */
    @Override
    public Object getPrincipal() {
        return principal;
    }
}