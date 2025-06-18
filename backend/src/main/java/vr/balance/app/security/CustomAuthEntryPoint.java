package vr.balance.app.security;

import com.fasterxml.jackson.databind.ObjectMapper;
import com.fasterxml.jackson.databind.SerializationFeature;
import com.fasterxml.jackson.datatype.jsr310.JavaTimeModule;
import jakarta.servlet.http.HttpServletRequest;
import jakarta.servlet.http.HttpServletResponse;
import org.springframework.security.core.AuthenticationException;
import org.springframework.security.web.AuthenticationEntryPoint;
import org.springframework.stereotype.Component;
import vr.balance.app.response.ApiErrorResponse;

import java.io.IOException;
import java.time.Instant;

/**
 * Custom implementation of {@link AuthenticationEntryPoint} that handles unauthorized access attempts.
 *
 * <p>This class is triggered whenever an unauthenticated request tries to access a protected endpoint.
 * It returns a structured JSON response with HTTP status 401 (Unauthorized), instead of a default HTML error page.
 *
 * <p>The response follows a consistent error format defined by {@link ApiErrorResponse}.
 *
 * @author [Your Name]
 */
@Component
public class CustomAuthEntryPoint implements AuthenticationEntryPoint {

    /**
     * Handles unauthorized requests by sending a JSON error response with HTTP status 401.
     *
     * <p>This method is automatically invoked by Spring Security when a user tries to access
     * a protected resource without authentication (e.g., missing or invalid JWT token).
     *
     * @param request the incoming HTTP request
     * @param response the HTTP response to write to
     * @param authException the exception that triggered the authentication failure
     * @throws IOException if writing the response fails
     */
    @Override
    public void commence(HttpServletRequest request,
                         HttpServletResponse response,
                         AuthenticationException authException)
            throws IOException {

        response.setStatus(HttpServletResponse.SC_UNAUTHORIZED);
        response.setContentType("application/json");

        ApiErrorResponse<String> errorResponse = new ApiErrorResponse<>(
                401,
                "Unauthorized",
                "Authentication required",
                Instant.now(),
                "CustomAuthEntryPoint"
        );

        ObjectMapper mapper = new ObjectMapper();
        mapper.registerModule(new JavaTimeModule());
        mapper.disable(SerializationFeature.WRITE_DATES_AS_TIMESTAMPS);

        String json = mapper.writeValueAsString(errorResponse);
        response.getWriter().write(json);
    }
}