package vr.balance.app.API.response;

import lombok.Data;
import vr.balance.app.enums.RoleEnum;

import java.time.LocalDate;

@Data
public class UserProfileResponse {
    private String username;
    private String email;
    private String firstName;
    private String lastName;
    private String phoneNumber;
    private RoleEnum role;
    private LocalDate birthDate;
}
