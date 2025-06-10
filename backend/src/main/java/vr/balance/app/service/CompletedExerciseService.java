package vr.balance.app.service;

import com.fasterxml.jackson.core.JsonProcessingException;
import com.fasterxml.jackson.databind.ObjectMapper;
import org.modelmapper.ModelMapper;
import org.springframework.data.domain.PageRequest;
import org.springframework.security.core.userdetails.UsernameNotFoundException;
import org.springframework.stereotype.Service;
import vr.balance.app.DTO.request.CompletedBalanceTestExerciseDTO;
import vr.balance.app.DTO.request.base.CompletedExerciseDTO;
import vr.balance.app.DTO.response.CompletedExerciseResponse;
import vr.balance.app.enums.ExerciseEnum;
import vr.balance.app.exceptions.NotFoundException;
import vr.balance.app.models.User;
import vr.balance.app.models.exercise.CompletedBalanceTestExercise;
import vr.balance.app.models.exercise.CompletedExercise;
import vr.balance.app.repository.UserRepository;
import vr.balance.app.repository.exercise.CompletedExerciseRepository;

import java.util.List;

@Service
public class CompletedExerciseService {

    private final CompletedExerciseRepository completedExerciseRepository;
    private final UserRepository userRepository;
    private final UserStatsService userStatsService;
    private final ModelMapper modelMapper;

    public CompletedExerciseService(
            CompletedExerciseRepository completedExerciseRepository,
            UserRepository userRepository,
            UserStatsService userStatsService,
            ModelMapper modelMapper) {
        this.completedExerciseRepository = completedExerciseRepository;
        this.userRepository = userRepository;
        this.userStatsService = userStatsService;
        this.modelMapper = modelMapper;
    }

    /**
     * Stores a completed exercise entry for a given user.
     * <p>
     * This method is generic and supports saving any exercise type that extends {@link CompletedExercise},
     * using the corresponding DTO that extends {@link CompletedExerciseDTO}. It allows flexible saving of
     * both standard and custom exercises (e.g. balance tests, firefly, etc.).
     * <p>
     * The provided {@link ExerciseEnum} is used to identify the exercise type, and {@code entityClass}
     * determines the specific persistence class to be used. User lookup and exercise mapping are performed internally.
     *
     * @param entityClass   the concrete entity class (e.g., {@code CompletedFireflyExercise.class}) to persist
     * @param dto           the data transfer object containing the completed exercise data
     * @param userId        the ID of the user who completed the exercise
     * @param exerciseEnum  the exercise type, used to resolve related exercise metadata
     * @param <CE>          a class extending {@link CompletedExercise}
     * @param <DTO>         a class extending {@link CompletedExerciseDTO}
     *
     * @throws UsernameNotFoundException if no user is found with the given ID
     */
    public <CE extends CompletedExercise, DTO extends CompletedExerciseDTO> void saveExercise(
            Class<CE> entityClass, DTO dto, Long userId, ExerciseEnum exerciseEnum) {

        User user = userRepository.findById(userId)
                .orElseThrow(() -> new UsernameNotFoundException("User not found."));

        CE completedExercise = modelMapper.map(dto, entityClass);
        completedExercise.setUser(user);
        completedExercise.setExercise(exerciseEnum);

        if (completedExercise instanceof CompletedBalanceTestExercise balanceTest) {
            try {
                ObjectMapper mapper = new ObjectMapper();
                balanceTest.setPhase_1(mapper.writeValueAsString(((CompletedBalanceTestExerciseDTO) dto).getPhase_1()));
                balanceTest.setPhase_2(mapper.writeValueAsString(((CompletedBalanceTestExerciseDTO) dto).getPhase_2()));
                balanceTest.setPhase_3(mapper.writeValueAsString(((CompletedBalanceTestExerciseDTO) dto).getPhase_3()));
                balanceTest.setPhase_4(mapper.writeValueAsString(((CompletedBalanceTestExerciseDTO) dto).getPhase_4()));
            } catch (JsonProcessingException e) {
                throw new RuntimeException("Failed to serialize phase data", e);
            }
        } else {
            userStatsService.updateUserStats(completedExercise);
        }

        completedExerciseRepository.save(completedExercise);
    }

    public List<CompletedExerciseResponse> getLast10CompletedExercises(Long userId) {
        User user = userRepository.findById(userId)
                .orElseThrow(() -> new NotFoundException("User not found."));

        // Fetch 10 most recent exercises (excluding balance tests) for this user
        List<CompletedExercise> recentExercises = completedExerciseRepository
                .findByUserIdAndExerciseNotOrderByCompletedAtDesc(
                        user.getId(),
                        ExerciseEnum.BALANCE_TEST_EXERCISE,
                        PageRequest.of(0, 10)
                );

        return recentExercises.stream()
                .map(exercise -> modelMapper.map(exercise, CompletedExerciseResponse.class))
                .toList();
    }
}