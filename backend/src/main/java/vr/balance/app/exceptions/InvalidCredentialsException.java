package vr.balance.app.exceptions;

import org.springframework.http.HttpStatus;
import vr.balance.app.exceptions.base.BaseException;

public class InvalidCredentialsException extends BaseException {

    public InvalidCredentialsException(String message) {
        super(message);
    }

    @Override
    public HttpStatus getStatus() {
        return HttpStatus.FORBIDDEN; // 403
    }
}