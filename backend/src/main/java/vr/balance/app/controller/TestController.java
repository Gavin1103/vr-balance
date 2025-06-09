package vr.balance.app.controller;

import io.swagger.v3.oas.annotations.Operation;
import io.swagger.v3.oas.annotations.security.SecurityRequirement;
import org.springframework.context.annotation.Profile;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.security.access.prepost.PreAuthorize;
import org.springframework.web.bind.annotation.*;
import vr.balance.app.repository.UserRepository;
import vr.balance.app.response.ApiStandardResponse;


@RestController
@RequestMapping("/api/test")
public class TestController {

    private final UserRepository userRepository;

    public TestController(UserRepository userRepository) {
        this.userRepository = userRepository;
    }

    @Operation(summary = "Test API endpoint")
    @GetMapping("public/test")
    public ResponseEntity<ApiStandardResponse<String>> test() {
        return ResponseEntity.ok(new ApiStandardResponse<>(
                HttpStatus.OK,
                "Test API endpoint"
        ));
    }

    @Profile("dev")
    @DeleteMapping("public/cleanup-cypress-test-user/emailCypress@vrbalance.com")
    @Operation(summary = "API endpoint to delete the test user from the cypress test")
    public ResponseEntity<ApiStandardResponse<String>> deleteTestUser() {
        userRepository.findByEmail("emailCypress@vrbalance.com").ifPresent(userRepository::delete);
        return ResponseEntity.ok(new ApiStandardResponse<>(
                HttpStatus.NO_CONTENT,
                "Successfully deleted test user"
        ));
    }
}
