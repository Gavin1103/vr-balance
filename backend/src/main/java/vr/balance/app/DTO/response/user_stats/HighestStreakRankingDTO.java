package vr.balance.app.DTO.response.user_stats;

import lombok.AllArgsConstructor;
import lombok.Data;

@Data
@AllArgsConstructor
public class HighestStreakRankingDTO {
    String username;
    int highestStreak;
}
