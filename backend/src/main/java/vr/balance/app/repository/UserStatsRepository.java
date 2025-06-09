package vr.balance.app.repository;

import org.springframework.data.jpa.repository.JpaRepository;
import vr.balance.app.models.User;
import vr.balance.app.models.UserStats;

public interface UserStatsRepository extends JpaRepository<UserStats, Long> {
    UserStats findByUser(User user);
}
