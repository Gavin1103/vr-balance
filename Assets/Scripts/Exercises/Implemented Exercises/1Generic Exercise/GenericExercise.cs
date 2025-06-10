using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using DTO.Request.Exercise.@base;

public class GenericExercise : Exercise
{
    public List<ExerciseMovement> Movements;
    public int AmountOfSets = 1;
    public float WaitTimeBetweenSets = 10f;
    public int AmountOfReps = 1;
    public float WaitTimeBetweenReps = 0.5f;

    public List<PositionChecker> Checkers;

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

    public GenericExercise(
            string title, string description, List<string> requirements,
            List<ExerciseMovement> movements, int amountOfSets, float waitTimeBetweenSets, int amountOfReps, float waitTimeBetweenReps,
            bool positionNeeded, bool easyDifficulty, bool mediumDifficulty, bool hardDifficulty, List<PositionChecker> positionCheckers)
            : base(title, description, requirements, positionNeeded, easyDifficulty, mediumDifficulty, hardDifficulty, positionCheckers)
    {
        Movements = movements;
        AmountOfSets = amountOfSets;
        WaitTimeBetweenSets = waitTimeBetweenSets;
        AmountOfReps = amountOfReps;
        WaitTimeBetweenReps = waitTimeBetweenReps;

        PosNeeded = positionNeeded;
        Easy = easyDifficulty;
        Medium = hardDifficulty;
        Hard = hardDifficulty;
        Checkers = positionCheckers;

        actionImageComponent = refs.MovementImageObject.GetComponent<Image>();
    }

    public override void StartExercise()
    {
        refs.RepsAndSetsObject.SetActive(true);

        refs.LeftStickAffordance.SetActive(true);
        refs.RightStickAffordance.SetActive(true);
        refs.HeadsetAffordance.SetActive(true);
        refs.SequenceUI.SetActive(true);
        currentMovementIndex = 0;
        currentRepIndex = 0;
        currentSetIndex = 0;

        refs.NeedsPosition = PosNeeded;
        refs.EasyDifficulty = Easy;
        refs.MediumDifficulty = Medium;
        refs.HardDifficulty = Hard;

        // Adjust reps based on difficulty
        string chosenDifficulty = DifficultyManager.Instance.SelectedDifficulty.ToString();
        // if (chosenDifficulty == "Medium") {
        //     AmountOfReps = Mathf.CeilToInt(AmountOfReps * 1.5f); // 1.5x as hard when medium
        // } else if (chosenDifficulty == "Hard") {
        //     AmountOfReps = AmountOfReps * 2; // Twice as hard when hard
        // }

        if (PosNeeded == true)
        {
            int Count = chosenDifficulty == "Easy" ? 0 : chosenDifficulty == "Medium" ? 1 : chosenDifficulty == "Hard" ? 2 : 0;
            if (Count >= 0 && Count < Checkers.Count)
            {
                refs.currentPosSO = Checkers[Count];
            }
        }

        base.StartExercise();
    }

    protected override void PlayExercise()
    {

        refs.MovementImageObject.transform.localPosition = new Vector3(300, 0, 0);
        refs.ActionImageLine.sizeDelta = new Vector2(0, refs.ActionImageLine.sizeDelta.y);

        playSetsCoroutine = ExerciseManager.Instance.StartCoroutine(PlaySets());
    }

    private IEnumerator PlaySets()
    {
        for (int i = 0; i < AmountOfSets; i++)
        {
            currentRepIndex = 0;
            for (int j = 0; j < AmountOfReps; j++)
            {
                currentMovementIndex = 0;

                foreach (var movement in Movements)
                {
                    movement.exercise = this;

                    // Update UI for current set/rep/movement
                    refs.RepsAndSetsText.text = $"Set {currentSetIndex + 1}/{AmountOfSets}\nRep {currentRepIndex + 1}/{AmountOfReps}";

                    actionImageComponent.sprite = currentMovement.InstructionImage;
                    yield return moveImageCoroutine = ExerciseManager.Instance.StartCoroutine(movement.Play());
                    movement.MovementEnded();
                    currentMovementIndex++;
                }

                // Wait between reps, except after the last rep
                if (currentRepIndex < AmountOfReps - 1 && WaitTimeBetweenReps > 0)
                {
                    yield return new WaitForSeconds(WaitTimeBetweenReps);
                }

                currentRepIndex++;
            }

            // Wait between sets, except after the last set
            if (currentSetIndex < AmountOfSets - 1 && WaitTimeBetweenSets > 0)
            {
                actionImageComponent.enabled = false;
                yield return ExerciseManager.Instance.StartCoroutine(ShowRestUI(WaitTimeBetweenSets));
                actionImageComponent.enabled = true;
            }

            currentSetIndex++;
        }
        // End of exercise
        ExerciseEnded();
        SoundManager.soundInstance.PlaySFX("Exercise End");
        ExerciseManager.Instance.ExerciseEnded();
    }

    private IEnumerator ShowRestUI(float duration)
    {
        refs.RestUI.SetActive(true);

        float elapsed = 0f;
        while (elapsed < duration)
        {
            int secondsLeft = Mathf.CeilToInt(duration - elapsed);
            refs.TakeABreakText.text = $"Take a short break!\n{secondsLeft}s";
            elapsed += Time.deltaTime;
            yield return null;
        }

        refs.RestUI.SetActive(false);
    }

    public override void ExerciseEnded()
    {
        SaveExercise();

        if (playSetsCoroutine != null)
        {
            ExerciseManager.Instance.StopCoroutine(playSetsCoroutine);
            playSetsCoroutine = null;
        }
        if (moveImageCoroutine != null)
        {
            ExerciseManager.Instance.StopCoroutine(moveImageCoroutine);
            moveImageCoroutine = null;
        }

        currentMovementIndex--;
        currentMovement.MovementEnded();

        refs.SequenceUI.SetActive(false);
        refs.RestUI.SetActive(false);
    }

    private void SaveExercise() {
        CompletedExerciseDTO dto = new CompletedExerciseDTO {
            exercise = this.Title,
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
           this.Title.ToLower()
       ));
    }
}