package vr.balance.app.repository.exercise;

import org.springframework.data.domain.Pageable;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.query.Param;
import vr.balance.app.DTO.response.ExerciseStatsResponse;
import vr.balance.app.enums.ExerciseEnum;
import vr.balance.app.models.User;
import vr.balance.app.models.exercise.CompletedExercise;

import java.time.Instant;
import java.util.List;

public interface CompletedExerciseRepository extends JpaRepository<CompletedExercise, Long> {

    CompletedExercise findFirstByUserOrderByCompletedAtDesc(User user);

    /**
     * Retrieves aggregated statistics for all completed exercises grouped by exercise type,
     * excluding the {@code BALANCE_TEST_EXERCISE}.
     *
     * @return A list of {@link ExerciseStatsResponse} containing the exercise type and the total count,
     * ordered by the number of completions in descending order.
     */
    @Query("""
                SELECT new vr.balance.app.DTO.response.ExerciseStatsResponse(
                    ce.exercise,
                    COUNT(ce)
                )
                FROM CompletedExercise ce
                WHERE ce.exercise <> vr.balance.app.enums.ExerciseEnum.Balance
                GROUP BY ce.exercise
                ORDER BY COUNT(ce) DESC
            """)
    List<ExerciseStatsResponse> findAllTimeStats();

    /**
     * Retrieves aggregated statistics for all completed exercises grouped by exercise type
     * that were completed within the given time range, excluding the {@code BALANCE_TEST_EXERCISE}.
     *
     * @param from The start of the time range (inclusive).
     * @param to   The end of the time range (inclusive).
     * @return A list of {@link ExerciseStatsResponse} containing the exercise type and the count
     * of completions within the given date range, ordered by count descending.
     */
    @Query("""
                SELECT new vr.balance.app.DTO.response.ExerciseStatsResponse(
                    ce.exercise,
                    COUNT(ce)
                )
                FROM CompletedExercise ce
                WHERE ce.completedAt BETWEEN :from AND :to
                  AND ce.exercise <> vr.balance.app.enums.ExerciseEnum.Balance
                GROUP BY ce.exercise
                ORDER BY COUNT(ce) DESC
            """)
    List<ExerciseStatsResponse> findStatsByDateRange(
            @Param("from") Instant from,
            @Param("to") Instant to
    );

    @Query(value = """
            SELECT ce.exercise, ce.earned_points, u.username
            FROM completed_exercise ce
            JOIN users u ON ce.user_id = u.id
            JOIN (
                SELECT user_id, MAX(earned_points) AS max_score
                FROM completed_exercise
                WHERE exercise = :exercise
                GROUP BY user_id
            ) sub ON ce.user_id = sub.user_id AND ce.earned_points = sub.max_score
            WHERE ce.exercise = :exercise
            ORDER BY ce.earned_points DESC
            LIMIT 10
            """, nativeQuery = true)
    List<Object[]> findTop10HighestScoresPerUser(@Param("exercise") String exerciseName);


    List<CompletedExercise> findByUserIdAndExerciseNotOrderByCompletedAtDesc(Long userId, ExerciseEnum exercise, Pageable pageable);
}
