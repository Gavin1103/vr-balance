
using System.Collections.Generic;

public class RepsAndSets {
    public List<ExerciseMovement> Movements;
    public float AmountOfSets = 1;
    public float AmountOfReps = 1;

    public RepsAndSets(List<ExerciseMovement> movements, float amountOfSets, float amountOfReps) {
        Movements = movements;
        AmountOfReps = amountOfReps;
        AmountOfSets = amountOfSets;
    }
}