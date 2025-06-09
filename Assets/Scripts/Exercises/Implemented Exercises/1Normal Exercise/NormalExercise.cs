using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

public class NormalExercise : Exercise {
    public List<ExerciseMovement> Movements;

    private Coroutine playMovementCoroutine;

    // Action images
    private int currentMovementIndex = 0;
    private Image actionImageComponent;
    // Properties
    private ExerciseMovement currentMovement {
        get {
            return Movements[currentMovementIndex];
        }
    }
    private NormalExerciseReferences refs {
        get {
            return NormalExerciseReferences.Instance;
        }
    }

    [HideInInspector] public NormalExerciseScoreCalculator ScoreCalculator = new NormalExerciseScoreCalculator();

    public NormalExercise(string title, string description, List<string> requirements, List<ExerciseMovement> movements, bool positionNeeded, bool easyDifficulty, bool mediumDifficulty, bool hardDifficulty) : base(title, description, requirements, positionNeeded, easyDifficulty, mediumDifficulty, hardDifficulty) {
        Movements = movements;
        actionImageComponent = refs.MovementImageObject.GetComponent<Image>();

        PosNeeded = positionNeeded;
        Easy = easyDifficulty;
        Medium = hardDifficulty;
        Hard = hardDifficulty;
    }

    public override void StartExercise() {
        refs.LeftStickAffordance.SetActive(true);
        refs.RightStickAffordance.SetActive(true);
        refs.HeadsetAffordance.SetActive(true);
        refs.SequenceUI.SetActive(true);
        currentMovementIndex = 0;

        refs.NeedsPosition = PosNeeded;
        refs.EasyDifficulty = Easy;
        refs.MediumDifficulty = Medium;
        refs.HardDifficulty = Hard;
        base.StartExercise();
    }

    protected override void PlayExercise() {
        // Set movement image and start animation
        actionImageComponent.sprite = currentMovement.InstructionImage;
        refs.MovementImageObject.transform.localPosition = new Vector3(300, 0, 0);

        // Animate movement from right to left
        refs.ActionImageLine.sizeDelta = new Vector2(0, refs.ActionImageLine.sizeDelta.y);
        playMovementCoroutine = ExerciseManager.Instance.StartCoroutine(PlayMovement(currentMovement));
    }

    private IEnumerator PlayMovement(ExerciseMovement movement) {
        movement.exercise = this;
        yield return refs.MovementImageObject.GetComponent<MonoBehaviour>().StartCoroutine(movement.Play());
        movement.MovementEnded();
        currentMovementIndex++;

        // Keep playing or end the exercise if it's the last movement
        if (currentMovementIndex < Movements.Count) {
            PlayExercise();
        } else {
            SoundManager.soundInstance.PlaySFX("Exercise End");
            currentMovementIndex--;
            ExerciseManager.Instance.ExerciseEnded();
        }
    }

    public override void ExerciseEnded() {
        currentMovement.MovementEnded();
        refs.LeftStickAffordance.SetActive(false);
        refs.RightStickAffordance.SetActive(false);
        refs.HeadsetAffordance.SetActive(false);
        refs.SequenceUI.SetActive(false);
    }

    public void RestartCurrentMovement() {
        if (playMovementCoroutine != null) {
            ExerciseManager.Instance.StopCoroutine(playMovementCoroutine);
        }

        // Restart movement from scratch
        actionImageComponent.sprite = currentMovement.InstructionImage;
        refs.MovementImageObject.transform.localPosition = new Vector3(300, 0, 0);
        refs.ActionImageLine.sizeDelta = new Vector2(0, refs.ActionImageLine.sizeDelta.y);

        playMovementCoroutine = ExerciseManager.Instance.StartCoroutine(PlayMovement(currentMovement));
    }
}