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
import vr.balance.app.exceptions.NotFoundException;
import vr.balance.app.models.User;
import vr.balance.app.models.UserStats;
import vr.balance.app.models.exercise.CompletedExercise;
import vr.balance.app.repository.UserRepository;
import vr.balance.app.repository.UserStatsRepository;
import vr.balance.app.repository.exercise.CompletedExerciseRepository;

import java.time.Instant;
import java.time.LocalDate;
import java.time.ZoneId;
import java.time.temporal.ChronoUnit;
import java.util.List;
import java.util.Optional;

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
                .orElseGet(() -> new UserStreakDTO(user.getUsername(),0, 0));
    }

    public List<HighestStreakRankingDTO> getTop20HighestStreak() {
        return userStatsRepository.findTop20ByHighestStreak(PageRequest.of(0, 20));
    }

    public List<CurrentStreakRankingDTO> getTop20CurrentStreak() {
        return userStatsRepository.findTop20ByCurrentStreak(PageRequest.of(0, 20));
    }

    public <CE extends CompletedExercise> void updateUserStats(CE completedExercise) {
        UserStats existingStats = userStatsRepository.findByUser(completedExercise.getUser());

        CompletedExercise lastExercise = completedExerciseRepository.findFirstByUserOrderByCompletedAtDesc(completedExercise.getUser());

        if (existingStats != null) {
            updateExistingStats(existingStats, completedExercise, lastExercise);
        } else {
            createNewStats(completedExercise);
        }
    }

    private <CE extends CompletedExercise> void updateExistingStats(UserStats stats, CE completedExercise, CE lastExercise) {
        ZoneId zoneId = ZoneId.systemDefault();

        LocalDate lastExerciseDate = toLocalDate(lastExercise.getCompletedAt(), zoneId);
        LocalDate currentDate = toLocalDate(completedExercise.getCompletedAt(), zoneId);

        int newTotalPoints = stats.getTotalPoints() + completedExercise.getEarnedPoints();
        int existingExerciseCount = stats.getTotalExercises();
        int newStreak = calculateNewStreak(lastExerciseDate, currentDate, stats.getCurrentStreak());
        int newHighestStreak = Math.max(stats.getHighestStreak(), newStreak);

        stats.setTotalPoints(newTotalPoints);
        stats.setCurrentStreak(newStreak);
        stats.setHighestStreak(newHighestStreak);
        stats.setTotalExercises(existingExerciseCount + 1);

        userStatsRepository.save(stats);
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

    private LocalDate toLocalDate(Instant instant, ZoneId zoneId) {
        return instant.atZone(zoneId).toLocalDate();
    }

    public int calculateNewStreak(LocalDate lastDate, LocalDate currentDate, int currentStreak) {
        long daysBetween = ChronoUnit.DAYS.between(lastDate, currentDate);
        return switch ((int) daysBetween) {
            case 0 -> currentStreak;       // Same day
            case 1 -> currentStreak + 1;   // Next day = streak continues
            default -> 1;                  // Streak broken, reset to 1
        };
    }
}