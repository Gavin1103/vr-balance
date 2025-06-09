using UnityEngine;
using System.Collections;

public class OneShotBehaviour : IMovementBehaviour {
    private float score;

    public OneShotBehaviour(float score) {
        this.score = score;
    }

    public override void OnMovementStart(ExerciseMovement movement) {
    }
    public override IEnumerator OnMovementUpdate(ExerciseMovement movement) {
        yield return null;
        // Decide score
        movement.currentScore = movement.exercise.ScoreCalculator.CalculateInstanceScore(score);
        // Show feedback
        movement.exercise.ScoreCalculator.DecideFeedback(movement.currentScore, movement.TotalScore);
    }
    public override void OnMovementEnd(ExerciseMovement movement) {
    }
}