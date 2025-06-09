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
     * Saves a completed exercise history record for a given user.
     * <p>
     * This method uses generics to allow saving any type of {@link CompletedExercise} based on the specific
     * {@link CompletedExerciseDTO} provided. The mapping is handled using a generic entity class and a DTO,
     * and the corresponding exercise metadata is resolved via {@link ExerciseEnum}.
     *
     * @param entityClass  the concrete class type of the exercise history entity (e.g. CompletedFireflyExercise.class)
     * @param dto          the data transfer object containing the completed exercise data
     * @param userId       the ID of the user who completed the exercise
     * @param exerciseEnum the enum representing the exercise type, used to look up the exercise entity
     * @param <CE>         the type of {@link CompletedExercise} (must extend CompletedExercise)
     * @param <DTO>        the type of {@link CompletedExerciseDTO} (must extend CompletedExerciseDTO)
     * @throws UsernameNotFoundException if the user with the provided ID does not exist
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