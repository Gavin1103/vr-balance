package vr.balance.app.DTO.response;

import lombok.Data;
import vr.balance.app.enums.DifficultyEnum;
import vr.balance.app.enums.ExerciseEnum;

import java.time.Instant;

@Data
public class CompletedExerciseResponse {
    ExerciseEnum exercise;
    int earnedPoints;
    Instant completedAt;
    DifficultyEnum difficulty;
}
