package vr.balance.app.DTO.request.user;

import jakarta.validation.constraints.NotBlank;
import lombok.Data;
import lombok.Getter;
import lombok.Setter;

import java.time.LocalDate;

@Getter
@Setter
@Data
public class EditUserDTO {
    @NotBlank(message = "Email is required")
    private String email;

    @NotBlank(message = "Username is required")
    private String username;

    private String firstName;
    private String lastName;
    private LocalDate birthDate;
    private String phoneNumber;
}
