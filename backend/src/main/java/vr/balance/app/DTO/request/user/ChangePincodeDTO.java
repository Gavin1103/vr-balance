package vr.balance.app.DTO.request.user;

import jakarta.validation.constraints.NotBlank;
import lombok.Data;
import lombok.Getter;
import lombok.Setter;

@Data
@Getter
@Setter
public class ChangePincodeDTO {
    @NotBlank(message = "is required")
    String currentPincode;

    @NotBlank(message = "is required")
    String newPincode;

    @NotBlank(message = "is required")
    String RepeatNewPincode;
}
