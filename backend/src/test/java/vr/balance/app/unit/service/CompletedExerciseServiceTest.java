package vr.balance.app.unit.service;

import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.Test;
import org.modelmapper.ModelMapper;
import vr.balance.app.DTO.request.CompletedFireflyExerciseDTO;
import vr.balance.app.enums.DifficultyEnum;
import vr.balance.app.enums.ExerciseEnum;
import vr.balance.app.models.User;
import vr.balance.app.models.exercise.CompletedFireflyExercise;
import vr.balance.app.repository.UserRepository;
import vr.balance.app.repository.exercise.CompletedBalanceTestExerciseRepository;
import vr.balance.app.repository.exercise.CompletedExerciseRepository;
import vr.balance.app.service.CompletedExerciseService;
import vr.balance.app.service.UserStatsService;

import java.time.Instant;
import java.util.Optional;

import static org.junit.jupiter.api.Assertions.assertEquals;
import static org.mockito.Mockito.*;
import static org.mockito.Mockito.mock;

public class CompletedExerciseServiceTest {

    private UserRepository userRepository;
    private CompletedExerciseRepository completedExerciseRepository;
    private CompletedExerciseService completedExerciseService;
    private CompletedBalanceTestExerciseRepository completedBalanceTestExerciseRepository;
    private ModelMapper modelMapper;
    private UserStatsService userStatsService;



    @BeforeEach
    void setUp() {
        userRepository = mock(UserRepository.class);
        completedExerciseRepository = mock(CompletedExerciseRepository.class);
        completedBalanceTestExerciseRepository = mock(CompletedBalanceTestExerciseRepository.class);
        modelMapper = mock(ModelMapper.class);
        userStatsService = mock(UserStatsService.class);

        completedExerciseService = new CompletedExerciseService(
                completedExerciseRepository,
                userRepository,
                userStatsService,
                modelMapper,
                completedBalanceTestExerciseRepository
        );
    }

    @Test
    void testSaveExercise_savesExerciseAndUpdatesStats() {
        // Arrange
        Long userId = 1L;
        User user = new User();
        user.setId(userId);

        CompletedFireflyExerciseDTO dto = CompletedFireflyExerciseDTO.builder()
                .exercise(ExerciseEnum.Firefly)
                .earnedPoints(100)
                .completedAt(Instant.now())
                .difficulty(DifficultyEnum.MEDIUM)
                .caughtFirefliesCount(10)
                .caughtWrongFirefliesCount(3)
                .build();

        CompletedFireflyExercise mappedEntity = new CompletedFireflyExercise();

        when(userRepository.findById(userId)).thenReturn(Optional.of(user));
        when(modelMapper.map(dto, CompletedFireflyExercise.class)).thenReturn(mappedEntity);

        // Act
        completedExerciseService.saveExercise(dto, userId);

        // Assert
        assertEquals(user, mappedEntity.getUser());
        assertEquals(ExerciseEnum.Firefly, mappedEntity.getExercise());

        verify(userStatsService).updateUserStats(mappedEntity);
        verify(completedExerciseRepository).save(mappedEntity);
    }
}
