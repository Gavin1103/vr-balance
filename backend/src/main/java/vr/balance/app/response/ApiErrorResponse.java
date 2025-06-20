package vr.balance.app.response;

import lombok.AllArgsConstructor;
import lombok.Data;

import java.time.Instant;

/**
 * A generic wrapper for error responses returned by the API.
 *
 * <p>This class is used to provide consistent, structured error messages
 * across the entire backend, including authentication, validation, and authorization failures.
 *
 * <p>It includes:
 * <ul>
 *   <li>{@code status} – the HTTP status code (e.g. 401, 403, 500)</li>
 *   <li>{@code error} – a short name for the error (e.g. "Unauthorized")</li>
 *   <li>{@code message} – a human-readable error message</li>
 *   <li>{@code timestamp} – the time at which the error occurred</li>
 *   <li>{@code details} – optional additional data or context</li>
 * </ul>
 *
 * @param <T> the type of additional error details, or {@code Void} if none
 */
@Data
@AllArgsConstructor
public class ApiErrorResponse<T> {

    /** The HTTP status code of the error (e.g. 404, 403, 500). */
    private int status;

    /** A short error name (e.g. "Forbidden", "Bad Request"). */
    private String error;

    /** A descriptive error message intended for the client. */
    private String message;

    /** Timestamp of when the error occurred. */
    private Instant timestamp;

    /** Optional additional details about the error (may be {@code null}). */
    private T details;

    /**
     * Convenience constructor that sets status, error, and message.
     * Timestamp is automatically set to the current time, and {@code details} is set to {@code null}.
     *
     * @param status the HTTP status code
     * @param error the error name
     * @param message the error message
     */
    public ApiErrorResponse(int status, String error, String message) {
        this.status = status;
        this.error = error;
        this.message = message;
        this.timestamp = Instant.now();
        this.details = null;
    }
}