package vr.balance.app.controller;

import io.swagger.v3.oas.annotations.Operation;
import io.swagger.v3.oas.annotations.media.Content;
import io.swagger.v3.oas.annotations.media.Schema;
import io.swagger.v3.oas.annotations.responses.ApiResponse;
import io.swagger.v3.oas.annotations.responses.ApiResponses;
import io.swagger.v3.oas.annotations.security.SecurityRequirement;
import jakarta.validation.Valid;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.security.access.prepost.PreAuthorize;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;
import vr.balance.app.DTO.request.user.*;
import vr.balance.app.DTO.response.JwtResponse;
import vr.balance.app.response.ApiErrorResponse;
import vr.balance.app.response.ApiStandardResponse;
import vr.balance.app.service.AuthenticationService;

@RestController
@RequestMapping("/api/auth")
public class AuthenticationController {

    private final AuthenticationService authenticationService;

    public AuthenticationController(AuthenticationService authenticationService) {
        this.authenticationService = authenticationService;
    }

    /**
     * Endpoint to authenticate a user via the website and return a JWT token.
     */
    @PostMapping("/login")
    @Operation(
            summary = "Login with email and password",
            description = "Authenticates a user and returns a JWT token if credentials are valid."
    )
    @ApiResponses(value = {
            @ApiResponse(
                    responseCode = "200",
                    description = "Login successful"
            ),
            @ApiResponse(
                    responseCode = "401",
                    description = "Invalid email or password",
                    content = @Content(mediaType = "application/json",
                            schema = @Schema(implementation = ApiErrorResponse.class))
            ),
            @ApiResponse(
                    responseCode = "400",
                    description = "Request failed due to missing login credentials",
                    content = @Content(mediaType = "application/json",
                            schema = @Schema(implementation = ApiErrorResponse.class))
            )
    })
    public ResponseEntity<ApiStandardResponse<JwtResponse>> login(
            @io.swagger.v3.oas.annotations.parameters.RequestBody(
                    description = "login-credential data",
                    required = true,
                    content = @Content(schema = @Schema(implementation = LoginDTO.class))
            )
            @Valid @RequestBody LoginDTO request) {
        String token = authenticationService.login(request.getEmail(), request.getPassword());
        JwtResponse response = new JwtResponse(token);

        return ResponseEntity.ok(new ApiStandardResponse<>(
                HttpStatus.OK,
                "Login successful",
                response
        ));
    }

    /**
     * Endpoint to authenticate a user via unity and return a JWT token.
     */
    @PostMapping("/login-pincode")
    @Operation(
            summary = "Login with email/username and pincode (for login via unity app)",
            description = "Authenticates a user and returns a JWT token if credentials are valid."
    )
    @ApiResponses(value = {
            @ApiResponse(
                    responseCode = "200",
                    description = "Login successful"
            ),
            @ApiResponse(
                    responseCode = "401",
                    description = "Invalid email or password",
                    content = @Content(mediaType = "application/json",
                            schema = @Schema(implementation = ApiErrorResponse.class))
            ),
            @ApiResponse(
                    responseCode = "400",
                    description = "Request failed due to missing login credentials",
                    content = @Content(mediaType = "application/json",
                            schema = @Schema(implementation = ApiErrorResponse.class))
            )
    })
    public ResponseEntity<ApiStandardResponse<JwtResponse>> loginWithPincode(@Valid @RequestBody LoginWithPincodeDTO request) {
        String token = authenticationService.loginWithPincode(request.getIdentifier(), request.getPincode());
        JwtResponse response = new JwtResponse(token);

        return ResponseEntity.ok(new ApiStandardResponse<>(
                HttpStatus.OK,
                "Login successful",
                response
        ));
    }

    /**
     * Endpoint to register a user.
     */
    @PostMapping("/register-patient")
    @PreAuthorize("hasAnyRole('ADMIN', 'PHYSIOTHERAPIST')")
    @SecurityRequirement(name = "bearerAuth")
    @Operation(
            summary = "Register a new patient",
            description = "Allows an admin or physiotherapist to register a new patient in the system."
    )
    @ApiResponses(value = {
            @ApiResponse(
                    responseCode = "201",
                    description = "Successfully created patient",
                    content = @Content(mediaType = "application/json", schema = @Schema(implementation = ApiStandardResponse.class))
            ),
            @ApiResponse(
                    responseCode = "403",
                    description = "Access denied – not authorized to perform this action",
                    content = @Content(mediaType = "application/json", schema = @Schema(implementation = ApiErrorResponse.class))
            ),
            @ApiResponse(
                    responseCode = "409",
                    description = "Email or username already in use",
                    content = @Content(mediaType = "application/json", schema = @Schema(implementation = ApiErrorResponse.class))
            ),
            @ApiResponse(
                    responseCode = "400",
                    description = "Request failed due to missing data",
                    content = @Content(mediaType = "application/json", schema = @Schema(implementation = ApiErrorResponse.class))
            )
    })
    public ResponseEntity<ApiStandardResponse<Void>> registerPatient(
            @io.swagger.v3.oas.annotations.parameters.RequestBody(
                    description = "Patient registration data",
                    required = true,
                    content = @Content(schema = @Schema(implementation = RegisterPatientDTO.class))
            )
            @Valid @RequestBody RegisterPatientDTO request
    ) {
        authenticationService.registerPatient(request);

        return ResponseEntity
                .status(HttpStatus.CREATED)
                .body(new ApiStandardResponse<>(
                        HttpStatus.CREATED,
                        "Successfully created patient"
                ));
    }

    /**
     * Endpoint to change password
     */
    @PostMapping("/change-password")
    @SecurityRequirement(name = "bearerAuth")
    @PreAuthorize("isAuthenticated()")
    @Operation(summary = "Change password")
    @ApiResponses(value = {
            @ApiResponse(
                    responseCode = "200",
                    description = "Successfully updated password",
                    content = @Content(mediaType = "application/json", schema = @Schema(implementation = ApiStandardResponse.class))
            ),
            @ApiResponse(
                    responseCode = "403",
                    description = "Incorrect current password",
                    content = @Content(mediaType = "application/json", schema = @Schema(implementation = ApiErrorResponse.class))
            ),
            @ApiResponse(
                    responseCode = "400",
                    description = "Request failed due to missing data, or new-password and repeat-new-password do not match",
                    content = @Content(mediaType = "application/json", schema = @Schema(implementation = ApiErrorResponse.class))
            ),
            @ApiResponse(
                    responseCode = "401",
                    description = "Unauthorized - you need to be logged in",
                    content = @Content(mediaType = "application/json", schema = @Schema(implementation = ApiErrorResponse.class))
            ),
    })
    public ResponseEntity<ApiStandardResponse<Void>> changePassword(@Valid @RequestBody ChangePasswordDTO request) {
        Long userId = authenticationService.getCurrentUserId();
        authenticationService.changePassword(request, userId);

        return ResponseEntity.ok(new ApiStandardResponse<>(
                HttpStatus.OK,
                "Successfully updated password"
        ));
    }

    /**
     * Endpoint to change password
     */
    @PostMapping("/change-pincode")
    @SecurityRequirement(name = "bearerAuth")
    @PreAuthorize("isAuthenticated()")
    @Operation(summary = "Change pincode")
    @ApiResponses(value = {
            @ApiResponse(
                    responseCode = "200",
                    description = "Successfully updated pincode",
                    content = @Content(mediaType = "application/json", schema = @Schema(implementation = ApiStandardResponse.class))
            ),
            @ApiResponse(
                    responseCode = "403",
                    description = "Incorrect current pincode",
                    content = @Content(mediaType = "application/json", schema = @Schema(implementation = ApiErrorResponse.class))
            ),
            @ApiResponse(
                    responseCode = "400",
                    description = "Request failed due to missing data, or new-pincode and repeat-new-pincode do not match",
                    content = @Content(mediaType = "application/json", schema = @Schema(implementation = ApiErrorResponse.class))
            ),
            @ApiResponse(
                    responseCode = "401",
                    description = "Unauthorized - you need to be logged in",
                    content = @Content(mediaType = "application/json", schema = @Schema(implementation = ApiErrorResponse.class))
            ),
    })
    public ResponseEntity<ApiStandardResponse<Void>> changePincode(@Valid @RequestBody ChangePincodeDTO request) {
        Long userId = authenticationService.getCurrentUserId();
        authenticationService.changePincode(request, userId);

        return ResponseEntity.ok(new ApiStandardResponse<>(
                HttpStatus.OK,
                "Successfully updated pincode"
        ));
    }
}
