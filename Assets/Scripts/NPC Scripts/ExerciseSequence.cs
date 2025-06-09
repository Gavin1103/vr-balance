using UnityEngine;

[CreateAssetMenu(fileName = "NewExerciseSequence", menuName = "Physio/Exercise Sequence")]
public class ExerciseSequence : ScriptableObject
{
    public ExerciseModule[] exercises;
}

