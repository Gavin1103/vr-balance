    package vr.balance.app.DTO.request;

    import lombok.*;
    import lombok.experimental.SuperBuilder;
    import vr.balance.app.DTO.request.base.CompletedExerciseDTO;


    @EqualsAndHashCode(callSuper = true)
    @Data
    @SuperBuilder
    @NoArgsConstructor
    @AllArgsConstructor
    public class CompletedFireflyExerciseDTO extends CompletedExerciseDTO {
        int caughtFirefliesCount;
        int caughtWrongFirefliesCount;
    }
