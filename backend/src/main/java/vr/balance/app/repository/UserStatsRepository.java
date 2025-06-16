package vr.balance.app.repository;

import org.springframework.data.domain.Pageable;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.query.Param;
import vr.balance.app.DTO.response.user_stats.CurrentStreakRankingDTO;
import vr.balance.app.DTO.response.user_stats.HighestStreakRankingDTO;
import vr.balance.app.DTO.response.user_stats.UserStreakDTO;
import vr.balance.app.models.User;
import vr.balance.app.models.UserStats;

import java.util.List;
import java.util.Optional;

public interface UserStatsRepository extends JpaRepository<UserStats, Long> {
    UserStats findByUser(User user);

    @Query("""
                SELECT new vr.balance.app.DTO.response.user_stats.UserStreakDTO(us.user.username,us.currentStreak, us.highestStreak)
                FROM UserStats us
                WHERE us.user.id = :userId
            """)
    Optional<UserStreakDTO> findStreakStatsByUserId(@Param("userId") Long userId);

    @Query("""
                SELECT new vr.balance.app.DTO.response.user_stats.HighestStreakRankingDTO(u.username, us.currentStreak)
                FROM UserStats us
                JOIN us.user u
                ORDER BY us.highestStreak ASC
            """)
    List<HighestStreakRankingDTO> findTop20ByHighestStreak(Pageable pageable);

    @Query("""
    SELECT new vr.balance.app.DTO.response.user_stats.CurrentStreakRankingDTO(u.username, us.highestStreak)
    FROM UserStats us
    JOIN us.user u
    ORDER BY us.currentStreak ASC
""")
    List<CurrentStreakRankingDTO> findTop20ByCurrentStreak(Pageable pageable);
}
