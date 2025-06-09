package vr.balance.app.exceptions;

import org.springframework.http.HttpStatus;
import vr.balance.app.exceptions.base.BaseException;

public class NotFoundException extends BaseException {

    public NotFoundException(String message) {
        super(message);
    }

    @Override
    public HttpStatus getStatus() {
        return HttpStatus.NOT_FOUND; // 404
    }
}