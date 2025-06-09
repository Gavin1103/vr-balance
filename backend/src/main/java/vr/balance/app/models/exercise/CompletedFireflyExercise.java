package vr.balance.app.models.exercise;

import jakarta.persistence.Entity;
import jakarta.persistence.Table;
import lombok.*;
import lombok.experimental.SuperBuilder;

@Entity
@Table(name = "completed_firefly_exercise")
@Getter
@Setter
@SuperBuilder
@NoArgsConstructor
@AllArgsConstructor
public class CompletedFireflyExercise extends CompletedExercise {
    //    TODO: Add more data to store
    int caughtFirefliesCount;
    int caughtWrongFirefliesCount;
}
