package vr.balance.app.service;

import org.modelmapper.ModelMapper;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.PageRequest;
import org.springframework.data.domain.Pageable;
import org.springframework.security.core.userdetails.UsernameNotFoundException;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;
import vr.balance.app.DTO.request.user.EditUserDTO;
import vr.balance.app.DTO.response.BalanceTestResponse;
import vr.balance.app.DTO.response.CompletedExerciseResponse;
import vr.balance.app.DTO.response.PatientDetailResponse;
import vr.balance.app.DTO.response.UserProfileResponse;
import vr.balance.app.enums.ExerciseEnum;
import vr.balance.app.enums.RoleEnum;
import vr.balance.app.exceptions.EmailAlreadyInUseException;
import vr.balance.app.exceptions.NotFoundException;
import vr.balance.app.exceptions.UsernameAlreadyInUseException;
import vr.balance.app.models.User;
import vr.balance.app.models.exercise.CompletedExercise;
import vr.balance.app.repository.UserRepository;
import vr.balance.app.repository.exercise.CompletedBalanceTestExerciseRepository;
import vr.balance.app.repository.exercise.CompletedExerciseRepository;

import java.time.Instant;
import java.util.List;

@Service
public class UserService {

    private final UserRepository userRepository;
    private final CompletedExerciseRepository completedExerciseRepository;
    private final CompletedBalanceTestExerciseRepository completedBalanceTestExerciseRepository;
    private final ModelMapper modelMapper;
    private final CompletedExerciseService completedExerciseService;

    @Autowired
    public UserService(ModelMapper modelMapper, UserRepository userRepository, CompletedExerciseRepository completedExerciseRepository, CompletedBalanceTestExerciseRepository completedBalanceTestExerciseRepository, CompletedExerciseService completedExerciseService) {
        this.userRepository = userRepository;
        this.modelMapper = modelMapper;
        this.completedExerciseRepository = completedExerciseRepository;
        this.completedBalanceTestExerciseRepository = completedBalanceTestExerciseRepository;
        this.completedExerciseService = completedExerciseService;
    }

    public UserProfileResponse getUserProfileById(Long userId) {
        User user = userRepository.findById(userId)
                .orElseThrow(() -> new NotFoundException("User not found"));

        return modelMapper.map(user, UserProfileResponse.class);
    }

    public void editUserProfile(EditUserDTO request, Long userId) {
        User user = userRepository.findById(userId)
                .orElseThrow(() -> new UsernameNotFoundException("User not found."));

        if (!request.getEmail().equals(user.getEmail()) && userRepository.existsByEmail(request.getEmail())) {
            throw new EmailAlreadyInUseException("Email is already in use.");
        }

        if (!request.getUsername().equals(user.getUsername()) && userRepository.existsByUsername(request.getUsername())) {
            throw new UsernameAlreadyInUseException("Username is already in use.");
        }

        modelMapper.map(request, user);
        user.setUpdatedAt(Instant.now());

        userRepository.save(user);
    }

    public Page<UserProfileResponse> getAllPatients(int page, int size) {
        Pageable pageable = PageRequest.of(page, size);
        Page<User> users = userRepository.findAllByRole(RoleEnum.PATIENT, pageable);

        return users.map(user -> modelMapper.map(user, UserProfileResponse.class));
    }

    @Transactional
    public void deleteUserById(Long userId) {
        User user = userRepository.findById(userId)
                .orElseThrow(() -> new NotFoundException("User not found."));

        userRepository.delete(user);
    }

    /**
     * Fetches detailed information about a specific patient, including:
     * - Basic user profile information
     * - The 10 most recent completed exercises excluding balance tests
     * - The 5 most recent balance test sessions (only phase data + timestamp. This is for the chart in the website)
     * - The 10 most recent balance test sessions (Without phases,this is for the option to download a balance test result via the site)
     *
     * @param userId ID of the user (patient) to fetch
     * @return A response object containing all relevant patient details
     */
    public PatientDetailResponse fetchPatientDetail(Long userId) {
        // Find the user by ID or throw exception if not found
        User user = userRepository.findById(userId)
                .orElseThrow(() -> new NotFoundException("User not found."));

        // Convert user entity to DTO
        UserProfileResponse userDto = modelMapper.map(user, UserProfileResponse.class);

        // Fetch 10 most recent exercises (excluding balance tests) for this user
        List<CompletedExercise> recentExercises = completedExerciseRepository
                .findByUserIdAndExerciseNotOrderByCompletedAtDesc(
                        user.getId(),
                        ExerciseEnum.Balance,
                        PageRequest.of(0, 10)
                );

        // Fetch the latest 10 balance test sessions for this user (without phases)
        List<CompletedExercise> last10BalanceTests = completedExerciseRepository.
                findTop10ByUserIdAndExerciseOrderByCompletedAtDesc(user.getId(), ExerciseEnum.Balance);

        // Convert the exercises to DTOs
        List<CompletedExerciseResponse> exerciseDtos = recentExercises.stream()
                .map(ex -> modelMapper.map(ex, CompletedExerciseResponse.class))
                .toList();
        List<CompletedExerciseResponse> recentBalanceTests = last10BalanceTests.stream()
                .map(ex -> modelMapper.map(ex, CompletedExerciseResponse.class))
                .toList();

        // Fetch the latest 5 balance test sessions for this user (only completedAt and phase data, for the chart in the website)
        List<BalanceTestResponse> recentBalanceTestsResults = completedBalanceTestExerciseRepository
                .findTop5LatestByUser(user.getId(), PageRequest.of(0, 5));

        // Return a response object containing all the mapped data
        return new PatientDetailResponse(userDto, exerciseDtos, recentBalanceTests, recentBalanceTestsResults);
    }
}