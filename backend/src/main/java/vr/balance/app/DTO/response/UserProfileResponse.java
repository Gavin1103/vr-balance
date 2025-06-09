package vr.balance.app.DTO.response;

import lombok.Data;
import vr.balance.app.enums.RoleEnum;

import java.time.LocalDate;

@Data
public class UserProfileResponse {
    private Long id;
    private String username;
    private String email;
    private String firstName;
    private String lastName;
    private String phoneNumber;
    private RoleEnum role;
    private LocalDate birthDate;
}
