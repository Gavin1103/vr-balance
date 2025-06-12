package vr.balance.app.DTO.request.base;

import lombok.*;
import lombok.experimental.SuperBuilder;
import vr.balance.app.enums.DifficultyEnum;
import vr.balance.app.enums.ExerciseEnum;

import java.time.Instant;

@Data
@SuperBuilder
@NoArgsConstructor
@AllArgsConstructor
@Getter
@Setter
public class CompletedExerciseDTO {
    ExerciseEnum exercise;
    DifficultyEnum difficulty;
    int earnedPoints;
    Instant completedAt;
}
