using System.Collections;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class HoldMovement : AffordanceMovement {
    private float holdTime;
    public HoldMovement(Vector3 left, Vector3 right, Vector3 head, float duration, Sprite image, float score, float holdTime) : base(left, right, head, duration, image, score) { 
        this.holdTime = holdTime;
    }

    public override IEnumerator Play() {
        NormalExerciseReferences.Instance.HoldMovementText.transform.parent.gameObject.SetActive(true);
        NormalExerciseReferences.Instance.HoldMovementText.text = holdTime.ToString("0.0") + "s";

        yield return base.Play();

        // Hold at target & accumulate score
        float elapsedWhileHolding = 0f;
        while (elapsedWhileHolding < holdTime) {
            if (Input.GetKeyDown(KeyCode.Space)) {
                elapsedWhileHolding = holdTime;
                break;
            }
            NormalExerciseReferences.Instance.HoldImageLine.sizeDelta = new Vector2(Mathf.Lerp(0, 160, elapsedWhileHolding / holdTime), NormalExerciseReferences.Instance.HoldImageLine.sizeDelta.y);

            exercise.ScoreCalculator.CalculateDistances(LeftStickTarget, RightStickTarget, HeadTarget);
            currentScore += exercise.ScoreCalculator.CalculateDurationScore(TotalScore, holdTime);

            elapsedWhileHolding += Time.deltaTime;
            NormalExerciseReferences.Instance.HoldMovementText.text = (holdTime - elapsedWhileHolding).ToString("0.0") + "s";

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
    }
}