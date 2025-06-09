package vr.balance.app.response;

import lombok.AllArgsConstructor;
import lombok.Data;

import java.time.Instant;

@Data
@AllArgsConstructor
public class ApiErrorResponse<T> {
    private int status;
    private String error;
    private String message;
    private Instant timestamp;
    private T details;

    public ApiErrorResponse(int status, String error, String message) {
        this.status = status;
        this.error = error;
        this.message = message;
        this.timestamp = Instant.now();
        this.details = null;
    }
}
