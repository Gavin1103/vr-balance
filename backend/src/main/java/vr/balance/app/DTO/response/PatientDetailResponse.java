package vr.balance.app.DTO.response;

import lombok.AllArgsConstructor;
import lombok.Data;
import lombok.NoArgsConstructor;

import java.util.List;

@AllArgsConstructor
@NoArgsConstructor
@Data
public class PatientDetailResponse {
    UserProfileResponse user;
    List<CompletedExerciseResponse> recentExercises;
    List<CompletedExerciseResponse> recentBalanceTests;
    List<BalanceTestResponse> recentBalanceTestResults; // For the chart in the website
}
