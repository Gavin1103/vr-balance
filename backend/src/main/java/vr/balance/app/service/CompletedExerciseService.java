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
import vr.balance.app.models.exercise.CompletedFireflyExercise;
import vr.balance.app.repository.UserRepository;
import vr.balance.app.repository.exercise.CompletedBalanceTestExerciseRepository;
import vr.balance.app.repository.exercise.CompletedExerciseRepository;
import vr.balance.app.util.Zip;

import java.io.IOException;
import java.util.List;
import java.util.Map;

import static vr.balance.app.enums.ExerciseEnum.*;

@Service
public class CompletedExerciseService {

    private final CompletedExerciseRepository completedExerciseRepository;
    private final UserRepository userRepository;
    private final UserStatsService userStatsService;
    private final ModelMapper modelMapper;
    private final CompletedBalanceTestExerciseRepository completedBalanceTestExerciseRepository;

    public CompletedExerciseService(
            CompletedExerciseRepository completedExerciseRepository,
            UserRepository userRepository,
            UserStatsService userStatsService,
            ModelMapper modelMapper, CompletedBalanceTestExerciseRepository completedBalanceTestExerciseRepository) {
        this.completedExerciseRepository = completedExerciseRepository;
        this.userRepository = userRepository;
        this.userStatsService = userStatsService;
        this.modelMapper = modelMapper;
        this.completedBalanceTestExerciseRepository = completedBalanceTestExerciseRepository;
    }

    /**
     * Saves a completed exercise to the database for a specific user.
     * <p>
     * This method dynamically maps a given {@link CompletedExerciseDTO} to its corresponding
     * {@link CompletedExercise} subclass using the exercise type provided in the DTO.
     * If the exercise is of a special type (e.g., balance test), extra processing is done.
     * Otherwise, user statistics are updated and the record is saved directly.
     *
     * @param dto     the data transfer object containing the completed exercise data
     * @param userId  the ID of the user who completed the exercise
     * @param <DTO>   the type of {@link CompletedExerciseDTO}
     * @throws UsernameNotFoundException if the user ID is not found in the repository
     */
    public <DTO extends CompletedExerciseDTO> void saveExercise(DTO dto, Long userId) {
        User user = userRepository.findById(userId)
                .orElseThrow(() -> new UsernameNotFoundException("User not found."));

        Class<? extends CompletedExercise> exerciseClass = getExerciseClass(dto.getExercise());
        CompletedExercise completedExercise = modelMapper.map(dto, exerciseClass);
        completedExercise.setUser(user);
        completedExercise.setExercise(dto.getExercise());

        if (completedExercise instanceof CompletedBalanceTestExercise balanceTest
                && dto instanceof CompletedBalanceTestExerciseDTO balanceTestDTO) {
            mapBalanceTestData(balanceTest, balanceTestDTO);
        } else {
            userStatsService.updateUserStats(completedExercise);
        }

        completedExerciseRepository.save(completedExercise);
    }

    /**
     * Returns the appropriate {@link CompletedExercise} subclass for the given exercise type.
     * <p>
     * This allows dynamic mapping of a DTO to its concrete entity class during persistence.
     *
     * @param exercise the exercise type from the {@link ExerciseEnum}
     * @return the corresponding entity class to use for mapping
     * @throws IllegalArgumentException if the exercise type is unknown
     */
    private Class<? extends CompletedExercise> getExerciseClass(ExerciseEnum exercise) {
        return switch (exercise) {
            case Balance -> CompletedBalanceTestExercise.class;
            case Firefly -> CompletedFireflyExercise.class;
            default -> CompletedExercise.class;
        };
    }

    /**
     * Maps detailed balance test phase data from the DTO to the entity.
     *
     * @param entity the balance test entity to populate
     * @param dto    the DTO containing the raw data
     */
    private void mapBalanceTestData(CompletedBalanceTestExercise entity, CompletedBalanceTestExerciseDTO dto) {
        try {
            ObjectMapper mapper = new ObjectMapper();
            entity.setPhase_1(mapper.writeValueAsString(dto.getPhase_1()));
            entity.setPhase_2(mapper.writeValueAsString(dto.getPhase_2()));
            entity.setPhase_3(mapper.writeValueAsString(dto.getPhase_3()));
            entity.setPhase_4(mapper.writeValueAsString(dto.getPhase_4()));
        } catch (JsonProcessingException e) {
            throw new RuntimeException("Failed to serialize balance test phase data", e);
        }
    }

    public List<CompletedExerciseResponse> getLast10CompletedExercises(Long userId) {
        User user = userRepository.findById(userId)
                .orElseThrow(() -> new NotFoundException("User not found."));

        // Fetch 10 most recent exercises (excluding balance tests) for this user
        List<CompletedExercise> recentExercises = completedExerciseRepository
                .findByUserIdAndExerciseNotOrderByCompletedAtDesc(
                        user.getId(),
                        Balance,
                        PageRequest.of(0, 10)
                );

        return recentExercises.stream()
                .map(exercise -> modelMapper.map(exercise, CompletedExerciseResponse.class))
                .toList();
    }

    /**
     * Generates a ZIP archive containing all four phase results of a balance test.
     *
     * <p>Each phase (phase_1 through phase_4) is written as a separate JSON file into the ZIP archive
     * under the folder {@code Results/}.
     *
     * <p>The filenames in the archive will be:
     * <ul>
     *     <li>Results/phase_1.json</li>
     *     <li>Results/phase_2.json</li>
     *     <li>Results/phase_3.json</li>
     *     <li>Results/phase_4.json</li>
     * </ul>
     *
     * @param id the ID of the {@link CompletedBalanceTestExercise} to download
     * @return a byte array representing the ZIP file containing all phase results
     * @throws NotFoundException if no balance test is found for the given ID
     * @throws IOException if an I/O error occurs while creating the ZIP file
     */
    public byte[] generateZipWithPhases(Long id) throws IOException {
        CompletedBalanceTestExercise test = completedBalanceTestExerciseRepository.findById(id)
                .orElseThrow(() -> new NotFoundException("Balance test not found"));

        Map<String, String> files = Map.of(
                "phase_1.json", test.getPhase_1(),
                "phase_2.json", test.getPhase_2(),
                "phase_3.json", test.getPhase_3(),
                "phase_4.json", test.getPhase_4()
        );

        return Zip.createZipFromMap("Results/", files);
    }
}