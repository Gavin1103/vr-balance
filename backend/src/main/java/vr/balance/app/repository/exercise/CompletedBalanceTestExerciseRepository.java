package vr.balance.app.repository.exercise;

import org.springframework.data.jpa.repository.JpaRepository;
import vr.balance.app.enums.ExerciseEnum;
import vr.balance.app.models.exercise.CompletedBalanceTestExercise;

public interface CompletedBalanceTestExerciseRepository extends JpaRepository<CompletedBalanceTestExercise, Long> {
    boolean existsByUserIdAndExercise(Long userId, ExerciseEnum exercise);
}
