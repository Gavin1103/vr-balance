package vr.balance.app.security;

import jakarta.servlet.FilterChain;
import jakarta.servlet.ServletException;
import jakarta.servlet.http.HttpServletRequest;
import jakarta.servlet.http.HttpServletResponse;
import org.springframework.security.authentication.UsernamePasswordAuthenticationToken;
import org.springframework.security.core.context.SecurityContextHolder;
import org.springframework.security.core.userdetails.UserDetails;
import org.springframework.stereotype.Component;
import org.springframework.web.filter.OncePerRequestFilter;

import java.io.IOException;

/**
 * Filter that intercepts every HTTP request and checks for a valid JWT token.
 *
 * <p>If a valid token is present in the {@code Authorization} header, the user is authenticated
 * and the {@link SecurityContextHolder} is updated with the authenticated user.
 *
 * <p>This filter runs once per request and is registered in the Spring Security filter chain
 * before the {@link org.springframework.security.web.authentication.UsernamePasswordAuthenticationFilter}.
 *
 * <p>Expected header format:
 * <pre>
 * Authorization: Bearer &lt;token&gt;
 * </pre>
 *
 * @see JwtService
 * @see CustomUserDetailsService
 */
@Component
public class JwtAuthenticationFilter extends OncePerRequestFilter {

    private final JwtService jwtService;
    private final CustomUserDetailsService userDetailsService;

    /**
     * Constructs the filter with required dependencies.
     *
     * @param jwtService the service for parsing and validating JWT tokens
     * @param userDetailsService the service for loading user details by ID
     */
    public JwtAuthenticationFilter(JwtService jwtService, CustomUserDetailsService userDetailsService) {
        this.jwtService = jwtService;
        this.userDetailsService = userDetailsService;
    }

    /**
     * Filters incoming requests and authenticates users based on a valid JWT token.
     *
     * <p>If no valid token is found, the request proceeds unauthenticated.
     *
     * @param request the HTTP request
     * @param response the HTTP response
     * @param filterChain the filter chain to continue processing
     * @throws ServletException in case of filter errors
     * @throws IOException in case of I/O errors
     */
    @Override
    protected void doFilterInternal(HttpServletRequest request,
                                    HttpServletResponse response,
                                    FilterChain filterChain) throws ServletException, IOException {

        final String authHeader = request.getHeader("Authorization");

        if (authHeader == null || !authHeader.startsWith("Bearer ")) {
            filterChain.doFilter(request, response);
            return;
        }

        String token = authHeader.substring(7);
        Long userId = jwtService.extractUserId(token);

        if (userId != null && SecurityContextHolder.getContext().getAuthentication() == null) {
            UserDetails userDetails = userDetailsService.loadUserById(userId);

            if (jwtService.isTokenValid(token)) {
                UsernamePasswordAuthenticationToken authToken = new UsernamePasswordAuthenticationToken(
                        userDetails, null, userDetails.getAuthorities());

                SecurityContextHolder.getContext().setAuthentication(authToken);
            }
        }

        filterChain.doFilter(request, response);
    }
}