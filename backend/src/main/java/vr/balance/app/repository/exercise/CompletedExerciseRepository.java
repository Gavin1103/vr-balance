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

    /**
     * Retrieves the most recent completed exercise for a given user that occurred before a specified time,
     * excluding a specific exercise type.
     * <p>
     * This is useful for streak calculations or progression tracking, where certain exercise types (e.g., BALANCE)
     * should not influence the user's stats.
     *
     * @param user             the user whose completed exercise history is being queried
     * @param excludedExercise the {@link ExerciseEnum} type to exclude from the result (e.g., {@code ExerciseEnum.Balance})
     * @param completedAt      the timestamp before which the exercise must have been completed
     * @return the most recent {@link CompletedExercise} that matches the criteria, or {@code null} if none found
     */
    CompletedExercise findFirstByUserAndExerciseNotAndCompletedAtBeforeOrderByCompletedAtDesc(
            User user,
            ExerciseEnum excludedExercise,
            Instant completedAt
    );

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


    /**
     * Retrieves the top 10 highest scores for a given exercise, returning only
     * each user's highest score (no duplicates per user).
     *
     * <p>This native SQL query:
     * <ul>
     *   <li>Finds the highest earned points per user for the specified exercise</li>
     *   <li>Joins with the users table to fetch usernames</li>
     *   <li>Orders by score descending</li>
     *   <li>Limits the result to the top 10 scores</li>
     * </ul>
     *
     * @param exerciseName the name of the exercise (as stored in the database)
     * @return a list of Object arrays containing: [exercise, earned_points, username]
     */
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


    /**
     * Retrieves a paginated list of completed exercises for a given user,
     * excluding exercises of the specified type, ordered by completion date descending.
     *
     * <p>This is useful for finding the most recent non-test exercises (e.g. not BalanceTest).
     *
     * @param userId the ID of the user
     * @param exercise the exercise type to exclude (e.g. ExerciseEnum.Balance)
     * @param pageable pagination information (e.g. page size = 1 to get the latest only)
     * @return a list of {@link CompletedExercise} records
     */
    List<CompletedExercise> findByUserIdAndExerciseNotOrderByCompletedAtDesc(Long userId, ExerciseEnum exercise, Pageable pageable);

    CompletedExercise findFirstByUserOrderByCompletedAtDesc(User user);
}
