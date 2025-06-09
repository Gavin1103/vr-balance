using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    PositionManager positionManager;
    public GameObject pauseMenu;

    private void Start()
    {
        positionManager = pauseMenu.GetComponent<PositionManager>();
    }

    // Quit the current exercise
    public void ExitButton()
    {
        positionManager.EndGame();
    }

    // Restart the current exercise
    public void RestartButton()
    {
        positionManager.RestartGame();
    }
}
