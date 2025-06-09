using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class BoundaryCreater : MonoBehaviour
{
    private PositionManager positionManager;
    private Boundss boundss;

    private float positiveZ_Minimum;
    private float positiveZ_Maximum;
    private float negativeZ_Minimum;
    private float negativeZ_Maximum;

    private float positiveX_Minimum;
    private float positiveX_Maximum;
    private float negativeX_Minimum;
    private float negativeX_Maximum;

    private float positiveY_Minimum;
    private float positiveY_Maximum;
    private float restPosition;

    Vector3 currentPosition;

    private bool activateCurrentPositionCheck;
    private bool exerciseOver = false;
    public string nameofbound;

    void Start()
    {
        positionManager = GetComponent<PositionManager>();
        boundss = GetComponent<Boundss>();
    }
    
    // Get current headset position
    void Update()
    {
        if (positionManager.ExerciseStart == true)
        {
            positionManager.HeightCalibration();
            positionManager.StartDataCollection();
        }

        currentPosition = positionManager.CurrentHeadsetPosition;
        ActivateCurrentPositionCheck();

        if (exerciseOver == true)
        {
            RestPosition();
        }
    }

    // Set boundary values
    public void SetBoundary(float minimumY, float minimumX, float minimumZ, float maximumY, float maximumX, float maximumZ)
    {
        positiveZ_Minimum = positionManager.HeadsetPosition.z + minimumZ;
        positiveZ_Maximum = positionManager.HeadsetPosition.z + maximumZ;
        negativeZ_Minimum = positionManager.HeadsetPosition.z - minimumZ;
        negativeZ_Maximum = positionManager.HeadsetPosition.z - maximumZ;

        positiveX_Minimum = positionManager.HeadsetPosition.x + minimumX;
        positiveX_Maximum = positionManager.HeadsetPosition.x + maximumX;
        negativeX_Minimum = positionManager.HeadsetPosition.x - minimumX;
        negativeX_Maximum = positionManager.HeadsetPosition.x - maximumX;

        positiveY_Minimum = positionManager.HeadsetPosition.y + minimumY;
        positiveY_Maximum = positionManager.HeadsetPosition.y + maximumY;

        restPosition = positionManager.HeadsetPosition.y + 0.03f;
    }

    /*
     *  The next few functions are checks some excersises can take.
     *  Under Bound = When player reaches the minimum body movement to start an exercise
     *  Upper Bound = When player surpasses the maximum body movement, the game will be paused
     */

    //Takes the chosen exercise boundary name and sets it and turn the boundary on.
    public void CheckPositions(string boundName, bool isOn)
    {
        nameofbound = boundName;
        activateCurrentPositionCheck = isOn;
    }

    public void ActivateCurrentPositionCheck()
    {
        if (activateCurrentPositionCheck) {
            switch (nameofbound)
            {
                case "BoundY": Boundary_Y(); break;
                case "BoundXPos": Boundary_X_Positive(); break;
                case "BoundXNeg": Boundary_X_Negative(); break;
                case "BoundZPos": Boundary_Z_Positive(); break;
                case "BoundZNeg:": Boundary_Z_Negative(); break;
                case "BoundXPosNeg": Boundary_X_Positive_Negative(); break;
                default: break;
            }
        }
    }

    // Lower the body
    public void Boundary_Y()
    {
        if (currentPosition.y > positiveY_Minimum && currentPosition.y < positiveY_Maximum)
        {
            InBounds();
            return;
        }
        OutofBounds();
    }

    //================================================================================================================================================================================

    // Moves body to the right
    public void Boundary_X_Positive()
    {
        if (currentPosition.x > positiveX_Minimum && currentPosition.x < positiveX_Maximum)
        {
            InBounds();
            return;
        }
        OutofBounds();
    }

    // Moves body to the left
    public void Boundary_X_Negative()
    {
        if (currentPosition.x < negativeX_Minimum && currentPosition.x > negativeX_Maximum)
        {
            InBounds();
            return;
        }
        OutofBounds();
    }

    //================================================================================================================================================================================

    // Moves body forwards
    public void Boundary_Z_Positive()
    {
        if (currentPosition.z > positiveZ_Minimum && currentPosition.z < positiveZ_Maximum )
        {
            InBounds();
            return;
        }
        OutofBounds();
    }

    // Moves body backwards
    public void Boundary_Z_Negative()
    {
        if (currentPosition.z < negativeZ_Minimum && currentPosition.z > negativeZ_Maximum)
        {
            InBounds();
            return;
        }
        OutofBounds();
    }

    //================================================================================================================================================================================

    // moves body to the right or left
    public void Boundary_X_Positive_Negative()
    {
        if (currentPosition.x > positiveX_Minimum && currentPosition.x < positiveX_Maximum ||
            currentPosition.x < negativeX_Minimum && currentPosition.x > negativeX_Maximum)
        {
            InBounds();
            return;
        }
        OutofBounds();
    }

    // If player is in position
    public void InBounds()
    {
        boundss.InBounds();
    }

    // If player is out position
    public void OutofBounds()
    {
        boundss.OutBounds();
    }

    //================================================================================================================================================================================

    // Sets current exercises bounds off. so the player cannot do the same movement and see it functioning
    public void TurnOffChecker()
    {
        activateCurrentPositionCheck = false;
        exerciseOver = true;
    }

    // Rest position after the exercise has finished
    public void RestPosition()
    {
        if (currentPosition.y < restPosition)
        {
            RP_InBounds();
            return;
        }
        RP_OutBounds();
    }

    public void RP_InBounds()
    {
        boundss.InBounds_RestPos();
    }

    public void RP_OutBounds()
    {
        boundss.OutBounds_RestPos();
    }

    /*========================================================================================================================================================
*      Get Set
*/
    public bool ExerciseOver { get { return exerciseOver; } set { exerciseOver = value; } }
}
