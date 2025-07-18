package vr.balance.app.controller;

import io.swagger.v3.oas.annotations.Operation;
import io.swagger.v3.oas.annotations.security.SecurityRequirement;
import org.springframework.http.*;
import org.springframework.security.access.prepost.PreAuthorize;
import org.springframework.web.bind.annotation.*;
import vr.balance.app.DTO.request.CompletedBalanceTestExerciseDTO;
import vr.balance.app.DTO.request.CompletedFireflyExerciseDTO;
import vr.balance.app.DTO.request.base.CompletedExerciseDTO;
import vr.balance.app.DTO.response.BalanceTestResponse;
import vr.balance.app.DTO.response.CompletedExerciseResponse;
import vr.balance.app.enums.ExerciseEnum;
import vr.balance.app.exceptions.NotFoundException;
import vr.balance.app.models.exercise.CompletedBalanceTestExercise;
import vr.balance.app.models.exercise.CompletedExercise;
import vr.balance.app.models.exercise.CompletedFireflyExercise;
import vr.balance.app.response.ApiStandardResponse;
import vr.balance.app.service.AuthenticationService;
import vr.balance.app.service.CompletedExerciseService;

import java.io.ByteArrayOutputStream;
import java.io.IOException;
import java.nio.charset.StandardCharsets;
import java.util.HashMap;
import java.util.List;
import java.util.Map;
import java.util.zip.ZipEntry;
import java.util.zip.ZipOutputStream;

@RestController
@RequestMapping("/api/exercise")
public class ExerciseController {

    private final CompletedExerciseService completedExerciseService;
    private final AuthenticationService authenticationService;

    public ExerciseController(CompletedExerciseService completedExerciseService, AuthenticationService authenticationService) {
        this.completedExerciseService = completedExerciseService;
        this.authenticationService = authenticationService;
    }

    /**
     * Endpoint to store completed exercises that do not require any additional data.
     * <p>
     * This is intended for simple exercises such as:
     * - Squats
     * - Lunges
     * <p>
     * These exercises only use the base fields from CompletedExerciseDTO:
     * - exercise (enum)
     * - difficulty (enum)
     * - earnedPoints (int)
     * - completedAt (DateTime)
     * <p>
     * ⚠️ Do not use this endpoint for exercises that require extra tracking data,
     *    like the balance test or firefly exercise.
     */
    @PostMapping("/store-exercise/standard")
    @PreAuthorize("isAuthenticated()")
    @SecurityRequirement(name = "bearerAuth")
    @Operation(summary = "Store a completed standard exercise (no extra fields required)")
    public ResponseEntity<ApiStandardResponse<Void>> storeStandardExercise(@RequestBody CompletedExerciseDTO dto) {
        completedExerciseService.saveExercise(
                dto,
                authenticationService.getCurrentUserId()
        );

        return ResponseEntity.status(HttpStatus.CREATED)
                .body(new ApiStandardResponse<>(
                        HttpStatus.CREATED,
                        "Exercise saved successfully"
                ));
    }

    @PostMapping("/store-exercise/firefly")
    @PreAuthorize("isAuthenticated()")
    @SecurityRequirement(name = "bearerAuth")
    @Operation(summary = "Store a completed firefly exercise")
    public ResponseEntity<ApiStandardResponse<Void>> storeFireflyExercise(@RequestBody CompletedFireflyExerciseDTO dto) {
        completedExerciseService.saveExercise(
                dto,
                authenticationService.getCurrentUserId()
        );

        return ResponseEntity.status(HttpStatus.CREATED)
                .body(new ApiStandardResponse<>(
                        HttpStatus.CREATED,
                        "Exercise saved successfully"
                ));
    }

    @PostMapping("/store-exercise/balance-test")
    @PreAuthorize("isAuthenticated()")
    @SecurityRequirement(name = "bearerAuth")
    @Operation(summary = "Store a completed balance test exercise")
    public ResponseEntity<ApiStandardResponse<Void>> storeBalanceTestExercise(@RequestBody CompletedBalanceTestExerciseDTO dto) {
        completedExerciseService.saveExercise(
                dto,
                authenticationService.getCurrentUserId()
        );

        return ResponseEntity.status(HttpStatus.CREATED)
                .body(new ApiStandardResponse<>(
                        HttpStatus.CREATED,
                        "Exercise saved successfully"
                ));
    }

    @GetMapping("/fetch-last-10-exercises")
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

    @GetMapping("/download-balance-test/{id}")
    @PreAuthorize("isAuthenticated()")
    @SecurityRequirement(name = "bearerAuth")
    @Operation(summary = "Download the results (all phases) of a balance test as a zip")
    public ResponseEntity<byte[]> downloadBalanceTestResults(@PathVariable("id") Long id) throws IOException {
        byte[] zipBytes = completedExerciseService.generateZipWithPhases(id);

        HttpHeaders headers = new HttpHeaders();
        headers.setContentType(MediaType.APPLICATION_OCTET_STREAM);
        headers.setContentDisposition(ContentDisposition.attachment().filename("balance-test-results.zip").build());

        return new ResponseEntity<>(zipBytes, headers, HttpStatus.OK);
    }
}
