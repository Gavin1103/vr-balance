package vr.balance.app.repository.exercise;

import org.springframework.data.jpa.repository.JpaRepository;
import vr.balance.app.models.exercise.CompletedFireflyExercise;

import java.time.Instant;
import java.util.Optional;

public interface CompletedFireflyExerciseRepository extends JpaRepository<CompletedFireflyExercise, Long> {
    long countByUserId(Long userId);

    Optional<CompletedFireflyExercise> findByUserIdAndCompletedAt(Long userId, Instant completedAt);
}
