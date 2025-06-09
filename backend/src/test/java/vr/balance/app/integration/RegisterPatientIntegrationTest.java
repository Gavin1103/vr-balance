package vr.balance.app.integration;

import com.fasterxml.jackson.databind.ObjectMapper;
import org.junit.jupiter.api.Test;
import org.junit.jupiter.api.extension.ExtendWith;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.boot.test.autoconfigure.web.servlet.AutoConfigureMockMvc;
import org.springframework.boot.test.context.SpringBootTest;
import org.springframework.http.MediaType;
import org.springframework.security.core.userdetails.UsernameNotFoundException;
import org.springframework.test.annotation.Rollback;
import org.springframework.test.context.junit.jupiter.SpringExtension;
import org.springframework.test.web.servlet.MockMvc;
import org.springframework.test.web.servlet.ResultActions;
import org.springframework.transaction.annotation.Transactional;
import vr.balance.app.API.request.user.RegisterPatientDTO;
import vr.balance.app.models.User;
import vr.balance.app.repository.UserRepository;
import vr.balance.app.security.JwtService;

import java.time.LocalDate;
import java.util.Optional;

import static org.assertj.core.api.AssertionsForClassTypes.assertThat;
import static org.springframework.test.web.servlet.result.MockMvcResultMatchers.status;
import static org.springframework.test.web.servlet.request.MockMvcRequestBuilders.post;
import static org.springframework.test.web.servlet.result.MockMvcResultMatchers.jsonPath;
import static vr.balance.app.enums.AuthenticationMethodEnum.STANDARD;

@AutoConfigureMockMvc
@SpringBootTest
@ExtendWith(SpringExtension.class)
@Transactional
@Rollback
public class RegisterPatientIntegrationTest {

    @Autowired
    private MockMvc mockMvc;

    @Autowired
    private ObjectMapper objectMapper;

    @Autowired
    private JwtService jwtService;

    @Autowired
    private UserRepository userRepository;

    @Test
    void registerPatient_shouldSucceed_whenAuthorizedAsAdmin() throws Exception {
        // ---------- ARRANGE ----------

        // Haal de admin-gebruiker op uit de database (deze moet al bestaan als testdata)
        User user = userRepository.findByEmail("admin@vrbalance.com")
                .orElseThrow(() -> new UsernameNotFoundException("User not found"));

        // Genereer een geldig JWT-token voor de user(admin@vrbalance.com)
        String token = jwtService.generateToken(user, STANDARD.toString());

        // Stel het e-mailadres van de nieuwe patiënt in (wordt later gebruikt voor validatie)
        String email = "patient@example.com";

        // Bouw het request-object dat de nieuwe patiënt representeert
        RegisterPatientDTO dto = RegisterPatientDTO.builder()
                .username("newPatient")
                .email(email)
                .firstName("John")
                .lastName("Doe")
                .birthDate(LocalDate.of(1990, 1, 1))
                .build();

        // Zet het request-object om naar JSON zodat het in de HTTP-request kan worden verstuurd
        String payload = objectMapper.writeValueAsString(dto);

        // ---------- ACT ----------

        // Voer een POST-request uit naar het beveiligde register-endpoint met JWT-authenticatie
        ResultActions result = mockMvc.perform(post("/api/auth/register-patient")
                .header("Authorization", "Bearer " + token) // Simuleert ingelogde admin
                .contentType(MediaType.APPLICATION_JSON)
                .content(payload));

        // ---------- ASSERT (Response validatie) ----------

        // Controleer of de response correct is:
        // - HTTP status 201 Created
        // - Message in de response klopt
        // - Statusveld in de JSON is ook 201
        result.andExpect(status().isCreated())
                .andExpect(jsonPath("$.message").value("Successfully created patient"))
                .andExpect(jsonPath("$.status").value(201));

        // ---------- ASSERT (Database validatie) ----------

        // Controleer of de patiënt daadwerkelijk is opgeslagen in de database
        Optional<User> registered = userRepository.findByEmail(email);

        // De gebruiker moet bestaan en de waarden moeten overeenkomen met wat is doorgestuurd
        assertThat(registered).isPresent();
        assertThat(registered.get().getUsername()).isEqualTo("newPatient");
        assertThat(registered.get().getFirstName()).isEqualTo("John");
        assertThat(registered.get().getRole().name()).isEqualTo("PATIENT");
    }

    @Test
    void registerPatient_shouldReturnUnauthorized_whenNoTokenProvided() throws Exception {
        mockMvc.perform(post("/api/auth/register-patient")
                        .contentType(MediaType.APPLICATION_JSON)
                        .content("{}"))
                .andExpect(status().isUnauthorized());
    }

    @Test
    void registerPatient_shouldReturnForbidden_whenRoleIsPatient() throws Exception {
        // Arrange
        User patientUser = userRepository.findByEmail("john@vrbalance.com").orElseThrow(() -> new UsernameNotFoundException("User not found"));
        String token = jwtService.generateToken(patientUser, STANDARD.toString());

        RegisterPatientDTO dto = RegisterPatientDTO.builder()
                .username("newPatient")
                .email("patient@example.com")
                .firstName("John")
                .lastName("Doe")
                .birthDate(LocalDate.of(1990, 1, 1))
                .build();

        String payload = objectMapper.writeValueAsString(dto);

        // Act & Assert
        mockMvc.perform(post("/api/auth/register-patient")
                        .header("Authorization", "Bearer " + token)
                        .contentType(MediaType.APPLICATION_JSON)
                        .content(payload))
                .andExpect(status().isForbidden());
    }

    @Test
    void registerPatient_shouldReturnBadRequest_whenIncompleteFields() throws Exception {
        // Arrange
        User user = userRepository.findByEmail("admin@vrbalance.com").orElseThrow(() -> new UsernameNotFoundException("User not found"));
        String token = jwtService.generateToken(user, STANDARD.toString());

        // Act & Assert
        mockMvc.perform(post("/api/auth/register-patient")
                        .header("Authorization", "Bearer " + token)
                        .contentType(MediaType.APPLICATION_JSON)
                        .content("{}"))
                .andExpect(status().isBadRequest());
    }
}
