using System.Collections;
using System.Timers;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class CheckPosition : MonoBehaviour
{
    PositionManager positionManager;

    public GameObject boundary;
    BoundaryCreater boundaryCreater;

    private string boundName = "";
    //==================================================================================================================================================================
    void Start()
    {
        positionManager = boundary.GetComponent<PositionManager>();
        boundaryCreater = boundary.GetComponent<BoundaryCreater>();
    }

    // Picks the chosen exercise based on the name of the exercise (Sorted Alphabetically)
    public void CheckExercise()
    {
        // Probably do the same check as in ChosenExercise, eliminating the use of a switch case
        
        switch (positionManager.ExerciseName)
        {
            case "Plank":       Plank();        break;
            case "SideBend":    SideBend();     break;
            case "Squat":       Squat();        break;
            default: break;
        }
    }

    /*===========================================================================================================================================================
     *      Exercise Section:
     *      1. All exercises are sorted based on the alphabet
     *      2. The positions is fixed based on the chosen exercise (and difficulty, if the minigame/balance test has any)
     *      3. Pause menu is (de)activated if the player breaks or holds position
     */

    public void Plank()
    {
        bool isOn = true;
        boundName = "BoundY";
        boundaryCreater.CheckPositions(boundName, isOn);
    }

    public void SideBend()
    {
        bool isOn = true;
        boundName = "BoundXPos";
        boundaryCreater.CheckPositions(boundName, isOn);
    }

    public void Squat()
    {
        //Add difficulty set

        bool isOn = true;
        boundName = "BoundY";
        boundaryCreater.CheckPositions(boundName, isOn);
    }
    



}
