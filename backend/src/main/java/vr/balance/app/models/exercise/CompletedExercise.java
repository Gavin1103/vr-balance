package vr.balance.app.models.exercise;

import jakarta.persistence.*;
import lombok.*;
import lombok.experimental.SuperBuilder;
import org.modelmapper.internal.bytebuddy.implementation.bind.annotation.Super;
import vr.balance.app.enums.DifficultyEnum;
import vr.balance.app.enums.ExerciseEnum;
import vr.balance.app.models.User;

import java.time.Instant;

/**
 * Abstract base class representing a completed exercise session.
 * <p>
 * This class is used as the superclass for all specific types of completed exercises,
 * such as balance tests or firefly exercises. It stores common metadata such as the
 * associated user, which exercise, completion timestamp, difficulty level, and
 * earned points.
 * <p>
 * This class is mapped using the {@code JOINED} inheritance strategy, meaning each subclass
 * will be stored in its own table and joined with this base table via their shared ID.
 *
 * <p><strong>Note:</strong> This class should not be instantiated directly.
 */
@Entity
@Inheritance(strategy = InheritanceType.JOINED)
@Table(name = "completed_exercise")
@Getter
@Setter
@NoArgsConstructor
@AllArgsConstructor
@SuperBuilder
public abstract class CompletedExercise {

    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private Long id;

    @Enumerated(EnumType.STRING)
    private ExerciseEnum exercise;

    @ManyToOne
    @JoinColumn(name = "user_id")
    private User user;

    private int earnedPoints;

    @Enumerated(EnumType.STRING)
    private DifficultyEnum difficulty;

    private Instant completedAt;
}