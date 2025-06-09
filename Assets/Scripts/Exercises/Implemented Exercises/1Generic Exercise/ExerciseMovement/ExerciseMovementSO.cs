using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewExerciseMovement", menuName = "Exercise/Exercise Movement")]
public class ExerciseMovementSO : ScriptableObject
{
    public Vector3 LeftStickTarget;
    public Vector3 RightStickTarget;
    public Vector3 HeadTarget;

    public List<BehaviourSO> Behaviours = new List<BehaviourSO>();

    public float Duration;
    public Sprite Image;
    public float Score;

    public ExerciseMovement CreateMovement()
    {
        List<IMovementBehaviour> behaviours = new List<IMovementBehaviour>();
        foreach (var behaviourSO in Behaviours)
        {
            IMovementBehaviour behaviour = behaviourSO.CreateBehaviour();
            behaviours.Add(behaviour);
        }

        return new ExerciseMovement(LeftStickTarget, RightStickTarget, HeadTarget, Duration, Image, Score, behaviours);
    }
}