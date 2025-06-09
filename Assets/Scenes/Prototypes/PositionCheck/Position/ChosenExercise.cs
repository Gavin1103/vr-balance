using JetBrains.Annotations;
using UnityEngine;

public class ChosenExercise : MonoBehaviour
{
    /*
     *  Settings for Exercise after pressing the UI button for one exercise
     *  exercise:       sends string of said exercise
     *  exerciseTimer:  sends total amount of time the exercise lasts
     *  
     *  Minimum and Maximum position values if the minigame/difficulty/balance test needs it
     *  minimumY:       Sends the minimum Y position for the exercise to start
    */
    [SerializeField] private string exerciseName;
    [SerializeField] private float exerciseTimer;

    private float minimumY;
    private float minimumX;
    private float minimumZ;
    private float maximumY;
    private float maximumX;
    private float maximumZ;

    //private CheckPosition checkPosition;
    PositionManager positionManager;
    public GameObject positionManagerObj;

    /*  Add function from Amine's exerciseManager
     *  Take name and difficulty
     *  make each function be called from string using:     
     *      public string calledFunctionName;
     *      Invoke(calledFunctionName, 0);
     *  difficulty using if statement within called function which position to take. ignore if no position is needed
     */

    // All exercises are sorted based on the alphabet
    public void Squat()
    {
        exerciseName = "Squat";
        exerciseTimer = 10f;
        //Set all position limitations minimum and maximum for X Y Z
        //currently public for testing purposes
        minimumY = 0.30f;
        maximumY = 0.60f;

        FinalizeSetting();
    }

    public void SideBend()
    {
        exerciseName = "SideBend";
        exerciseTimer = 10f;

        minimumX = 0.20f;
        maximumX = 0.40f;

        FinalizeSetting();
    }

    public void Plank()
    {
        exerciseName = "Plank";
        exerciseTimer = 10f;

        minimumY = 3.00f;
        maximumY = 3.50f;

        FinalizeSetting();
    }

    public void FinalizeSetting()
    {
        positionManager = positionManagerObj.GetComponent<PositionManager>();
        positionManager.Settings();
    }

    /*==================================================================================================================================
    *   Get Set
    */

    public string ExerciseName { get { return exerciseName; } }
    public float ExerciseTimer { get { return exerciseTimer; } }
    public float MinimumY { get { return minimumY; } }
    public float MinimumX { get { return minimumX; } }
    public float MinimumZ { get { return minimumZ; } }
    public float MaximumY { get { return maximumY; } }
    public float MaximumX { get { return maximumX; } }
    public float MaximumZ { get { return maximumZ; } }
}
