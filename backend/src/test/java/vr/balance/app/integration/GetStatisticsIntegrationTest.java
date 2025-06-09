package vr.balance.app.integration;

import com.fasterxml.jackson.databind.ObjectMapper;
import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.Test;
import org.junit.jupiter.api.extension.ExtendWith;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.boot.test.autoconfigure.web.servlet.AutoConfigureMockMvc;
import org.springframework.boot.test.context.SpringBootTest;
import org.springframework.core.env.Environment;
import org.springframework.test.context.ActiveProfiles;
import org.springframework.test.context.junit.jupiter.SpringExtension;
import org.springframework.test.web.servlet.MockMvc;
import org.springframework.test.web.servlet.ResultActions;

import java.util.Arrays;

import static org.springframework.test.web.servlet.request.MockMvcRequestBuilders.get;
import static org.springframework.test.web.servlet.result.MockMvcResultMatchers.jsonPath;
import static org.springframework.test.web.servlet.result.MockMvcResultMatchers.status;

@AutoConfigureMockMvc
@SpringBootTest
@ExtendWith(SpringExtension.class)
public class GetStatisticsIntegrationTest {

    @Autowired
    private MockMvc mockMvc;

    @Test
    void getStatistics_shouldReturnExerciseCount() throws Exception {
        // Arrange
        String ApiMessage = "Fetched successfully";
        String exerciseName = "FIREFLY_EXERCISE";

        // NOTE: Check the `createCompletedFireflyExercises()` method in the database seeder
        // to verify how many Firefly exercises are generated per user. The total is calculated as:
        // 6 users × 3 exercises = 18 exercises in total
        int exerciseCount = 18;

        // Act
        ResultActions result = mockMvc.perform(get("/api/statistics/public/exercise"));

        // Assert
        result.andExpect(status().isOk())
                .andExpect(jsonPath("$.message").value(ApiMessage))
                .andExpect(jsonPath("$.data").isArray())
                .andExpect(jsonPath("$.data[0].exercise").value(exerciseName))
                .andExpect(jsonPath("$.data[0].count").value(exerciseCount));
    }
}
