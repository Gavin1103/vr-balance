package vr.balance.app.response;

import lombok.AllArgsConstructor;
import lombok.Data;
import org.springframework.http.HttpStatus;

import java.time.Instant;

@Data
@AllArgsConstructor
public class ApiStandardResponse<T> {
    private int status;
    private String message;
    private Instant timestamp;
    private T data;

    public ApiStandardResponse(HttpStatus status, String message, T data) {
        this.status = status.value();
        this.message = message;
        this.timestamp = Instant.now();
        this.data = data;
    }

    public ApiStandardResponse(HttpStatus status, String message) {
        this.status = status.value();
        this.message = message;
        this.timestamp = Instant.now();
        this.data = null;
    }
}
