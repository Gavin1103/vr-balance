# 1 | Exercise System

This system manages the lifecycle of exercises: selection, display, execution, and completion. It is modular, scalable, and built around an abstract `Exercise` class which is extended by different types of exercises. The user selects an exercise, reviews its details, and starts it. Once finished, the end screen displays a result message.

---

## 1.1 | Base Class: `Exercise`

All exercises inherit from this abstract class. It contains shared properties and methods for setup, execution, and finishing.

```csharp
public abstract class Exercise {
    public string Title;
    public ExerciseCategory Category;
    public string Description;
    public List<string> Requirements;

    protected Difficulty dif => DifficultyManager.Instance.SelectedDifficulty;

    protected Exercise(string title, ExerciseCategory category, string description, List<string> requirements) {
        Title = title;
        Category = category;
        Description = description;
        Requirements = requirements;
    }

    public virtual void StartExercise() {
        PlayExercise();
    }

    protected virtual void PlayExercise() {
        // To be implemented in child class
    }

    public virtual void ExerciseEnded() {}

    public virtual void DisplayEndScreen() {
        string[] messages = {
            $"Great job completing the {Title}!",
            $"You’ve finished the {Title} – well done!",
            $"That’s it for the {Title}. Nice work!",
            $"The {Title} is complete – keep it up!",
            $"Well done! {Title} finished successfully.",
            $"You just wrapped up the {Title} – awesome!",
            $"That was the {Title}. Great effort!",
            $"Nice! {Title} completed and logged."
        };
        string selectedMessage = messages[Random.Range(0, messages.Length)];
        EndScreenUI.Instance.UpdateEndUI(selectedMessage, "Score:", Mathf.RoundToInt(ScoreManager.Instance.Score).ToString());
    }
}
```

---

## 1.2 | `ExerciseManager`

This class handles the flow between different exercise stages and manages UI transitions, exercise selection, and execution.

```csharp
public class ExerciseManager : MonoBehaviour {
    public static ExerciseManager Instance { get; private set; }

    public List<Exercise> Exercises = new();
    private Exercise currentExercise;

    [Header("General UI")]
    public GameObject MainUI, ExerciseUI, ExitUI, EndUI;

    [Header("Buttons & Categories")]
    public GameObject ButtonPrefab;
    public Transform AllRowsContainer;
    public List<CategoryRow> CategoryRows;

    [Header("Exercise Info UI")]
    public GameObject SelectedExerciseInfo, ExercisesMenu, Leaderboard;
    public TMP_Text SelectedExerciseTitle, SelectedExerciseDescription, SelectedExerciseRequirements;
    public GameObject VideoPlayer;
    public Button StartExerciseButton;

    [Header("Extra Info")]
    public GameObject ExtraInfoObject;
    public TextMeshProUGUI ExtraInfoText;

    [Header("Tracking")]
    public Transform LeftStick, RightStick, Headset;

    [Header("Balance Test")]
    public GameObject BalanceTestWarning;
    public BalanceTestExercise BalanceTestExercise;

    void Awake() {
        Instance = this;
    }

    void Start() {
        Exercises = ExerciseFactory.CreateAllExercises();
        BalanceTestExercise = Exercises.Find(e => e.Title.ToLower().Contains("balance")) as BalanceTestExercise;
        GenerateExerciseButtons();
    }

    void GenerateExerciseButtons() {
        foreach (Exercise exercise in Exercises) {
            var row = CategoryRows.Find(r => r.CategoryName.ToLower() == exercise.Category.ToString().ToLower());
            if (row == null) continue;

            GameObject buttonObj = Instantiate(ButtonPrefab, row.ButtonContainer);
            Button button = buttonObj.GetComponent<Button>();
            TMP_Text text = buttonObj.GetComponentInChildren<TMP_Text>();

            text.text = exercise.Title;
            button.onClick.AddListener(() => SelectExercise(exercise));
        }
    }

    void SelectExercise(Exercise exercise) {
        SelectedExerciseInfo.SetActive(true);
        ExercisesMenu.SetActive(false);

        foreach (Transform child in AllRowsContainer) {
            TMP_Text text = child.GetComponentInChildren<TMP_Text>();
            if (text.text == exercise.Title) {
                break;
            }
        }

        SelectedExerciseTitle.text = exercise.Title;
        SelectedExerciseDescription.text = exercise.Description;

        SelectedExerciseRequirements.text = string.Join("\n", exercise.Requirements.ConvertAll(r => $"• {r}"));

        bool isBalanceTest = exercise.Title.ToLower().Contains("balance");

        if (DifficultyManager.Instance.AdvisedDifficulty == Difficulty.None && !isBalanceTest) {
            StartExerciseButton.interactable = false;
            BalanceTestWarning.SetActive(true);
        } else {
            StartExerciseButton.interactable = true;
            BalanceTestWarning.SetActive(false);
        }

        DifficultyManager.Instance.ShowDropdown(!isBalanceTest);
        Leaderboard.SetActive(!isBalanceTest);

        StartExerciseButton.onClick.RemoveAllListeners();
        StartExerciseButton.onClick.AddListener(() => StartExercise(exercise));
    }

    void StartExercise(Exercise exercise) {
        SoundManager.soundInstance.PlaySFX("Exercise Start");
        ScoreManager.Instance.ResetScore();
        MainUI.SetActive(false);
        ExerciseUI.SetActive(true);
        ExitUI.SetActive(false);
        StartCoroutine(WaitBeforeStarting(exercise));
    }

    IEnumerator WaitBeforeStarting(Exercise exercise) {
        float countdownTime = 4f;
        ExtraInfoObject.SetActive(true);
        SoundManager.soundInstance.PlaySFX("SFX-Countdown_1");

        while (countdownTime > 0f) {
            ExtraInfoText.text = Mathf.Ceil(countdownTime).ToString();
            if (Input.GetKeyDown(KeyCode.Space)) break;
            countdownTime -= Time.deltaTime;
            yield return null;
        }

        ExtraInfoText.text = "Go!";
        ExerciseUI.SetActive(true);
        ExitUI.SetActive(true);
        ExtraInfoObject.SetActive(false);

        currentExercise = exercise;
        currentExercise.StartExercise();
    }

    public void ExerciseEnded() {
        ExerciseUI.SetActive(false);
        EndUI.SetActive(true);
        currentExercise.ExerciseEnded();
        currentExercise.DisplayEndScreen();
        ScoreManager.Instance.ResetScore();
        currentExercise = null;
    }

    public void SelectBalanceTestExercise() {
        if (BalanceTestExercise != null) {
            SelectExercise(BalanceTestExercise);
        }
    }
}
```

---

## 1.3 | `ExerciseFactory`

Uses the factory pattern to load all exercise assets from the `Resources/Exercises` folder and create runtime instances of them.

```csharp
public class ExerciseFactory : MonoBehaviour {
    public static ExerciseFactory Instance { get; private set; }

    void Awake() {
        Instance = this;
    }

    public static List<Exercise> CreateAllExercises() {
        List<Exercise> exercises = new();
        ExerciseSO[] assets = Resources.LoadAll<ExerciseSO>("Exercises");

        foreach (var asset in assets) {
            exercises.Add(asset.CreateExercise());
        }

        return exercises;
    }
}
```

---

## 1.4 | `ExerciseSO`

Each exercise is stored as a ScriptableObject. This makes creating many unique exercises easy without duplicating logic.

```csharp
public abstract class ExerciseSO : ScriptableObject {
    public string Title;
    public ExerciseCategory Category;
    public string Description;
    public List<string> Requirements;

    public abstract Exercise CreateExercise();
}
```

---

## 1.5 | UI Flow Overview

- `MainUI`: Entry menu
- `ExerciseUI`: Active during exercise
- `EndUI`: Shows score and message
- `SelectedExerciseInfo`: Shows details about the chosen exercise
- `ExercisesMenu`: Category selection
- `Leaderboard`: Performance scores
- `ExtraInfoText`: Countdown timer
- `BalanceTestWarning`: Warning if the balance test was skipped

---

## 1.6 | Additional: `BehaviourSO`

Used to define reusable logic blocks for exercise movement or condition checks.

```csharp
public abstract class BehaviourSO : ScriptableObject {
    public abstract IMovementBehaviour CreateBehaviour();
}
```

---

## 1.6 | Default Exercise Implementation

Exercises can be implemented in different ways. The most common and standardized one is the `GenericExercise`, which represents a sequence of timed, guided movements. This structure allows data-driven creation, real-time feedback, and flexible difficulty handling.

---

### 1.6.1 | `GenericExercise` Class

Inherits from `Exercise` and manages exercise logic with sets, reps, wait times, and movement sequences. It handles UI, score calculation, feedback, and saving progress to the backend.

```csharp
public class GenericExercise : Exercise {
    public string BackendEnum;
    public List<ExerciseMovement> Movements;
    public RepsAndSetsConfig RepsAndSetsConfig;
    public int AmountOfSets = 1;
    public float WaitTimeBetweenSets = 10f;
    public int AmountOfReps = 1;
    public float WaitTimeBetweenReps = 0.5f;

    private int currentSetIndex = 0;
    private int currentRepIndex = 0;
    private int currentMovementIndex = 0;

    private Image actionImageComponent;
    private Coroutine playSetsCoroutine;
    private Coroutine moveImageCoroutine;

    private ExerciseService excerciseSerice = new ExerciseService();

    [HideInInspector] public GenericExerciseScoreCalculator ScoreCalculator = new();

    protected GenericExerciseReferences refs => GenericExerciseReferences.Instance;
    private ExerciseMovement currentMovement => Movements[currentMovementIndex];

    public GenericExercise(
        string backendEnum, string title, ExerciseCategory category, string description, List<string> requirements,
    ) : base(title, category, description, requirements) {
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
                    refs.RepsAndSetsText.text = $"Set {currentSetIndex + 1}/{AmountOfSets}\nRep {currentRepIndex + 1}/{AmountOfReps}";
                    actionImageComponent.sprite = movement.InstructionImage;

                    yield return moveImageCoroutine = ExerciseManager.Instance.StartCoroutine(movement.Play());
                    movement.MovementEnded();
                    currentMovementIndex++;
                }

                if (currentRepIndex < AmountOfReps - 1 && WaitTimeBetweenReps > 0)
                    yield return new WaitForSeconds(WaitTimeBetweenReps);

                currentRepIndex++;
            }

            if (currentSetIndex < AmountOfSets - 1 && WaitTimeBetweenSets > 0) {
                actionImageComponent.enabled = false;
                yield return ShowRestUI(WaitTimeBetweenSets);
                actionImageComponent.enabled = true;
            }

            currentSetIndex++;
        }

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

        if (playSetsCoroutine != null)
            ExerciseManager.Instance.StopCoroutine(playSetsCoroutine);
        if (moveImageCoroutine != null)
            ExerciseManager.Instance.StopCoroutine(moveImageCoroutine);

        currentMovement.MovementEnded();

        refs.SequenceUI.SetActive(false);
        ExerciseManager.Instance.ExtraInfoObject.SetActive(false);
    }

    protected virtual void SaveExercise() {
        var dto = new CompletedExerciseDTO {
            exercise = BackendEnum,
            earnedPoints = (int)ScoreManager.Instance.Score,
            difficulty = DifficultyManager.Instance.SelectedDifficulty,
            completedAt = System.DateTime.UtcNow
        };

        ExerciseManager.Instance.StartCoroutine(excerciseSerice.SaveExercise(dto,
            onSuccess: res => Debug.Log(res.message),
            onError: err => Debug.Log(err.message),
            "standard"));
    }
}
```

---

### 1.6.2 | `GenericExerciseSO`

ScriptableObject that holds the configuration and data for a `GenericExercise`.

```csharp
[CreateAssetMenu(fileName = "NewGenericExercise", menuName = "Exercise/Generic")]
public class GenericExerciseSO : ExerciseSO {
    public string BackendEnum;
    public RepsAndSetsSO RepsAndSetsConfig;
    public List<ExerciseMovementSO> Movements;

    void OnEnable() {
        RepsAndSetsConfig = ScriptableObject.CreateInstance<RepsAndSetsSO>();
    }

    public override Exercise CreateExercise() {
        List<ExerciseMovement> movements = GetMovements();
        var exercise = new GenericExercise(
            backendEnum: BackendEnum,
            category: Category,
            title: Title,
            description: Description,
            requirements: Requirements
        );

        exercise.Movements = movements;
        exercise.RepsAndSetsConfig = RepsAndSetsConfig.CreateConfig();

        return exercise;
    }

    private List<ExerciseMovement> GetMovements() =>
        Movements.ConvertAll(moveSO => moveSO.CreateMovement());
}
```

---

### 1.6.3 | `ExerciseMovement`

Represents one single movement in an exercise, including its duration, target positions, instruction image, and behavior logic.

```csharp
public class ExerciseMovement {
    [HideInInspector] public GenericExercise exercise;

    public List<IMovementBehaviour> ExerciseBehaviours { get; set; }
    public float Duration { get; set; }
    public Sprite InstructionImage { get; set; }
    public float TotalScore { get; set; }

    public Vector3 LeftStickTarget, RightStickTarget, HeadTarget;
    public Vector3 startPos = new(400, 0, 0), endPos = new(-400, 0, 0);
    public float currentScore;

    public ExerciseMovement(Vector3 left, Vector3 right, Vector3 head, float duration, Sprite image, float score, List<IMovementBehaviour> behaviours) {
        LeftStickTarget = left;
        RightStickTarget = right;
        HeadTarget = head;
        Duration = duration;
        InstructionImage = image;
        TotalScore = score;
        ExerciseBehaviours = behaviours;
    }

    public IEnumerator Play() {
        foreach (var behaviour in ExerciseBehaviours) {
            behaviour.ExerciseMovement = this;
            behaviour.OnMovementStart(this);
        }

        float elapsed = 0f;
        while (elapsed < Duration) {
            GenericExerciseReferences.Instance.ActionImageLine.sizeDelta = new Vector2(Mathf.Lerp(0, 160, elapsed / Duration), GenericExerciseReferences.Instance.ActionImageLine.sizeDelta.y);
            GenericExerciseReferences.Instance.MovementImageObject.transform.localPosition = Vector3.Lerp(startPos, endPos, elapsed / Duration);
            elapsed += Time.deltaTime;

            if (Input.GetKeyDown(KeyCode.Space)) {
                elapsed = Duration;
                break;
            }

            yield return null;
        }

        while (Input.GetKey(KeyCode.Space)) {
            yield return null;
        }

        foreach (var behaviour in ExerciseBehaviours) {
            yield return behaviour.OnMovementUpdate(this);
        }
    }

    public void MovementEnded() {
        foreach (var behaviour in ExerciseBehaviours) {
            behaviour.OnMovementEnd(this);
        }
    }
}
```

---

## 2 | Score & Feedback Systems

The exercise system includes real-time scoring and feedback. Scores are calculated every frame based on proximity to target positions. Feedback is delivered through text, particles, and sounds to enhance user engagement.

---

### 2.1 | `GenericExerciseScoreCalculator`

Handles distance checking, percentage calculation, live UI feedback, and score awarding.

```csharp
public class GenericExerciseScoreCalculator {
    private float maxDistance = 1f;
    private float leftDist, rightDist, headDist;

    public float LeftPercentage { get; private set; }
    public float RightPercentage { get; private set; }
    public float HeadPercentage { get; private set; }

    public void CalculateDistances(Vector3 leftTarget, Vector3 rightTarget, Vector3 headTarget) {
        leftDist = Vector3.Distance(ExerciseManager.Instance.LeftStick.position, leftTarget);
        rightDist = Vector3.Distance(ExerciseManager.Instance.RightStick.position, rightTarget);
        headDist = Vector3.Distance(ExerciseManager.Instance.Headset.position, headTarget);

        LeftPercentage = Mathf.Clamp01(1f - (leftDist / maxDistance)) * 100f;
        RightPercentage = Mathf.Clamp01(1f - (rightDist / maxDistance)) * 100f;
        HeadPercentage = Mathf.Clamp01(1f - (headDist / maxDistance)) * 100f;

        var refs = GenericExerciseReferences.Instance;
        refs.LeftTextPercentage.text = LeftPercentage <= 0 ? "L TOO FAR!" : $"L {LeftPercentage:F0}%";
        refs.RightTextPercentage.text = RightPercentage <= 0 ? "R TOO FAR!" : $"R {RightPercentage:F0}%";
        refs.HeadTextPercentage.text = HeadPercentage <= 0 ? "H TOO FAR!" : $"H {HeadPercentage:F0}%";

        refs.LeftPulseAffordance.SetPulseScale(HeadPercentage);
        refs.RightPulseAffordance.SetPulseScale(HeadPercentage);
        refs.HeadsetPulseAffordance.SetPulseScale(HeadPercentage);
    }

    public bool IsAccurate() => LeftPercentage > 60 && RightPercentage > 60 && HeadPercentage > 60;
    public bool AllTargetsHit() => leftDist < maxDistance && rightDist < maxDistance && headDist < maxDistance;

    public float CalculateInstanceScore(float maxScore) {
        if (!AllTargetsHit()) return 0f;
        float score = ((LeftPercentage + RightPercentage + HeadPercentage) / 300f) * maxScore;
        ScoreManager.Instance.AddScore(score);
        return score;
    }

    public float CalculateDurationScore(float maxScore, float holdTime) {
        if (!AllTargetsHit()) return 0f;
        float framePercentage = (LeftPercentage + RightPercentage + HeadPercentage) / 3f;
        float frameScore = (framePercentage / 100f) * (maxScore / holdTime) * Time.deltaTime;
        return Mathf.Min(frameScore, maxScore);
    }

    public void DecideFeedback(float score, float maxScore) {
        Vector3 spawnPos = GenericExerciseReferences.Instance.MovementImageObject.transform.position + new Vector3(0, 0.5f, 0);

        if (!AllTargetsHit()) {
            FeedbackManager.Instance.DisplayMissFeedback(spawnPos);
            return;
        }

        ScoreManager.Instance.AddScore(score);
        FeedbackManager.Instance.CalculateAndDisplayFeedbackText(score, maxScore, spawnPos);
    }
}
```