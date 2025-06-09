package vr.balance.app.exceptions;

import org.springframework.http.HttpStatus;
import vr.balance.app.exceptions.base.BaseException;

public class EmailAlreadyInUseException extends BaseException {

    public EmailAlreadyInUseException(String message) {
        super(message);
    }

    @Override
    public HttpStatus getStatus() {
        return HttpStatus.CONFLICT; // 409
    }
}