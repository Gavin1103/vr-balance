package vr.balance.app.service;

import org.modelmapper.ModelMapper;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.PageRequest;
import org.springframework.data.domain.Pageable;
import org.springframework.data.domain.Sort;
import org.springframework.stereotype.Service;
import vr.balance.app.DTO.response.user_stats.CurrentStreakRankingDTO;
import vr.balance.app.DTO.response.user_stats.HighestStreakRankingDTO;
import vr.balance.app.DTO.response.user_stats.UserStatsResponse;
import vr.balance.app.DTO.response.user_stats.UserStreakDTO;
import vr.balance.app.enums.ExerciseEnum;
import vr.balance.app.exceptions.NotFoundException;
import vr.balance.app.models.User;
import vr.balance.app.models.UserStats;
import vr.balance.app.models.exercise.CompletedExercise;
import vr.balance.app.repository.UserRepository;
import vr.balance.app.repository.UserStatsRepository;
import vr.balance.app.repository.exercise.CompletedExerciseRepository;

import java.time.Instant;
import java.time.LocalDate;
import java.time.ZoneOffset;
import java.time.temporal.ChronoUnit;
import java.util.List;

@Service
public class UserStatsService {

    private final UserStatsRepository userStatsRepository;
    private final CompletedExerciseRepository completedExerciseRepository;
    private final ModelMapper modelMapper;
    private final UserRepository userRepository;

    public UserStatsService(UserStatsRepository userStatsRepository, CompletedExerciseRepository completedExerciseRepository, ModelMapper modelMapper, UserRepository userRepository) {
        this.userStatsRepository = userStatsRepository;
        this.completedExerciseRepository = completedExerciseRepository;
        this.modelMapper = modelMapper;
        this.userRepository = userRepository;
    }

    public Page<UserStatsResponse> getAllUserStats(int page, int size, String sortBy, String direction) {
        Sort sort = Sort.by(Sort.Direction.fromString(direction), sortBy);
        Pageable pageable = PageRequest.of(page, size, sort);

        return userStatsRepository.findAll(pageable)
                .map(userStats -> {
                    UserStatsResponse response = modelMapper.map(userStats, UserStatsResponse.class);
                    response.setUsername(userStats.getUser().getUsername());
                    return response;
                });
    }

    public UserStreakDTO getUserStreak(Long userId) {
        User user = userRepository.findById(userId)
                .orElseThrow(() -> new NotFoundException("User not found."));

        return userStatsRepository.findStreakStatsByUserId(user.getId())
                .orElseGet(() -> new UserStreakDTO(user.getUsername(), 0, 0));
    }

    public List<HighestStreakRankingDTO> getTop20HighestStreak() {
        return userStatsRepository.findTop20ByHighestStreak(PageRequest.of(0, 20));
    }

    public List<CurrentStreakRankingDTO> getTop20CurrentStreak() {
        return userStatsRepository.findTop20ByCurrentStreak(PageRequest.of(0, 20));
    }

    /**
     * Updates the {@link UserStats} for the given completed exercise.
     *
     * <p>Before updating any statistics, this method verifies that the new exercise's timestamp
     * is not earlier than the user's most recent exercise. If it is, an exception is thrown to
     * prevent inconsistent or retroactive streak updates.</p>
     *
     * <p>If the exercise is not a {@code BalanceTest}, this method will:
     * <ul>
     *   <li>Retrieve the most recent non-balance exercise before the current one</li>
     *   <li>Update the user's streak, total points, and exercise count if stats exist</li>
     *   <li>Create new stats if none exist yet</li>
     * </ul>
     *
     * @param completedExercise the completed exercise instance (excluding BalanceTest)
     * @param <CE>              a subclass of {@link CompletedExercise}
     */
    public <CE extends CompletedExercise> void updateUserStats(CE completedExercise) {
        UserStats existingStats = userStatsRepository.findByUser(completedExercise.getUser());

        if (existingStats != null) {
            // Retrieve the most recent completed exercise regardless of type
            CompletedExercise latestExercise = completedExerciseRepository.findFirstByUserOrderByCompletedAtDesc(completedExercise.getUser());

            if (latestExercise != null && completedExercise.getCompletedAt().isBefore(latestExercise.getCompletedAt())) {
                throw new IllegalArgumentException("Cannot submit an exercise with a date earlier than your most recent completed exercise.");
            }

            CompletedExercise lastNonBalanceExercise = completedExerciseRepository
                    .findFirstByUserAndExerciseNotAndCompletedAtBeforeOrderByCompletedAtDesc(
                            completedExercise.getUser(),
                            ExerciseEnum.Balance,
                            completedExercise.getCompletedAt()
                    );

            updateExistingStats(existingStats, completedExercise, lastNonBalanceExercise);
        } else {
            createNewStats(completedExercise);
        }
    }

    /**
     * Updates the existing {@link UserStats} of a user based on a newly completed exercise.
     *
     * <p>This method calculates and updates:
     * <ul>
     *     <li>Total points earned</li>
     *     <li>Total number of completed exercises</li>
     *     <li>Current streak based on the last non-balance exercise date</li>
     *     <li>Highest streak achieved so far</li>
     * </ul>
     *
     * <p>If there is no previous non-balance exercise ({@code lastExercise} is {@code null}),
     * the streak will be initialized to 1. This is fallback behavior and assumes the user's
     * history is either empty or contains only balance tests.</p>
     *
     * @param stats             The current {@link UserStats} of the user
     * @param completedExercise The newly completed exercise
     * @param lastExercise      The most recent non-balance exercise before the current one, or {@code null} if none exist
     * @param <CE>              A subclass of {@link CompletedExercise}
     */
    private <CE extends CompletedExercise> void updateExistingStats(UserStats stats, CE completedExercise, CE lastExercise) {
        LocalDate currentDate = toLocalDate(completedExercise.getCompletedAt());

        int newStreak;

        // Defensive null-check in case future changes skip validation
        if (lastExercise != null) {
            LocalDate lastExerciseDate = toLocalDate(lastExercise.getCompletedAt());
            newStreak = calculateNewStreak(lastExerciseDate, currentDate, stats.getCurrentStreak());
        } else {
            newStreak = 1;
        }

        int newTotalPoints = stats.getTotalPoints() + completedExercise.getEarnedPoints();
        int newHighestStreak = Math.max(stats.getHighestStreak(), newStreak);
        int newExerciseCount = stats.getTotalExercises() + 1;

        stats.setTotalPoints(newTotalPoints);
        stats.setCurrentStreak(newStreak);
        stats.setHighestStreak(newHighestStreak);
        stats.setTotalExercises(newExerciseCount);

        userStatsRepository.save(stats);
    }

    private LocalDate toLocalDate(Instant instant) {
        return instant.atZone(ZoneOffset.UTC).toLocalDate();
    }

    public int calculateNewStreak(LocalDate lastDate, LocalDate currentDate, int currentStreak) {
        long daysBetween = ChronoUnit.DAYS.between(lastDate, currentDate);
        return switch ((int) daysBetween) {
            case 0 -> currentStreak;       // Same day
            case 1 -> currentStreak + 1;   // Next day = streak continues
            default -> 1;                  // Streak broken, reset to 1
        };
    }

    private <CE extends CompletedExercise> void createNewStats(CE completedExercise) {
        UserStats newStats = UserStats.builder()
                .user(completedExercise.getUser())
                .totalPoints(completedExercise.getEarnedPoints())
                .currentStreak(1)
                .highestStreak(1)
                .totalExercises(1)
                .build();

        userStatsRepository.save(newStats);
    }
}