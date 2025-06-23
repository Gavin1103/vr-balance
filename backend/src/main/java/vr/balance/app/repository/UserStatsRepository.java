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

    /**
     * Retrieves the current and highest streak values for a given user by their ID.
     *
     * @param userId the ID of the user
     * @return an {@link Optional} containing a {@link UserStreakDTO} if found, otherwise empty
     */
    @Query("""
             SELECT new vr.balance.app.DTO.response.user_stats.UserStreakDTO(us.user.username, us.currentStreak, us.highestStreak)
             FROM UserStats us
             WHERE us.user.id = :userId
            """)
    Optional<UserStreakDTO> findStreakStatsByUserId(@Param("userId") Long userId);

    /**
     * Retrieves the top users ranked by their highest ever streaks.
     *
     * <p>Note: This method returns users ordered by their {@code highestStreak} in descending order.
     *
     * @param pageable a Pageable object with size set to 20 to limit the result
     * @return a list of {@link HighestStreakRankingDTO} objects
     */
    @Query("""
             SELECT new vr.balance.app.DTO.response.user_stats.HighestStreakRankingDTO(u.username, us.highestStreak)
             FROM UserStats us
             JOIN us.user u
             ORDER BY us.highestStreak DESC
            """)
    List<HighestStreakRankingDTO> findTop20ByHighestStreak(Pageable pageable);

    /**
     * Retrieves the top users ranked by their current streaks.
     *
     * <p>Note: This method orders by {@code currentStreak} in descending order.
     *
     * @param pageable a Pageable object with size set to 20 to limit the result
     * @return a list of {@link CurrentStreakRankingDTO} objects
     */
    @Query("""
             SELECT new vr.balance.app.DTO.response.user_stats.CurrentStreakRankingDTO(u.username, us.currentStreak)
             FROM UserStats us
             JOIN us.user u
             ORDER BY us.currentStreak DESC
            """)
    List<CurrentStreakRankingDTO> findTop20ByCurrentStreak(Pageable pageable);
}
