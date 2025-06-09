package vr.balance.app.exceptions.handler;

import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.MethodArgumentNotValidException;
import org.springframework.web.bind.annotation.*;
import vr.balance.app.exceptions.base.BaseException;
import vr.balance.app.response.ApiErrorResponse;

import java.time.Instant;
import java.util.stream.Collectors;

@RestControllerAdvice
public class GlobalExceptionHandler {

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

    @ExceptionHandler(Exception.class)
    public ResponseEntity<ApiErrorResponse<String>> handleGeneric(Exception ex) {
        ex.printStackTrace();

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
