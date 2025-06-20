package vr.balance.app.security;

import com.fasterxml.jackson.databind.ObjectMapper;
import com.fasterxml.jackson.databind.SerializationFeature;
import com.fasterxml.jackson.datatype.jsr310.JavaTimeModule;
import jakarta.servlet.http.HttpServletRequest;
import jakarta.servlet.http.HttpServletResponse;
import org.springframework.security.access.AccessDeniedException;
import org.springframework.security.web.access.AccessDeniedHandler;
import org.springframework.stereotype.Component;
import vr.balance.app.response.ApiErrorResponse;

import java.io.IOException;
import java.time.Instant;

/**
 * Custom implementation of {@link AccessDeniedHandler} that handles forbidden access attempts.
 *
 * <p>This handler is triggered when an authenticated user tries to access a resource
 * for which they do not have sufficient permissions (HTTP 403 Forbidden).
 *
 * <p>Instead of returning a default HTML error page, this class returns a structured
 * JSON response using the {@link ApiErrorResponse} format.
 *
 * @author [Your Name]
 */
@Component
public class CustomAccessDeniedHandler implements AccessDeniedHandler {

    /**
     * Handles access denied exceptions by sending a JSON response with HTTP status 403.
     *
     * <p>This method is automatically invoked by Spring Security when a user is authenticated
     * but lacks the required role or permission to access a protected endpoint.
     *
     * @param request the incoming HTTP request
     * @param response the HTTP response to write to
     * @param accessDeniedException the exception that triggered the access denial
     * @throws IOException if writing the response fails
     */
    @Override
    public void handle(HttpServletRequest request,
                       HttpServletResponse response,
                       AccessDeniedException accessDeniedException) throws IOException {

        response.setStatus(HttpServletResponse.SC_FORBIDDEN);
        response.setContentType("application/json");

        ApiErrorResponse<Void> errorResponse = new ApiErrorResponse<>(
                403,
                "Forbidden",
                "Access denied",
                Instant.now(),
                null
        );

        ObjectMapper mapper = new ObjectMapper();
        mapper.registerModule(new JavaTimeModule());
        mapper.disable(SerializationFeature.WRITE_DATES_AS_TIMESTAMPS);

        response.getWriter().write(mapper.writeValueAsString(errorResponse));
    }
}