package vr.balance.app.repository.exercise;

import org.springframework.data.domain.Pageable;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.query.Param;
import vr.balance.app.DTO.response.BalanceTestResponse;
import vr.balance.app.enums.ExerciseEnum;
import vr.balance.app.models.exercise.CompletedBalanceTestExercise;

import java.util.List;

public interface CompletedBalanceTestExerciseRepository extends JpaRepository<CompletedBalanceTestExercise, Long> {
    long countByUserIdAndExercise(Long userId, ExerciseEnum exercise);

    @Query("""
                SELECT new vr.balance.app.DTO.response.BalanceTestResponse(
                    c.completedAt, c.phase_1, c.phase_2, c.phase_3, c.phase_4
                )
                FROM CompletedBalanceTestExercise c
                WHERE c.user.id = :userId
                ORDER BY c.completedAt DESC
            """)
    List<BalanceTestResponse> findTop5LatestByUser(@Param("userId") Long userId, Pageable pageable);
}
