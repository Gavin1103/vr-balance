package vr.balance.app.unit.service;

import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.Test;
import org.mockito.ArgumentCaptor;
import org.springframework.test.context.ActiveProfiles;
import vr.balance.app.enums.DifficultyEnum;
import vr.balance.app.enums.ExerciseEnum;
import vr.balance.app.models.User;
import vr.balance.app.models.UserStats;
import vr.balance.app.models.exercise.CompletedFireflyExercise;
import vr.balance.app.repository.UserStatsRepository;
import vr.balance.app.repository.exercise.CompletedExerciseRepository;
import vr.balance.app.service.UserStatsService;

import java.time.Instant;
import java.time.LocalDate;

import static org.junit.jupiter.api.Assertions.assertEquals;
import static org.mockito.Mockito.*;

class UserStatsServiceTest {

    private UserStatsService userStatsService;
    private UserStatsRepository userStatsRepository;
    private CompletedExerciseRepository completedExerciseRepository;

    @BeforeEach
    void setUp() {
        userStatsRepository = mock(UserStatsRepository.class);
        completedExerciseRepository = mock(CompletedExerciseRepository.class);
        userStatsService = new UserStatsService(userStatsRepository, completedExerciseRepository, null);
    }

    @Test
    void testUpdateExistingStats_updatesValuesCorrectly() {
        // Arrange
        User user = new User();
        user.setId(1L);

        UserStats stats = UserStats.builder()
                .user(user)
                .totalPoints(100)
                .currentStreak(2)
                .highestStreak(3)
                .totalExercises(5)
                .build();

        Instant yesterday = Instant.now().minusSeconds(86400);
        Instant now = Instant.now();

        CompletedFireflyExercise lastExercise = CompletedFireflyExercise.builder()
                .user(user)
                .earnedPoints(0)
                .completedAt(yesterday)
                .build();

        CompletedFireflyExercise newExercise = CompletedFireflyExercise.builder()
                .user(user)
                .earnedPoints(50)
                .completedAt(now)
                .build();

        when(userStatsRepository.findByUser(user)).thenReturn(stats);
        when(completedExerciseRepository.findFirstByUserOrderByCompletedAtDesc(user)).thenReturn(lastExercise);

        // Act
        userStatsService.updateUserStats(newExercise);

        // Assert
        ArgumentCaptor<UserStats> captor = ArgumentCaptor.forClass(UserStats.class);
        verify(userStatsRepository).save(captor.capture());

        UserStats updatedStats = captor.getValue();

        assertEquals(150, updatedStats.getTotalPoints());
        assertEquals(3, updatedStats.getCurrentStreak());
        assertEquals(3, updatedStats.getHighestStreak());
        assertEquals(6, updatedStats.getTotalExercises());
    }

    @Test
    void testCreateNewStats_createsStatsCorrectly() {
        // Arrange
        User user = new User();
        user.setId(1L);

        CompletedFireflyExercise exercise = CompletedFireflyExercise.builder()
                .user(user)
                .exercise(ExerciseEnum.FIREFLY_EXERCISE)
                .difficulty(DifficultyEnum.EASY)
                .earnedPoints(100)
                .completedAt(Instant.now())
                .build();

        when(userStatsRepository.findByUser(user)).thenReturn(null);

        // Act
        userStatsService.updateUserStats(exercise);

        // Assert
        ArgumentCaptor<UserStats> captor = ArgumentCaptor.forClass(UserStats.class);
        verify(userStatsRepository).save(captor.capture());

        UserStats savedStats = captor.getValue();

        assertEquals(user, savedStats.getUser());
        assertEquals(100, savedStats.getTotalPoints());
        assertEquals(1, savedStats.getCurrentStreak());
        assertEquals(1, savedStats.getHighestStreak());
        assertEquals(1, savedStats.getTotalExercises());
    }

    //    Test streak
    @Test
    void testSameDay_returnsCurrentStreak() {
        LocalDate date = LocalDate.now();
        int result = userStatsService.calculateNewStreak(date, date, 3);
        assertEquals(3, result);
    }

    @Test
    void testNextDay_incrementsStreak() {
        LocalDate yesterday = LocalDate.now().minusDays(1);
        LocalDate today = LocalDate.now();
        int result = userStatsService.calculateNewStreak(yesterday, today, 2);
        assertEquals(3, result);
    }

    @Test
    void testStreakBroken_resetsToOne() {
        LocalDate twoDaysAgo = LocalDate.now().minusDays(2);
        LocalDate today = LocalDate.now();
        int result = userStatsService.calculateNewStreak(twoDaysAgo, today, 5);
        assertEquals(1, result);
    }
}