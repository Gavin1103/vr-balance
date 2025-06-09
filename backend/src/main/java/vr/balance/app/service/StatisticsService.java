package vr.balance.app.service;

import org.springframework.stereotype.Service;
import vr.balance.app.DTO.response.ExerciseStatsResponse;
import vr.balance.app.DTO.response.LeaderboardExerciseResponse;
import vr.balance.app.enums.ExerciseEnum;
import vr.balance.app.repository.exercise.CompletedExerciseRepository;

import java.time.Instant;
import java.time.temporal.ChronoUnit;
import java.util.List;

@Service
public class StatisticsService {

    private final CompletedExerciseRepository completedExerciseRepository;

    public StatisticsService(CompletedExerciseRepository completedExerciseRepository) {
        this.completedExerciseRepository = completedExerciseRepository;
    }

    public List<ExerciseStatsResponse> getAllTimeStats() {
        return completedExerciseRepository.findAllTimeStats();
    }

    public List<ExerciseStatsResponse> getStatsFromLast30Days() {
        Instant from = Instant.now().minus(30, ChronoUnit.DAYS);
        Instant to = Instant.now();
        return completedExerciseRepository.findStatsByDateRange(from, to);
    }

    public List<LeaderboardExerciseResponse> getLeaderboardForExercise(ExerciseEnum exercise) {
        List<Object[]> rows = completedExerciseRepository.findTop10HighestScoresPerUser(exercise.name());

        return rows.stream().map(row -> {
            LeaderboardExerciseResponse dto = new LeaderboardExerciseResponse();
            dto.setHighscore(((Number) row[1]).intValue());
            dto.setUsername((String) row[2]);
            return dto;
        }).toList();
    }


}
