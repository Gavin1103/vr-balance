package vr.balance.app.response;

import lombok.AllArgsConstructor;
import lombok.Data;
import org.springframework.http.HttpStatus;

import java.time.Instant;

/**
 * A generic wrapper for successful API responses.
 *
 * <p>This class is used to return consistent and informative success responses
 * across the backend, including optional response data.
 *
 * <p>It includes:
 * <ul>
 *   <li>{@code status} – the HTTP status code (e.g. 200, 201)</li>
 *   <li>{@code message} – a human-readable success message</li>
 *   <li>{@code timestamp} – the time at which the response was generated</li>
 *   <li>{@code data} – the actual response payload (can be {@code null})</li>
 * </ul>
 *
 * @param <T> the type of data returned (can be {@code Void} if no data is included)
 */
@Data
@AllArgsConstructor
public class ApiStandardResponse<T> {

    /** The HTTP status code of the response (e.g. 200, 201). */
    private int status;

    /** A descriptive message about the outcome of the request. */
    private String message;

    /** Timestamp of when the response was generated. */
    private Instant timestamp;

    /** The actual response payload, if applicable. */
    private T data;

    /**
     * Constructs a response with status, message, and data.
     *
     * @param status the HTTP status
     * @param message the response message
     * @param data the response body
     */
    public ApiStandardResponse(HttpStatus status, String message, T data) {
        this.status = status.value();
        this.message = message;
        this.timestamp = Instant.now();
        this.data = data;
    }

    /**
     * Constructs a response with status and message, without data.
     *
     * @param status the HTTP status
     * @param message the response message
     */
    public ApiStandardResponse(HttpStatus status, String message) {
        this.status = status.value();
        this.message = message;
        this.timestamp = Instant.now();
        this.data = null;
    }
}