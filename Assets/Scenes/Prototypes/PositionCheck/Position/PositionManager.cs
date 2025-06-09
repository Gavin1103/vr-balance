using UnityEngine;

public class PositionManager : MonoBehaviour
{
    ChosenExercise chosenExercise;
    CheckPosition checkPosition;

    public GameObject excel;
    ExcelData excelData;

    // Exercise
    private bool exerciseStart;
    private string exerciseName;

    // Position 
    [SerializeField] private float defaultHeight = 1.8f;
    [SerializeField] private Vector3 currentHeadsetPosition;
    [SerializeField] private Vector3 headsetPosition;

    private float minimumY;
    private float minimumX;
    private float minimumZ;
    private float maximumY;
    private float maximumX;
    private float maximumZ;

    // Gameobjects and scripts
    public GameObject mainCamera;
    public GameObject startUI;

    BoundaryCreater boundaryCreater;

    Timers timer;
    AffordanceCheck affordanceCheck;

    //Feedback Affordance
    public GameObject cubeFeedback;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        timer = GetComponent<Timers>();
        chosenExercise = mainCamera.GetComponent<ChosenExercise>();
        checkPosition = mainCamera.GetComponent<CheckPosition>();
        boundaryCreater = GetComponent<BoundaryCreater>();
        affordanceCheck = GetComponent<AffordanceCheck>();

        excelData = excel.GetComponent<ExcelData>();

    }

    //  Put all data of the chosen exercise in the variables and calibrate your current height
    public void Settings()
    {
        //if statement if the exercise use any position check, if not skip straight to StartGame()

        exerciseStart = true;
        exerciseName = chosenExercise.ExerciseName;
        timer.ResetExerciseTimer = chosenExercise.ExerciseTimer;


        minimumY = chosenExercise.MinimumY;
        maximumY = chosenExercise.MaximumY;
        minimumX = chosenExercise.MinimumX;
        maximumX = chosenExercise.MaximumX;
        minimumZ = chosenExercise.MinimumZ;
        maximumZ = chosenExercise.MaximumZ;

        HeightCalibration();
        headsetPosition = currentHeadsetPosition;

        boundaryCreater.SetBoundary(minimumY, minimumX, minimumZ, maximumY, maximumX, maximumZ);
        
        FeedbackLine(exerciseStart);
        affordanceCheck.ColouredLine(Color.red);

        StartDataCollection();
        
        // puts on which boundary to turn on based on the difficulty
        checkPosition.CheckExercise();
    }
    // Puts a line on the top of the player's head if the game
    public void FeedbackLine(bool isActive)
    {
        cubeFeedback.SetActive(isActive);
    }

    //Updates the X Y Z positions for the user of different heights. Making exercise fair for all
    public void HeightCalibration()
    {
        float headHeight = mainCamera.transform.localPosition.y;
        float scale = defaultHeight / headHeight;
        transform.localScale = Vector3.one * scale;
        currentHeadsetPosition = transform.localScale;

        currentHeadsetPosition.z = mainCamera.transform.localPosition.z;
        currentHeadsetPosition.x = mainCamera.transform.localPosition.x;
    }

    //====================================================================================================================================================================

    /*      Starts, Restarts, Ends exercise for position
     *      
     *      Start       Start the actual exercise/minigame/balance test
     *      Restart     Resets the game from the start
     *      End         stops the current exercise. 
     *      
     *      Restart and End will send currently made data to the database and put data collection off
     *      Restart turns the data collection on after resetting from the start
     */

    public void StartGame()
    {
        // Return to Amine's exerciseManager
    }

    public void RestartGame()
    {  
        SendData();
        Settings();
    }

    public void EndGame()
    {
        //if statement if the current exercise used any position checker, if not ignore and go straight to data sending
        exerciseStart = false;
        timer.ResetEndTimer = 3f;
        timer.ResetStartTimer = 5f;
        timer.ResetExerciseTimer = chosenExercise.ExerciseTimer;
        startUI.SetActive(true);
        boundaryCreater.ExerciseOver = false;
        FeedbackLine(exerciseStart);

        SendData();

        // Return to Amine's exerciseManager
    }

    // Send data to the database and stop collecting
    public void SendData()
    {
        Debug.Log("Data Send");
        // Ends data collection and send to database


        //For research experiment
        //excelData.EndDataCollection();
    }

    // Start data collection the moment an exercise is chosen
    public void StartDataCollection()
    {
        //Put instance to data collection

        //For research experiment
        //excelData.StartDataCollection(exerciseName);
    }

    /*========================================================================================================================================================
 *      Get Set
 */
    public bool ExerciseStart { get { return exerciseStart; } set { exerciseStart = value; } }
    public Vector3 CurrentHeadsetPosition { get { return currentHeadsetPosition; } }
    public Vector3 HeadsetPosition { get { return headsetPosition; } }
    public string ExerciseName { get { return exerciseName; } }


}
