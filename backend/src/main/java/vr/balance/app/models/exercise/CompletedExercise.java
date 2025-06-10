package vr.balance.app.models.exercise;

import jakarta.persistence.*;
import lombok.*;
import lombok.experimental.SuperBuilder;
import org.modelmapper.internal.bytebuddy.implementation.bind.annotation.Super;
import vr.balance.app.enums.DifficultyEnum;
import vr.balance.app.enums.ExerciseEnum;
import vr.balance.app.models.User;

import java.time.Instant;


@Entity
@Inheritance(strategy = InheritanceType.JOINED)
@Table(name = "completed_exercise")
@Getter
@Setter
@NoArgsConstructor
@AllArgsConstructor
@SuperBuilder
public class CompletedExercise {

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