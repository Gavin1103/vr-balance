using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using DTO.Request.Exercise.@base;

public class GenericExercise : Exercise {
    public string BackendEnum;

    public List<ExerciseMovement> Movements;
    public RepsAndSetsConfig RepsAndSetsConfig;
    public int AmountOfSets = 1;
    public float WaitTimeBetweenSets = 10f;
    public int AmountOfReps = 1;
    public float WaitTimeBetweenReps = 0.5f;

    // Action images
    private int currentSetIndex = 0;
    private int currentRepIndex = 0;
    private int currentMovementIndex = 0;
    private Image actionImageComponent;
    // Services
    private ExerciseService excerciseSerice = new ExerciseService();
    // Coroutines
    private Coroutine playSetsCoroutine;
    private Coroutine moveImageCoroutine;
    // Properties
    private ExerciseMovement currentMovement
    {
        get
        {
            return Movements[currentMovementIndex];
        }
    }
    protected GenericExerciseReferences refs
    {
        get
        {
            return GenericExerciseReferences.Instance;
        }
    }

    [HideInInspector] public GenericExerciseScoreCalculator ScoreCalculator = new GenericExerciseScoreCalculator();

    public GenericExercise(string backendEnum, string title, ExerciseCategory category, string description, List<string> requirements) : base(title, category, description, requirements) {
        BackendEnum = backendEnum;

        actionImageComponent = refs.MovementImageObject.GetComponent<Image>();
    }

    public override void StartExercise() {
        RepsAndSetsConfig.ApplyTo(this);
        refs.RepsAndSetsObject.SetActive(true);

        refs.SequenceUI.SetActive(true);
        currentMovementIndex = 0;
        currentRepIndex = 0;
        currentSetIndex = 0;

        base.StartExercise();
    }

    protected override void PlayExercise() {
        refs.MovementImageObject.transform.localPosition = new Vector3(300, 0, 0);
        refs.ActionImageLine.sizeDelta = new Vector2(0, refs.ActionImageLine.sizeDelta.y);

        playSetsCoroutine = ExerciseManager.Instance.StartCoroutine(PlaySets());
    }

    private IEnumerator PlaySets() {
        for (int i = 0; i < AmountOfSets; i++) {
            currentRepIndex = 0;
            for (int j = 0; j < AmountOfReps; j++) {
                currentMovementIndex = 0;

                foreach (var movement in Movements) {
                    movement.exercise = this;

                    // Update UI for current set/rep/movement
                    refs.RepsAndSetsText.text = $"Set {currentSetIndex + 1}/{AmountOfSets}\nRep {currentRepIndex + 1}/{AmountOfReps}";
                    actionImageComponent.sprite = currentMovement.InstructionImage;
                    yield return moveImageCoroutine = ExerciseManager.Instance.StartCoroutine(movement.Play());
                    movement.MovementEnded();
                    currentMovementIndex++;
                }

                // Wait between reps, except after the last rep
                if (currentRepIndex < AmountOfReps - 1 && WaitTimeBetweenReps > 0) {
                    yield return new WaitForSeconds(WaitTimeBetweenReps);
                }

                currentRepIndex++;
            }

            // Wait between sets, except after the last set
            if (currentSetIndex < AmountOfSets - 1 && WaitTimeBetweenSets > 0) {
                actionImageComponent.enabled = false;
                yield return ExerciseManager.Instance.StartCoroutine(ShowRestUI(WaitTimeBetweenSets));
                actionImageComponent.enabled = true;
            }

            currentSetIndex++;
        }
        // End of exercise
        SoundManager.soundInstance.PlaySFX("Exercise End");
        currentMovementIndex--;
        ExerciseManager.Instance.ExerciseEnded();
    }

    private IEnumerator ShowRestUI(float duration) {
        ExerciseManager.Instance.ExtraInfoObject.SetActive(true);

        float elapsed = 0f;
        while (elapsed < duration) {
            int secondsLeft = Mathf.CeilToInt(duration - elapsed);
            ExerciseManager.Instance.ExtraInfoText.text = $"Take a short break!\n{secondsLeft}s";
            elapsed += Time.deltaTime;
            yield return null;
        }

        ExerciseManager.Instance.ExtraInfoObject.SetActive(false);
    }

    public override void ExerciseEnded() {
        SaveExercise();

        if (playSetsCoroutine != null) {
            ExerciseManager.Instance.StopCoroutine(playSetsCoroutine);
            playSetsCoroutine = null;
        }
        if (moveImageCoroutine != null) {
            ExerciseManager.Instance.StopCoroutine(moveImageCoroutine);
            moveImageCoroutine = null;
        }

        currentMovement.MovementEnded();

        refs.SequenceUI.SetActive(false);
        ExerciseManager.Instance.ExtraInfoObject.SetActive(false);
    }

    protected virtual void SaveExercise() {
        CompletedExerciseDTO dto = new CompletedExerciseDTO {
            exercise = BackendEnum,
            earnedPoints = (int)ScoreManager.Instance.Score,
            difficulty = DifficultyManager.Instance.SelectedDifficulty,
            completedAt = System.DateTime.UtcNow
        };

       ExerciseManager.Instance.StartCoroutine(excerciseSerice.SaveExercise(
           dto,
           onSuccess: ApiResponse => {
               Debug.Log(ApiResponse.message);
           },
           onError: error => {
               Debug.Log(error.message);
           },
           "standard"
       ));
    }
}