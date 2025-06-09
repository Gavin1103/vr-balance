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
import vr.balance.app.DTO.request.CompletedFireflyExerciseDTO;
import vr.balance.app.enums.DifficultyEnum;
import vr.balance.app.models.User;
import vr.balance.app.models.exercise.CompletedFireflyExercise;
import vr.balance.app.repository.UserRepository;
import vr.balance.app.repository.exercise.CompletedFireflyExerciseRepository;
import vr.balance.app.security.JwtService;

import java.time.Instant;
import java.util.Optional;

import static org.assertj.core.api.AssertionsForClassTypes.assertThat;
import static org.springframework.test.web.servlet.request.MockMvcRequestBuilders.post;
import static org.springframework.test.web.servlet.result.MockMvcResultMatchers.jsonPath;
import static org.springframework.test.web.servlet.result.MockMvcResultMatchers.status;
import static vr.balance.app.enums.AuthenticationMethodEnum.STANDARD;

@AutoConfigureMockMvc
@SpringBootTest
@ExtendWith(SpringExtension.class)
@Transactional
@Rollback
public class SaveFireflyExerciseIntegrationTest {

    @Autowired
    private MockMvc mockMvc;

    @Autowired
    private ObjectMapper objectMapper;

    @Autowired
    private JwtService jwtService;

    @Autowired
    private UserRepository userRepository;

    @Autowired
    private CompletedFireflyExerciseRepository completedFireflyExerciseRepository;

    @Test
    void saveFireflyExercise_shouldSucceed() throws Exception {
        // Arrange
        User user = userRepository.findByEmail("admin@vrbalance.com")
                .orElseThrow(() -> new UsernameNotFoundException("User not found"));
        String token = jwtService.generateToken(user, STANDARD.toString());

        Instant now = Instant.now();

        CompletedFireflyExerciseDTO dto = CompletedFireflyExerciseDTO.builder()
                .difficulty(DifficultyEnum.EASY)
                .earnedPoints(300)
                .completedAt(now)
                .caughtFirefliesCount(34)
                .caughtWrongFirefliesCount(10)
                .build();

        String payload = objectMapper.writeValueAsString(dto);

        // Act
        ResultActions result = mockMvc.perform(post("/api/exercise/store-exercise/firefly")
                .header("Authorization", "Bearer " + token)
                .contentType(MediaType.APPLICATION_JSON)
                .content(payload));

        // Assert (HTTP)
        result.andExpect(status().isCreated())
                .andExpect(jsonPath("$.message").value("Exercise saved successfully"))
                .andExpect(jsonPath("$.status").value(201));

        // Assert (Database)
        Optional<CompletedFireflyExercise> stored = completedFireflyExerciseRepository
                .findByUserIdAndCompletedAt(user.getId(), now);

        assertThat(stored).isPresent();
        assertThat(stored.get().getEarnedPoints()).isEqualTo(300);
        assertThat(stored.get().getDifficulty()).isEqualTo(DifficultyEnum.EASY);
        assertThat(stored.get().getCaughtFirefliesCount()).isEqualTo(34);
        assertThat(stored.get().getCaughtWrongFirefliesCount()).isEqualTo(10);
    }
}
