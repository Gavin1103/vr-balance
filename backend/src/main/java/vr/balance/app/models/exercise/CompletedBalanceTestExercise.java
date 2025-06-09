package vr.balance.app.models.exercise;

import jakarta.persistence.Column;
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
    @Column(columnDefinition = "TEXT")
    String phase_1;

    //    Stand on 2 legs with eyes closed
    @Column(columnDefinition = "TEXT")
    String phase_2;

    //    Stand on your left leg
    @Column(columnDefinition = "TEXT")
    String phase_3;

    //    Stand on your right leg
    @Column(columnDefinition = "TEXT")
    String phase_4;
}
