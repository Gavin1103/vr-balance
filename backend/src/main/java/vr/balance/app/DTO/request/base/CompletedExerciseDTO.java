package vr.balance.app.DTO.request.base;

import lombok.AllArgsConstructor;
import lombok.Data;
import lombok.NoArgsConstructor;
import lombok.experimental.SuperBuilder;
import vr.balance.app.enums.DifficultyEnum;

import java.time.Instant;

/**
 * Abstract base DTO for representing data of a completed exercise.
 * <p>
 * This class provides common properties shared by all completed exercise types,
 * including difficulty, earned points, and the timestamp of completion.
 * Subclasses should extend this DTO to represent specific types of exercises
 * (e.g. balance tests, firefly games).
 *
 * <p><strong>Note:</strong> This class is used to transfer completed exercise data
 * between the client and server and is not intended to be instantiated directly.
 */
@Data
@SuperBuilder
@NoArgsConstructor
@AllArgsConstructor
public abstract class CompletedExerciseDTO {
    DifficultyEnum difficulty;
    int earnedPoints;
    Instant completedAt;
}
