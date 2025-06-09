using UnityEngine;

public class Boundss : MonoBehaviour
{
    // Pause menu checker
    private bool isActivated = true;
    public GameObject pauseMenuUI;

    Timers timer;
    AffordanceCheck affordanceCheck;
    void Start()
    {
        timer = GetComponent<Timers>();
        affordanceCheck = GetComponent<AffordanceCheck>();
    }

    // If player is in position, start timer and give feedback
    public void InBounds()
    {
        timer.StartBufferTimer = true;
        affordanceCheck.ColouredLine(Color.yellow);

        isActivated = false;
        PauseMenuActivation(isActivated);
    }

    // If player is not in position, reset start timer, show pause menu and give feedback
    public void OutBounds()
    {
        timer.StartBufferTimer = false;
        affordanceCheck.ColouredLine(Color.red);
        timer.ResetStartTimer = 5f;

        if (!isActivated) {
        isActivated = true;
        PauseMenuActivation(isActivated);
        }
    }

    // If player is in rest position, start timer
    public void InBounds_RestPos()
    {
        timer.EndTimer();
    }

    // If player is not in rest position, reset end timer, give feedback and deactivate pause menu so it won't show the second you turn on next exercise
    public void OutBounds_RestPos()
    {
        isActivated = true;
        timer.ResetEndTimer = 3f; 
        affordanceCheck.ColouredLine(Color.red);
    }

    //  Turn on/off the pause menu while the exercise is active and position is broken
    public void PauseMenuActivation(bool isActivated)
    {
        pauseMenuUI.SetActive(isActivated);
    }
}
