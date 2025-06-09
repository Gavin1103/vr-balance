package vr.balance.app.models.exercise;

import jakarta.persistence.Entity;
import jakarta.persistence.Table;
import lombok.Getter;
import lombok.Setter;

@Entity
@Table(name = "completed_balance_test_exercise")
@Getter
@Setter
public class CompletedBalanceTestExercise extends CompletedExercise {
    //    Stand on 2 legs with eyes open
    String phase_1;
    //    Stand on 2 legs with eyes closed
    String phase_2;
    //    Stand on your left leg
    String phase_3;
    //    Stand on your right leg
    String phase_4;
}
