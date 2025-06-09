package vr.balance.app.exceptions;

import org.springframework.http.HttpStatus;
import vr.balance.app.exceptions.base.BaseException;

public class PasswordMismatchException extends BaseException {
    public PasswordMismatchException(String message) {
        super(message);
    }

    @Override
    public HttpStatus getStatus() {
        return HttpStatus.BAD_REQUEST; // 400
    }
}
