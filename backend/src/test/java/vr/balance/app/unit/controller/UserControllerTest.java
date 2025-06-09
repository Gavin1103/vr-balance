package vr.balance.app.unit.controller;

import org.junit.jupiter.api.Test;
import org.junit.jupiter.api.extension.ExtendWith;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.boot.test.autoconfigure.web.servlet.AutoConfigureMockMvc;
import org.springframework.boot.test.context.SpringBootTest;
import org.springframework.http.MediaType;
import static org.springframework.test.web.servlet.request.MockMvcRequestBuilders.get;

import org.springframework.security.test.context.support.WithMockUser;
import org.springframework.test.context.bean.override.mockito.MockitoBean;
import org.springframework.test.context.junit.jupiter.SpringExtension;
import org.springframework.test.web.servlet.MockMvc;
import vr.balance.app.API.response.UserProfileResponse;
import vr.balance.app.service.AuthenticationService;
import vr.balance.app.service.UserService;

import static org.mockito.Mockito.verify;
import static org.mockito.Mockito.when;
import static org.springframework.test.web.servlet.result.MockMvcResultMatchers.status;
import static org.springframework.test.web.servlet.result.MockMvcResultMatchers.jsonPath;

@AutoConfigureMockMvc
@SpringBootTest
@ExtendWith(SpringExtension.class)
public class UserControllerTest {

    @Autowired
    private MockMvc mockMvc;

    @MockitoBean
    private UserService userService;

    @MockitoBean
    private AuthenticationService authenticationService;

    @Test
    @WithMockUser(username = "testuser", roles = {"PATIENT"})
    void getCurrentUser_shouldReturnProfileSuccessfully() throws Exception {
        // Arrange
        Long userId = 1L;
        UserProfileResponse response = new UserProfileResponse();
        response.setUsername("john_doe");
        response.setEmail("john@example.com");

        when(authenticationService.getCurrentUserId()).thenReturn(userId);
        when(userService.getUserProfileById(userId)).thenReturn(response);

        // Act & Assert
        mockMvc.perform(get("/api/user/me")
                        .contentType(MediaType.APPLICATION_JSON))
                .andExpect(status().isOk())
                .andExpect(jsonPath("$.message").value("Successfully retrieved profile"))
                .andExpect(jsonPath("$.data.username").value("john_doe"))
                .andExpect(jsonPath("$.data.email").value("john@example.com"));

        verify(authenticationService).getCurrentUserId();
        verify(userService).getUserProfileById(userId);
    }

    @Test
    void getCurrentUser_shouldReturn401_whenNotAuthenticated() throws Exception {
        mockMvc.perform(get("/api/user/me")
                        .contentType(MediaType.APPLICATION_JSON))
                .andExpect(status().isUnauthorized());
    }
}
