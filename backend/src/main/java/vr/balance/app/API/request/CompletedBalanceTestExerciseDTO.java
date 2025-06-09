package vr.balance.app.API.request;

import lombok.Data;
import lombok.EqualsAndHashCode;
import lombok.experimental.SuperBuilder;
import vr.balance.app.API.request.base.CompletedExerciseDTO;

@EqualsAndHashCode(callSuper = true)
@Data
@SuperBuilder
public class CompletedBalanceTestExerciseDTO extends CompletedExerciseDTO {
    String phase_1;
    String phase_2;
    String phase_3;
    String phase_4;
}
