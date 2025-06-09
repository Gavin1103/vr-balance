using System.Collections;
using UnityEngine;

public class CloseEyesMovement : AffordanceMovement {
    private float closeEyesTime;

    public CloseEyesMovement(Vector3 left, Vector3 right, Vector3 head, float duration, Sprite image, float score, float closeEyesTime) : base(left, right, head, duration, image, score) { 
        this.closeEyesTime = closeEyesTime;
    }

    public override IEnumerator Play() {
        NormalExerciseReferences.Instance.HoldMovementText.transform.parent.gameObject.SetActive(true);
        NormalExerciseReferences.Instance.HoldMovementText.text = closeEyesTime.ToString("0.0") + "s";
        NormalExerciseReferences.Instance.EyesClosedSphere.SetActive(true);
        MapManager.Instance.CurrentActiveMap.SetActive(false);

        yield return base.Play();

        // Hold at target & accumulate score
        float elapsedWhileHolding = 0f;
        while (elapsedWhileHolding < closeEyesTime) {
            if (Input.GetKeyDown(KeyCode.Space)) {
                elapsedWhileHolding = closeEyesTime;
                break;
            }

            NormalExerciseReferences.Instance.HoldImageLine.sizeDelta = new Vector2(Mathf.Lerp(0, 160, elapsedWhileHolding / closeEyesTime), NormalExerciseReferences.Instance.HoldImageLine.sizeDelta.y);

            exercise.ScoreCalculator.CalculateDistances(LeftStickTarget, RightStickTarget, HeadTarget);
            currentScore += exercise.ScoreCalculator.CalculateDurationScore(TotalScore, closeEyesTime);

            elapsedWhileHolding += Time.deltaTime;
            NormalExerciseReferences.Instance.HoldMovementText.text = (closeEyesTime - elapsedWhileHolding).ToString("0.0") + "s";

            yield return null;            
        }
        while (Input.GetKey(KeyCode.Space)) { // Need< Or my Scheiße breaks because the next action image will skip aswell
            yield return null;
        }

        exercise.ScoreCalculator.DecideFeedback(currentScore, TotalScore);
    }

    public override void MovementEnded() {
        NormalExerciseReferences.Instance.HoldMovementText.transform.parent.gameObject.SetActive(false);
        NormalExerciseReferences.Instance.HoldImageLine.sizeDelta = new Vector2(0, NormalExerciseReferences.Instance.HoldImageLine.sizeDelta.y);
        NormalExerciseReferences.Instance.EyesClosedSphere.SetActive(false);
        MapManager.Instance.CurrentActiveMap.SetActive(true);
    }
}