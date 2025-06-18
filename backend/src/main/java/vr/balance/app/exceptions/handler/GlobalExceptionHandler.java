package vr.balance.app.exceptions.handler;

import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.MethodArgumentNotValidException;
import org.springframework.web.bind.annotation.*;
import vr.balance.app.exceptions.base.BaseException;
import vr.balance.app.response.ApiErrorResponse;

import java.time.Instant;
import java.util.stream.Collectors;

/**
 * Global exception handler that captures and processes common exceptions thrown in the application.
 *
 * <p>This class uses {@link RestControllerAdvice} to automatically handle exceptions thrown
 * in any controller, providing structured JSON error responses using {@link ApiErrorResponse}.
 *
 * <p>Handled exceptions include:
 * <ul>
 *   <li>{@link MethodArgumentNotValidException} – for validation errors on request DTOs</li>
 *   <li>{@link BaseException} – custom business exceptions with status codes</li>
 *   <li>{@link org.springframework.security.access.AccessDeniedException} – access denied (403)</li>
 *   <li>{@link org.springframework.security.authorization.AuthorizationDeniedException} – authorization failure (403)</li>
 *   <li>{@link Exception} – fallback for unexpected internal server errors (500)</li>
 * </ul>
 *
 */
@RestControllerAdvice
public class GlobalExceptionHandler {

    /**
     * Handles validation errors for invalid DTO inputs (e.g. @Valid failure).
     *
     * @param ex the thrown {@link MethodArgumentNotValidException}
     * @return a 400 Bad Request response with field-level error messages
     */
    @ExceptionHandler(MethodArgumentNotValidException.class)
    public ResponseEntity<ApiErrorResponse<Void>> handleValidationErrors(MethodArgumentNotValidException ex) {
        String message = ex.getBindingResult().getFieldErrors().stream()
                .map(error -> error.getField() + ": " + error.getDefaultMessage())
                .collect(Collectors.joining("; "));

        return ResponseEntity.badRequest().body(
                new ApiErrorResponse<>(
                        HttpStatus.BAD_REQUEST.value(),
                        "Bad Request",
                        message
                )
        );
    }

    /**
     * Handles custom application exceptions extending {@link BaseException}.
     *
     * @param ex the custom exception
     * @return a response with the custom status code and error message
     */
    @ExceptionHandler(BaseException.class)
    public ResponseEntity<ApiErrorResponse<Void>> handleBaseException(BaseException ex) {
        HttpStatus status = ex.getStatus();
        return ResponseEntity.status(status)
                .body(new ApiErrorResponse<>(
                        status.value(),
                        status.getReasonPhrase(),
                        ex.getMessage()
                ));
    }

    /**
     * Handles Spring Security's {@link org.springframework.security.access.AccessDeniedException}.
     *
     * <p>This occurs when an authenticated user tries to access a resource without the proper authority.
     *
     * @param ex the access denied exception
     * @return a 403 Forbidden response with a generic error message
     */
    @ExceptionHandler(org.springframework.security.access.AccessDeniedException.class)
    public ResponseEntity<ApiErrorResponse<String>> handleAccessDeniedException(org.springframework.security.access.AccessDeniedException ex) {
        return ResponseEntity.status(HttpStatus.FORBIDDEN)
                .body(new ApiErrorResponse<>(
                        HttpStatus.FORBIDDEN.value(),
                        "Forbidden",
                        "Access denied",
                        Instant.now(),
                        "AccessDeniedException"
                ));
    }

    /**
     * Handles {@link org.springframework.security.authorization.AuthorizationDeniedException}
     * which occurs in method-level security expressions like @PreAuthorize.
     *
     * @param ex the authorization denied exception
     * @return a 403 Forbidden response
     */
    @ExceptionHandler(org.springframework.security.authorization.AuthorizationDeniedException.class)
    public ResponseEntity<ApiErrorResponse<String>> handleAuthorizationDeniedException(org.springframework.security.authorization.AuthorizationDeniedException ex) {
        return ResponseEntity.status(HttpStatus.FORBIDDEN)
                .body(new ApiErrorResponse<>(
                        HttpStatus.FORBIDDEN.value(),
                        "Forbidden",
                        "Access denied",
                        Instant.now(),
                        "AuthorizationDeniedException"
                ));
    }

    /**
     * Handles all uncaught exceptions as a fallback.
     *
     * <p>This ensures that internal server errors do not return stack traces to clients.
     *
     * @param ex the unexpected exception
     * @return a 500 Internal Server Error response
     */
    @ExceptionHandler(Exception.class)
    public ResponseEntity<ApiErrorResponse<String>> handleGeneric(Exception ex) {
        ex.printStackTrace(); // Optional: log to monitoring system

        return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR)
                .body(new ApiErrorResponse<>(
                        HttpStatus.INTERNAL_SERVER_ERROR.value(),
                        HttpStatus.INTERNAL_SERVER_ERROR.getReasonPhrase(),
                        ex.getMessage(),
                        Instant.now(),
                        "Unexpected server error"
                ));
    }
}