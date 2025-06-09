using System.Collections;
using UnityEngine;

public class NormalMovement : AffordanceMovement {
    public NormalMovement(Vector3 left, Vector3 right, Vector3 head, float duration, Sprite image, float score) : base(left, right, head, duration, image, score) {

    }

    public override IEnumerator Play() {
        yield return base.Play();
        currentScore = exercise.ScoreCalculator.CalculateInstanceScore(TotalScore);
        
        exercise.ScoreCalculator.DecideFeedback(currentScore, TotalScore);
    }

    public override void MovementEnded() {

    }
}