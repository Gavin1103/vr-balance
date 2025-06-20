package vr.balance.app.security;

import io.jsonwebtoken.*;
import io.jsonwebtoken.security.Keys;
import jakarta.servlet.http.HttpServletRequest;
import org.springframework.stereotype.Component;
import org.springframework.util.StringUtils;
import vr.balance.app.models.User;

import java.security.Key;
import java.util.Date;
import java.util.function.Function;

/**
 * Service class responsible for generating, parsing, and validating JSON Web Tokens (JWTs).
 *
 * <p>This class uses the JJWT library to handle token creation and validation.
 * It signs tokens using the HMAC SHA-256 algorithm and a generated secret key.
 *
 * <p>The token contains:
 * <ul>
 *   <li>User ID as the subject</li>
 *   <li>User role</li>
 *   <li>The authentication method used (e.g., password or pincode)</li>
 * </ul>
 *
 * <p>Token expiration is set to 1 hour.
 *
 */
@Component
public class JwtService {

    private static final long EXPIRATION_TIME = 1000 * 60 * 60; // 1 hour
    private final Key key = Keys.secretKeyFor(SignatureAlgorithm.HS256);

    /**
     * Generates a JWT token for the given user and authentication method.
     *
     * @param user   the authenticated user
     * @param method the method used to authenticate (e.g., "password", "pincode")
     * @return a signed JWT token string
     */
    public String generateToken(User user, String method) {
        return Jwts.builder()
                .setHeaderParam("typ", "JWT")
                .setSubject(user.getId().toString())
                .claim("role", user.getRole().name())
                .claim("auth_method", method)
                .setIssuedAt(new Date())
                .setExpiration(new Date(System.currentTimeMillis() + EXPIRATION_TIME))
                .signWith(key)
                .compact();
    }

    /**
     * Extracts the user ID from the token's subject claim.
     *
     * @param token the JWT token
     * @return the user ID as a {@code Long}
     * @throws JwtException if the token is invalid or cannot be parsed
     */
    public Long extractUserId(String token) {
        String subject = Jwts.parserBuilder()
                .setSigningKey(key)
                .build()
                .parseClaimsJws(token)
                .getBody()
                .getSubject();

        return Long.parseLong(subject);
    }

    /**
     * Extracts a specific claim from the JWT token based on a key.
     *
     * @param request the HTTP request containing the Authorization header
     * @param key     the name of the claim to extract
     * @return the claim value as a String, or {@code null} if not found
     */
    public String extractUserData(HttpServletRequest request, String key) {
        final String token = extractJwtFromRequest(request);
        return extractClaim(token, claims -> claims.get(key, String.class));
    }

    /**
     * Retrieves the raw JWT token from the Authorization header of a request.
     *
     * @param request the HTTP request
     * @return the JWT token, or {@code null} if not present or malformed
     */
    public String extractJwtFromRequest(HttpServletRequest request) {
        String bearerToken = request.getHeader("Authorization");
        if (StringUtils.hasText(bearerToken) && bearerToken.startsWith("Bearer ")) {
            return bearerToken.substring(7);
        }
        return null;
    }

    /**
     * Extracts a claim from the JWT token using a resolver function.
     *
     * @param token          the JWT token
     * @param claimsResolver a function that maps claims to the desired value
     * @param <T>            the return type of the claim
     * @return the extracted claim
     */
    public <T> T extractClaim(String token, Function<Claims, T> claimsResolver) {
        final Claims claims = extractAllClaims(token);
        return claimsResolver.apply(claims);
    }

    /**
     * Checks if the token is valid (i.e., not expired).
     *
     * @param token the JWT token
     * @return {@code true} if valid, {@code false} if expired
     */
    public boolean isTokenValid(String token) {
        return !isTokenExpired(token);
    }

    /**
     * Checks if the token is expired based on its expiration claim.
     *
     * @param token the JWT token
     * @return {@code true} if expired, {@code false} otherwise
     */
    private boolean isTokenExpired(String token) {
        return extractExpiration(token).before(new Date());
    }

    /**
     * Retrieves the expiration date of the JWT token.
     *
     * @param token the JWT token
     * @return the expiration {@link Date}
     */
    private Date extractExpiration(String token) {
        return extractAllClaims(token).getExpiration();
    }

    /**
     * Parses and returns all claims from the JWT token.
     *
     * @param token the JWT token
     * @return the {@link Claims} object containing all payload data
     * @throws JwtException if the token is invalid
     */
    private Claims extractAllClaims(String token) {
        return Jwts.parserBuilder()
                .setSigningKey(key)
                .build()
                .parseClaimsJws(token)
                .getBody();
    }
}