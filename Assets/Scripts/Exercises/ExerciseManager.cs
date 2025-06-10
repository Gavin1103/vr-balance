using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class ExerciseManager : MonoBehaviour {
    public static ExerciseManager Instance { get; private set; }

    [Header("Exercises")]
    public List<Exercise> Exercises = new List<Exercise>();
    private Exercise currentExercise;
    [Header("General UI References")]
    public GameObject MainUI;
    public GameObject ExerciseUI;
    public GameObject EndUI;
    public GameObject ExerciseTimer;
    [Header("Spawn Exercise Buttons")]
    public GameObject ButtonPrefab;
    public Transform ButtonContainer;
    [Header("Exercise Info UI")]
    public GameObject SelectedExerciseInfo;
    public GameObject ExercisesMenu;
    public GameObject Leaderboard;
    public TMP_Text SelectedExerciseTitle;
    public TMP_Text SelectedExerciseDescription;
    public TMP_Text SelectedExerciseRequirements;
    public TMP_Text TimeLeft;
    private Button currentSelectedExerciseButton;
    private TMP_Text currentSelectedExerciseText;
    public GameObject VideoPlayer;
    public Button StartExerciseButton;
    [Header("Tracking Targets")]
    public Transform LeftStick;
    public Transform RightStick;
    public Transform Headset;
    [Header("Balance Test")]
    public GameObject BalanceTestWarning;
    public BalanceTestExercise BalanceTestExercise;
    public SaveHeadPositionData saveHeadPositionData;

    //Balance Test Runner
    public BalanceTestRunner balanceTestRunner; 


    void Awake() {
        Instance = this;
    }

    void Start()
    {
        Exercises = ExerciseFactory.CreateAllExercises();
        BalanceTestExercise = Exercises.Find(e => e.Title.ToLower().Contains("balance")) as BalanceTestExercise;
        GenerateExerciseButtons();
    }

    private void GenerateExerciseButtons() {
        foreach (Exercise exercise in Exercises) {
            GameObject buttonObj = Instantiate(ButtonPrefab, ButtonContainer);
            Button button = buttonObj.GetComponent<Button>();
            TMP_Text buttonText = buttonObj.GetComponentInChildren<TMP_Text>();

            buttonText.text = exercise.Title;
            button.onClick.AddListener(() => SelectExercise(exercise));
        }
    }

    private void SelectExercise(Exercise exercise) {
        SelectedExerciseInfo.SetActive(true);
        ExercisesMenu.SetActive(false);
        // VideoPlayer.SetActive(true);

   

        // Find new button and text
        foreach (Transform child in ButtonContainer) {
            TMP_Text text = child.GetComponentInChildren<TMP_Text>();
            if (text.text == exercise.Title) {
                currentSelectedExerciseButton = child.GetComponent<Button>();
                currentSelectedExerciseText = text;
                break;
            }
        }



        // Update title and description
        SelectedExerciseTitle.text = exercise.Title;
        SelectedExerciseDescription.text = exercise.Description;
        string requirementText = "";
        foreach (string req in exercise.Requirements) {
            requirementText += $"• {req}\n";
        }
        SelectedExerciseRequirements.text = requirementText;

        // Set video explanation

        // Check balance test
        bool isBalanceTest = exercise.Title.ToLower().Contains("balance");
        if (DifficultyManager.Instance.AdvisedDifficulty == Difficulty.None && !isBalanceTest) {
            StartExerciseButton.interactable = false;
            BalanceTestWarning.SetActive(true);
        } else {
            StartExerciseButton.interactable = true;
            BalanceTestWarning.SetActive(false);
        }

        if (isBalanceTest) {
            DifficultyManager.Instance.ShowDropdown(false);
            Leaderboard.SetActive(false);
        } else {
            DifficultyManager.Instance.ShowDropdown(true);
            Leaderboard.SetActive(true);
        }

        // Onclick
        StartExerciseButton.onClick.RemoveAllListeners();
        StartExerciseButton.onClick.AddListener(() => StartExercise(exercise));
    }

    public void SelectBalanceTestExercise() {
        if (BalanceTestExercise != null) {
            SelectExercise(BalanceTestExercise);
        }
    }

    private void StartExercise(Exercise exercise) {
        SoundManager.soundInstance.PlaySFX("Exercise Start");
        ScoreManager.Instance.ResetScore();
        MainUI.SetActive(false);
        ExerciseUI.SetActive(true);

        // Start the 5 second delay coroutine
        StartCoroutine(WaitBeforeStarting(exercise));

        // Temporary location, Give color to the cube
        //NormalExerciseReferences.Instance.FeedbackLine.GetComponent<Renderer>().material.color = Color.red;
    }
    private IEnumerator WaitBeforeStarting(Exercise exercise) {
        // Countdown from 5 seconds to 0
        float countdownTime = 5f;
        ExerciseTimer.SetActive(true);

        while (countdownTime > 0) {
            // Update the TimeLeft UI with the remaining time (rounded to an integer)
            TimeLeft.text = Mathf.Ceil(countdownTime).ToString();

            // Wait for 1 second before updating again
            yield return new WaitForSeconds(1f);

            // Decrease the countdown
            countdownTime--;
        }

        // When the countdown is done, clear the text
        TimeLeft.text = "Go!";
        ExerciseTimer.SetActive(false);
        // After countdown ends, execute the following
        currentExercise = exercise;
        currentExercise.StartExercise();

        if (exercise.Title.ToLower().Contains("balance")) {
            if (balanceTestRunner != null) {
                balanceTestRunner.gameObject.SetActive(true);
                balanceTestRunner.StartBalanceTestSequence();
            }
        }

        // Temporary location, give color to the cube
        // NormalExerciseReferences.Instance.FeedbackLine.GetComponent<Renderer>().material.color = Color.red;
    }

    public void ExerciseEnded() {
        ExerciseUI.SetActive(false);
        EndUI.SetActive(true);

        // Temporary location, Set FeedbackCube off after exercise ended
        //GenericExerciseReferences.Instance.FeedbackLine.SetActive(false);
        //GenericExerciseReferences.Instance.RenderLineMinimal.SetActive(false);
        //GenericExerciseReferences.Instance.RenderLineMaximal.SetActive(false);

        EndScreenUI.Instance.UpdateEndUI(currentExercise.Title, ScoreManager.Instance.Score);

        currentExercise.ExerciseEnded();
        ScoreManager.Instance.ResetScore();
        currentExercise = null;
    }
}