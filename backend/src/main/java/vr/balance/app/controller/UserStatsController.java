package vr.balance.app.controller;

import io.swagger.v3.oas.annotations.Operation;
import io.swagger.v3.oas.annotations.media.Content;
import io.swagger.v3.oas.annotations.media.Schema;
import io.swagger.v3.oas.annotations.responses.ApiResponse;
import io.swagger.v3.oas.annotations.responses.ApiResponses;
import io.swagger.v3.oas.annotations.security.SecurityRequirement;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.PageRequest;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.security.access.prepost.PreAuthorize;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.RestController;
import vr.balance.app.DTO.response.user_stats.CurrentStreakRankingDTO;
import vr.balance.app.DTO.response.user_stats.HighestStreakRankingDTO;
import vr.balance.app.DTO.response.user_stats.UserStatsResponse;
import vr.balance.app.DTO.response.user_stats.UserStreakDTO;
import vr.balance.app.response.ApiErrorResponse;
import vr.balance.app.response.ApiStandardResponse;
import vr.balance.app.service.AuthenticationService;
import vr.balance.app.service.UserStatsService;

import java.util.List;

@RestController
@RequestMapping("/api/user-stats")
public class UserStatsController {

    private final UserStatsService userStatsService;
    private final AuthenticationService authenticationService;

    public UserStatsController(UserStatsService userStatsService, AuthenticationService authenticationService) {
        this.userStatsService = userStatsService;
        this.authenticationService = authenticationService;
    }

    /**
     * Endpoint to fetch the leaderboard
     */
    @GetMapping("/public/leaderboard")
    @Operation(summary = "retrieve the leaderboard")
    @ApiResponses(value = {
            @ApiResponse(
                    responseCode = "200",
                    description = "Successfully fetched the leaderboard"
            )
    })
    public ResponseEntity<ApiStandardResponse<Page<UserStatsResponse>>> getLeaderboard(
            @RequestParam(defaultValue = "0") int page,
            @RequestParam(defaultValue = "10") int size,
            @RequestParam(defaultValue = "totalPoints") String sortBy,
            @RequestParam(defaultValue = "desc") String direction
    ) {
        Page<UserStatsResponse> leaderboard = userStatsService.getAllUserStats(page, size, sortBy, direction);
        return ResponseEntity.ok(new ApiStandardResponse<>(
                HttpStatus.OK,
                "Successfully fetched leaderboard",
                leaderboard
        ));
    }

    /**
     * Endpoint to fetch the current and highest streak of the logged-in user
     */
    @GetMapping("/get-streak")
    @Operation(summary = "Fetch the highest and current streak of the logged in user")
    @PreAuthorize("isAuthenticated()")
    @SecurityRequirement(name = "bearerAuth")
    @ApiResponses(value = {
            @ApiResponse(
                    responseCode = "200",
                    description = "Successfully fetched the streak"
            ),
            @ApiResponse(
                    responseCode = "401",
                    description = "Unauthorized - you need to be logged in",
                    content = @Content(mediaType = "application/json", schema = @Schema(implementation = ApiErrorResponse.class))
            ),
    })
    public ResponseEntity<ApiStandardResponse<UserStreakDTO>> getStreak() {
        Long userId = authenticationService.getCurrentUserId();
        UserStreakDTO userStreak = userStatsService.getUserStreak(userId);
        return ResponseEntity.ok(new ApiStandardResponse<>(
                HttpStatus.OK,
                "Successfully fetched streak",
                userStreak
        ));
    }

    /**
     * Endpoint to fetch the top 20 current streaks
     */
    @GetMapping("/public/top-20-current-streaks")
    @Operation(summary = "Fetch the top 20 current streaks")
    @ApiResponses(value = {
            @ApiResponse(
                    responseCode = "200",
                    description = "Successfully fetched the top 20 current streak"
            )
    })
    public ResponseEntity<ApiStandardResponse<List<CurrentStreakRankingDTO>>> fetchTop20CurrentStreak() {
        List<CurrentStreakRankingDTO> currentStreaks = userStatsService.getTop20CurrentStreak();

        return ResponseEntity.ok(new ApiStandardResponse<>(
                HttpStatus.OK,
                "Successfully fetched the top 20 current streak",
                currentStreaks
        ));
    }

    /**
     * Endpoint to fetch the top 20 highest streaks
     */
    @GetMapping("/public/top-20-highest-streaks")
    @Operation(summary = "Fetch the top 20 current streaks")
    @ApiResponses(value = {
            @ApiResponse(
                    responseCode = "200",
                    description = "Successfully fetched the top 20 highest streaks"
            )
    })
    public ResponseEntity<ApiStandardResponse<List<HighestStreakRankingDTO>>> fetchTop20HighestStreak() {
        List<HighestStreakRankingDTO> highestStreaks = userStatsService.getTop20HighestStreak();
        return ResponseEntity.ok(new ApiStandardResponse<>(
                HttpStatus.OK,
                "Successfully fetched the top 20 highest streaks",
                highestStreaks
        ));
    }
}
