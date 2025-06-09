package vr.balance.app.controller;

import io.swagger.v3.oas.annotations.Operation;
import io.swagger.v3.oas.annotations.security.SecurityRequirement;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.security.access.prepost.PreAuthorize;
import org.springframework.web.bind.annotation.*;
import vr.balance.app.DTO.request.CompletedBalanceTestExerciseDTO;
import vr.balance.app.DTO.request.CompletedFireflyExerciseDTO;
import vr.balance.app.DTO.response.CompletedExerciseResponse;
import vr.balance.app.enums.ExerciseEnum;
import vr.balance.app.models.exercise.CompletedBalanceTestExercise;
import vr.balance.app.models.exercise.CompletedFireflyExercise;
import vr.balance.app.response.ApiStandardResponse;
import vr.balance.app.service.AuthenticationService;
import vr.balance.app.service.CompletedExerciseService;

import java.util.List;

@RestController
@RequestMapping("/api/exercise")
public class ExerciseController {

    private final CompletedExerciseService completedExerciseService;
    private final AuthenticationService authenticationService;

    public ExerciseController(CompletedExerciseService completedExerciseService, AuthenticationService authenticationService) {
        this.completedExerciseService = completedExerciseService;
        this.authenticationService = authenticationService;
    }

    @PostMapping("/store-exercise/firefly")
    @PreAuthorize("isAuthenticated()")
    @SecurityRequirement(name = "bearerAuth")
    public ResponseEntity<ApiStandardResponse<Void>> storeFirefly(@RequestBody CompletedFireflyExerciseDTO dto) {
        completedExerciseService.saveExercise(
                CompletedFireflyExercise.class,
                dto,
                authenticationService.getCurrentUserId(),
                ExerciseEnum.FIREFLY_EXERCISE
        );

        return ResponseEntity
                .status(HttpStatus.CREATED)
                .body(new ApiStandardResponse<>(
                        HttpStatus.CREATED,
                        "Exercise saved successfully"
                ));
    }

    @PostMapping("/store-exercise/balance-test")
    @PreAuthorize("isAuthenticated()")
    @SecurityRequirement(name = "bearerAuth")
    public ResponseEntity<ApiStandardResponse<Void>> storeBalanceTest(@RequestBody CompletedBalanceTestExerciseDTO dto) {
        completedExerciseService.saveExercise(
                CompletedBalanceTestExercise.class,
                dto,
                authenticationService.getCurrentUserId(),
                ExerciseEnum.BALANCE_TEST_EXERCISE
        );

        return ResponseEntity
                .status(HttpStatus.CREATED)
                .body(new ApiStandardResponse<>(
                        HttpStatus.CREATED,
                        "Exercise saved successfully"
                ));
    }

    @PostMapping("/fetch-last-10-exercises")
    @PreAuthorize("isAuthenticated()")
    @SecurityRequirement(name = "bearerAuth")
    @Operation(summary = "Fetch the last 10 completedExercises of the logged-in user")
    public ResponseEntity<ApiStandardResponse<List<CompletedExerciseResponse>>> fetchLast10Exercises() {
        List<CompletedExerciseResponse> exercises = completedExerciseService.getLast10CompletedExercises(authenticationService.getCurrentUserId());

        return ResponseEntity
                .status(HttpStatus.OK)
                .body(new ApiStandardResponse<>(
                        HttpStatus.OK,
                        "Successfully fetched last 10 exercises",
                        exercises
                ));
    }
}
