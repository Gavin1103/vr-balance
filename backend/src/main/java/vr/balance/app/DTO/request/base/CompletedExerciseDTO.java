package vr.balance.app.DTO.request.base;

import lombok.AllArgsConstructor;
import lombok.Data;
import lombok.NoArgsConstructor;
import lombok.experimental.SuperBuilder;
import vr.balance.app.enums.DifficultyEnum;
import vr.balance.app.enums.ExerciseEnum;

import java.time.Instant;

@Data
@SuperBuilder
@NoArgsConstructor
@AllArgsConstructor
public class CompletedExerciseDTO {
    ExerciseEnum exercise;
    DifficultyEnum difficulty;
    int earnedPoints;
    Instant completedAt;
}
