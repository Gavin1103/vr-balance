using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RepsAndSets", menuName = "Exercises/Reps & Sets")]
public class RepsAndSetsSO : ScriptableObject {
    public List<ExerciseMovementSO> Movements;
    [Min(1)]
    public int AmountOfSets = 1;
    [Min(1)]
    public int AmountOfReps = 1;

    public RepsAndSets CreateRepsAndSets() {
        List<ExerciseMovement> movements = new List<ExerciseMovement>();
        foreach (var moveSO in Movements) {
            ExerciseMovement movement = moveSO.CreateMovement();
            movements.Add(movement);
        }
        return new RepsAndSets(movements, AmountOfSets, AmountOfReps);
    }
}