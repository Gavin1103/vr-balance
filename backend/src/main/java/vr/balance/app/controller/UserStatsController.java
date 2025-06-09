package vr.balance.app.controller;

import io.swagger.v3.oas.annotations.Operation;
import io.swagger.v3.oas.annotations.responses.ApiResponse;
import io.swagger.v3.oas.annotations.responses.ApiResponses;
import org.springframework.data.domain.Page;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.RestController;
import vr.balance.app.DTO.response.user_stats.UserStatsResponse;
import vr.balance.app.response.ApiStandardResponse;
import vr.balance.app.service.UserStatsService;

@RestController
@RequestMapping("/api/user-stats")
public class UserStatsController {

    private final UserStatsService userStatsService;

    public UserStatsController(UserStatsService userStatsService) {
        this.userStatsService = userStatsService;
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
}
