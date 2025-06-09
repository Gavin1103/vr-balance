package vr.balance.app.controller;

import io.swagger.v3.oas.annotations.Operation;
import io.swagger.v3.oas.annotations.media.Content;
import io.swagger.v3.oas.annotations.media.Schema;
import io.swagger.v3.oas.annotations.responses.ApiResponse;
import io.swagger.v3.oas.annotations.responses.ApiResponses;
import io.swagger.v3.oas.annotations.security.SecurityRequirement;
import jakarta.validation.Valid;
import org.springframework.data.domain.Page;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.security.access.prepost.PreAuthorize;
import org.springframework.web.bind.annotation.*;
import vr.balance.app.API.request.user.EditUserDTO;
import vr.balance.app.API.response.UserProfileResponse;
import vr.balance.app.response.ApiErrorResponse;
import vr.balance.app.response.ApiStandardResponse;
import vr.balance.app.service.AuthenticationService;
import vr.balance.app.service.UserService;

@RestController
@RequestMapping("/api/user")
public class UserController {

    private final UserService userService;
    private final AuthenticationService authenticationService;

    public UserController(UserService userService, AuthenticationService authenticationService) {
        this.userService = userService;
        this.authenticationService = authenticationService;
    }

    /**
     * Endpoint to retrieve profile information of the logged-in user
     */
    @GetMapping("/me")
    @SecurityRequirement(name = "bearerAuth")
    @PreAuthorize("isAuthenticated()")
    @Operation(summary = "Get the profile of the logged-in user")
    @ApiResponses(value = {
            @ApiResponse(
                    responseCode = "200",
                    description = "Successfully retrieved profile"
            ),
            @ApiResponse(
                    responseCode = "404",
                    description = "User not found",
                    content = @Content(mediaType = "application/json",
                            schema = @Schema(implementation = ApiErrorResponse.class))
            ),
            @ApiResponse(
                    responseCode = "401",
                    description = "Unauthorized - you need to be logged in",
                    content = @Content(mediaType = "application/json",
                            schema = @Schema(implementation = ApiErrorResponse.class))
            )
    })
    public ResponseEntity<ApiStandardResponse<UserProfileResponse>> getCurrentUser() {
        Long userId = authenticationService.getCurrentUserId();
        UserProfileResponse profile = userService.getUserProfileById(userId);

        return ResponseEntity.ok(new ApiStandardResponse<>(
                HttpStatus.OK,
                "Successfully retrieved profile",
                profile
        ));
    }

    /**
     * Endpoint to edit the logged-in user (me)
     */
    @PostMapping("/edit-me")
    @SecurityRequirement(name = "bearerAuth")
    @PreAuthorize("isAuthenticated()")
    @Operation(summary = "Edit the profile logged-in user")
    @ApiResponses(value = {
            @ApiResponse(
                    responseCode = "200",
                    description = "Successfully updated the user profile"
            ),
            @ApiResponse(
                    responseCode = "404",
                    description = "User not found",
                    content = @Content(mediaType = "application/json", schema = @Schema(implementation = ApiErrorResponse.class))
            ),
            @ApiResponse(
                    responseCode = "401",
                    description = "Unauthorized - you need to be logged in",
                    content = @Content(mediaType = "application/json", schema = @Schema(implementation = ApiErrorResponse.class))
            ),
            @ApiResponse(
                    responseCode = "400",
                    description = "Request failed due to missing data",
                    content = @Content(mediaType = "application/json", schema = @Schema(implementation = ApiErrorResponse.class))
            ),
            @ApiResponse(
                    responseCode = "409",
                    description = "Email or username already in use",
                    content = @Content(mediaType = "application/json", schema = @Schema(implementation = ApiErrorResponse.class))
            ),
    })
    public ResponseEntity<ApiStandardResponse<Void>> editCurrentUser(@Valid @RequestBody EditUserDTO request) {
        Long userId = authenticationService.getCurrentUserId();
        userService.editUserProfile(request, userId);

        return ResponseEntity.ok(new ApiStandardResponse<>(
                HttpStatus.OK,
                "Successfully updated the user profile"
        ));
    }

    /**
     * Endpoint to fetch users with pagination
     */
    @GetMapping("/get-all-users")
    @PreAuthorize("hasAnyRole('ADMIN', 'PHYSIOTHERAPIST')")
    @SecurityRequirement(name = "bearerAuth")
    @Operation(summary = "Fetch all users with pagination")
    @ApiResponses(value = {
            @ApiResponse(
                    responseCode = "200",
                    description = "Successfully fetched users"
            ),
            @ApiResponse(
                    responseCode = "401",
                    description = "Unauthorized - you need to be logged in",
                    content = @Content(mediaType = "application/json", schema = @Schema(implementation = ApiErrorResponse.class))
            ),
            @ApiResponse(
                    responseCode = "403",
                    description = "Forbidden - you dont have the right permissions",
                    content = @Content(mediaType = "application/json", schema = @Schema(implementation = ApiErrorResponse.class))
            ),
    })
    public ResponseEntity<ApiStandardResponse<Page<UserProfileResponse>>> getAllUsers(
            @RequestParam(defaultValue = "0") int page,
            @RequestParam(defaultValue = "10") int size
    ) {
        Page<UserProfileResponse> users = userService.getAllUserProfiles(page, size);

        return ResponseEntity.ok(new ApiStandardResponse<>(
                HttpStatus.OK,
                "Successfully fetched user profiles",
                users
        ));
    }
}
