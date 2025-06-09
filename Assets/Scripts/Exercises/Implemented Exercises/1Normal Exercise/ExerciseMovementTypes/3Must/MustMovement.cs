using System.Collections;
using UnityEngine;

public class MustMovement : AffordanceMovement {
    public MustMovement(Vector3 left, Vector3 right, Vector3 head, float duration, Sprite image, float score) : base(left, right, head, duration, image, score) { }

    public override IEnumerator Play() {
        NormalExerciseReferences.Instance.InformationText.text = "Must Complete";
        NormalExerciseReferences.Instance.InformationObject.SetActive(true);
        yield return base.Play();

        while (true) {
            if (Input.GetKeyDown(KeyCode.Space)) {
                break;
            }

            exercise.ScoreCalculator.CalculateDistances(LeftStickTarget, RightStickTarget, HeadTarget);
            if (exercise.ScoreCalculator.AllTargetsHit()) {
                if (exercise.ScoreCalculator.IsAccurate()) {
                    break; // Continue only when target positions are accurately hit
                } else {
                    NormalExerciseReferences.Instance.InformationText.text = "Not Accurate Enough!";
                }
            } else {
                NormalExerciseReferences.Instance.InformationText.text = "Must Complete";
            }

            yield return null;
        }
        while (Input.GetKey(KeyCode.Space)) { // Need< Or my Scheiße breaks because the next action image will skip aswell
            yield return null;
        }

        currentScore = exercise.ScoreCalculator.CalculateInstanceScore(TotalScore);
        exercise.ScoreCalculator.DecideFeedback(currentScore, TotalScore);
    }

    public override void MovementEnded() {
        NormalExerciseReferences.Instance.InformationObject.SetActive(false);
    }
}