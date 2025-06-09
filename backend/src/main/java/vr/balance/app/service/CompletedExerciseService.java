package vr.balance.app.service;

import org.modelmapper.ModelMapper;
import org.springframework.security.core.userdetails.UsernameNotFoundException;
import org.springframework.stereotype.Service;
import vr.balance.app.API.request.base.CompletedExerciseDTO;
import vr.balance.app.API.response.LeaderboardExerciseResponse;
import vr.balance.app.enums.ExerciseEnum;
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
    private final ModelMapper mapper;
    private final UserStatsService userStatsService;

    public CompletedExerciseService(
            CompletedExerciseRepository completedExerciseRepository,
            UserRepository userRepository,
            ModelMapper mapper,
            UserStatsService userStatsService) {
        this.completedExerciseRepository = completedExerciseRepository;
        this.userRepository = userRepository;
        this.mapper = mapper;
        this.userStatsService = userStatsService;
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
     * @param <DTO>         the type of {@link CompletedExerciseDTO} (must extend CompletedExerciseDTO)
     * @throws UsernameNotFoundException if the user with the provided ID does not exist
     */
    public <CE extends CompletedExercise, DTO extends CompletedExerciseDTO> void saveExercise(Class<CE> entityClass, DTO dto, Long userId, ExerciseEnum exerciseEnum) {
        User user = userRepository.findById(userId)
                .orElseThrow(() -> new UsernameNotFoundException("User not found."));

        CE completedExercise = mapper.map(dto, entityClass);
        completedExercise.setUser(user);
        completedExercise.setExercise(exerciseEnum);

        if (entityClass != CompletedBalanceTestExercise.class) {
            userStatsService.updateUserStats(completedExercise);
        }

        completedExerciseRepository.save(completedExercise);
    }
}