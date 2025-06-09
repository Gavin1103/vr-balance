package vr.balance.app.API.response;

import lombok.AllArgsConstructor;
import lombok.Data;
import vr.balance.app.enums.ExerciseEnum;

import java.time.Instant;

@Data
@AllArgsConstructor
public class ExerciseStatsResponse {
    private ExerciseEnum exercise;
    private long count;
}
