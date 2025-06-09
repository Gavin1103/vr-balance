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
    List<BalanceTestResponse> recentBalanceTests;
}
