package vr.balance.app.controller;

import io.swagger.v3.oas.annotations.Operation;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;
import vr.balance.app.DTO.response.ExerciseStatsResponse;
import vr.balance.app.DTO.response.LeaderboardExerciseResponse;
import vr.balance.app.enums.ExerciseEnum;
import vr.balance.app.response.ApiStandardResponse;
import vr.balance.app.service.StatisticsService;

import java.util.List;

@RestController
@RequestMapping("/api/statistics")
public class StatisticsController {

    private final StatisticsService statisticsService;

    public StatisticsController(StatisticsService statisticsService) {
        this.statisticsService = statisticsService;
    }

    @GetMapping("/public/exercise")
    @Operation(
            summary = "Get exercise statistics",
            description = "Returns the number of completed exercises per exercise type,"
    )
    public ResponseEntity<ApiStandardResponse<List<ExerciseStatsResponse>>> getStats() {
        List<ExerciseStatsResponse> stats = statisticsService.getAllTimeStats();
        return ResponseEntity.ok(new ApiStandardResponse<>(HttpStatus.OK,
                "Fetched successfully",
                stats
        ));
    }

    @GetMapping("/public/exercise/last-30-days")
    @Operation(
            summary = "Get exercise statistics from the last 30 days",
            description = "Returns the number of completed exercises per exercise type, filtered by a date range."
    )
    public ResponseEntity<ApiStandardResponse<List<ExerciseStatsResponse>>> getStatsFromLast30Days() {
        List<ExerciseStatsResponse> stats = statisticsService.getStatsFromLast30Days();
        return ResponseEntity.ok(new ApiStandardResponse<>(HttpStatus.OK,
                "Fetched successfully",
                stats
        ));
    }

    @GetMapping("/public/firefly-leaderboard")
    public ResponseEntity<ApiStandardResponse<List<LeaderboardExerciseResponse>>> getLeaderboardForFirefly() {
        List<LeaderboardExerciseResponse> leaderboard = statisticsService.getLeaderboardForExercise(ExerciseEnum.FIREFLY_EXERCISE);
        return ResponseEntity.ok(new ApiStandardResponse<>(HttpStatus.OK,
                "Fetched successfully",
                leaderboard
        ));
    }
}
