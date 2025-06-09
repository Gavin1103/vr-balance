using UnityEngine;

public class Timers : MonoBehaviour
{
    [SerializeField] private float exerciseTimer;
    [SerializeField] private float startTimer = 5f;
    [SerializeField] private float endTimer = 3f;

    private bool startBufferTimer = false;

    PositionManager positionManager;
    BoundaryCreater boundaryCreater;
    AffordanceCheck affordanceCheck;

    void Start()
    {
        positionManager = GetComponent<PositionManager>();
        boundaryCreater = GetComponent<BoundaryCreater>();
        affordanceCheck = GetComponent<AffordanceCheck>();
    }

    void Update()
    {
        if (startBufferTimer)
        {
            StartTimer();
        }
    }
    // If the player hits the target position. start counting down.
    public void StartTimer()
    {
        startTimer -= Time.deltaTime;
        if (startTimer < 0) { ExerciseTimer(); }
    }

    // If the player hold the target position after StartTimer() reached 0. start ExerciseTimer
    public void ExerciseTimer()
    {
            /* if (game is only balance test ) { exerciseTimer -= Time.deltaTime; } 
             * else if (game is NPC test ) { } 
             * else if (game doesn't need timer) { }
             */

            exerciseTimer -= Time.deltaTime;
            affordanceCheck.ColouredLine(Color.green);

            positionManager.StartGame();

            FinishTimer();  
    }

    // If exercise timer hits 0 finish the current exercise. Only needed if the game needs position checking.
    public void FinishTimer()
    {
        if (exerciseTimer < 0) { 
            startBufferTimer = false;
            affordanceCheck.ColouredLine(Color.red);
            boundaryCreater.TurnOffChecker(); 
        }          
    }

    // If player is in rest position. endTimer starts and ends the exercise
    public void EndTimer()
    {
        affordanceCheck.ColouredLine(Color.green);
        endTimer -= Time.deltaTime;

        if (endTimer < 0)
        {
            positionManager.EndGame();
        }
    }

    public bool StartBufferTimer { get { return startBufferTimer; } set { startBufferTimer = value; } }
    public float ResetStartTimer {  get { return startTimer; } set { startTimer = value; } }
    public float ResetEndTimer { get { return endTimer; } set { endTimer = value; } }
    public float ResetExerciseTimer { get { return exerciseTimer; } set { exerciseTimer = value; } }
}
