package vr.balance.app.API.request.user;

import jakarta.validation.constraints.NotBlank;
import lombok.Data;
import lombok.Getter;
import lombok.Setter;

@Getter
@Setter
@Data
public class LoginWithPincodeDTO {
    @NotBlank(message = "Username or email required")
    private String identifier;

    @NotBlank(message = "Pincode is required")
    private String pincode;
}
