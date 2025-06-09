using System.Collections;
using UnityEngine;

public abstract class AffordanceMovement : ExerciseMovement {
    public Vector3 LeftStickTarget { get; set; }
    public Vector3 RightStickTarget { get; set; }
    public Vector3 HeadTarget { get; set; }

    protected AffordanceMovement(Vector3 left, Vector3 right, Vector3 head, float duration, Sprite image, float score) :base(duration, image, score) {
        LeftStickTarget = left;
        RightStickTarget = right;
        HeadTarget = head;
    }

    public override IEnumerator Play() {
        // Move affordances to target positions
        NormalExerciseReferences.Instance.LeftStickAffordance.transform.localPosition = LeftStickTarget;
        NormalExerciseReferences.Instance.RightStickAffordance.transform.localPosition = RightStickTarget;
        NormalExerciseReferences.Instance.HeadsetAffordance.transform.localPosition = HeadTarget;
        // Move instruction image to the end
        float elapsed = 0f;
        while (elapsed < Duration) {
            NormalExerciseReferences.Instance.ActionImageLine.sizeDelta = new Vector2(Mathf.Lerp(0, 160, elapsed / Duration), NormalExerciseReferences.Instance.ActionImageLine.sizeDelta.y);
            NormalExerciseReferences.Instance.MovementImageObject.transform.localPosition = Vector3.Lerp(startPos, endPos, elapsed / Duration);
            exercise.ScoreCalculator.CalculateDistances(LeftStickTarget, RightStickTarget, HeadTarget);
            elapsed += Time.deltaTime;

            // for debugging
            if (Input.GetKeyDown(KeyCode.Space)) {
                elapsed = Duration;
                NormalExerciseReferences.Instance.ActionImageLine.sizeDelta = new Vector2(160, NormalExerciseReferences.Instance.ActionImageLine.sizeDelta.y);
                NormalExerciseReferences.Instance.MovementImageObject.transform.localPosition = endPos;
                break;
            }

            yield return null;
        }
        while (Input.GetKey(KeyCode.Space)) { // Need< Or my Scheiße breaks because the next action image will skip aswell
            yield return null;
        }
    }

}