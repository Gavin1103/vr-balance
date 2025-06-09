package vr.balance.app.controller;

import io.swagger.v3.oas.annotations.security.SecurityRequirement;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.security.access.prepost.PreAuthorize;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;
import vr.balance.app.DTO.request.CompletedBalanceTestExerciseDTO;
import vr.balance.app.DTO.request.CompletedFireflyExerciseDTO;
import vr.balance.app.enums.ExerciseEnum;
import vr.balance.app.models.exercise.CompletedBalanceTestExercise;
import vr.balance.app.models.exercise.CompletedFireflyExercise;
import vr.balance.app.response.ApiStandardResponse;
import vr.balance.app.service.AuthenticationService;
import vr.balance.app.service.CompletedExerciseService;

@RestController
@RequestMapping("/api")
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
                ));    }
}
