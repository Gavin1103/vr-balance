package vr.balance.app.DTO.response;

import lombok.AllArgsConstructor;
import lombok.Data;
import vr.balance.app.enums.ExerciseEnum;

@Data
@AllArgsConstructor
public class ExerciseStatsResponse {
    private ExerciseEnum exercise;
    private long count;
}
