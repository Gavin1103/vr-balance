package vr.balance.app.DTO.response;

import com.fasterxml.jackson.core.type.TypeReference;
import com.fasterxml.jackson.databind.ObjectMapper;
import lombok.Data;
import lombok.NoArgsConstructor;
import vr.balance.app.DTO.request.Vector3;

import java.time.Instant;
import java.util.List;


@Data
@NoArgsConstructor
public class BalanceTestResponse {
    private Instant completedAt;
    private List<Vector3> phase_1;
    private List<Vector3> phase_2;
    private List<Vector3> phase_3;
    private List<Vector3> phase_4;

    public BalanceTestResponse(
            Instant completedAt,
            String phase1Json,
            String phase2Json,
            String phase3Json,
            String phase4Json
    ) {
        this.completedAt = completedAt;
        this.phase_1 = parsePhase(phase1Json);
        this.phase_2 = parsePhase(phase2Json);
        this.phase_3 = parsePhase(phase3Json);
        this.phase_4 = parsePhase(phase4Json);
    }

    private List<Vector3> parsePhase(String json) {
        try {
            ObjectMapper objectMapper = new ObjectMapper();
            return objectMapper.readValue(json, new TypeReference<>() {});
        } catch (Exception e) {
            return List.of();
        }
    }
}