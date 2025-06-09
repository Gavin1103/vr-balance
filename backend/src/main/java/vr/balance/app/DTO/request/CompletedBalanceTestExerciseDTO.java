package vr.balance.app.DTO.request;

import lombok.Data;
import lombok.EqualsAndHashCode;
import lombok.NoArgsConstructor;
import lombok.experimental.SuperBuilder;
import vr.balance.app.DTO.request.base.CompletedExerciseDTO;

import java.util.List;

@EqualsAndHashCode(callSuper = true)
@Data
@SuperBuilder
@NoArgsConstructor
public class CompletedBalanceTestExerciseDTO extends CompletedExerciseDTO {
    private List<Vector3> phase_1;
    private List<Vector3> phase_2;
    private List<Vector3> phase_3;
    private List<Vector3> phase_4;
}
