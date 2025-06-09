package vr.balance.app.API.request.user;

import jakarta.validation.constraints.NotBlank;
import lombok.Builder;
import lombok.Data;
import lombok.Getter;
import lombok.Setter;

@Data
@Getter
@Setter
@Builder
public class ChangePasswordDTO {
    @NotBlank(message = "is required")
    String currentPassword;

    @NotBlank(message = "is required")
    String newPassword;

    @NotBlank(message = "is required")
    String repeatNewPassword;
}
