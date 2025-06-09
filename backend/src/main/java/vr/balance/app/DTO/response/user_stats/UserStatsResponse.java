package vr.balance.app.DTO.response.user_stats;

import lombok.Data;
import lombok.Getter;
import lombok.Setter;

@Data
@Setter
@Getter
public class UserStatsResponse {
    private int highestStreak;
    private int currentStreak;
    private int totalPoints;
    private int totalExercises;
    private String username;
}
