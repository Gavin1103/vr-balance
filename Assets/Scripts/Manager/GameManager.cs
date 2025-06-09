using System;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour {
    // Singleton instance to access GameManager globally
    public static GameManager Instance { get; private set; }

    //[SerializeField] private FireFlyWaveManager fireFlyWaveManager;

    // Enum to define different game modes
    public enum GameMode {
        FireFly,
        BalanceTest
    }

    private void Awake() {
        // Ensure only one GameManager exists and persist it between scenes
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Prevent destruction on scene load
        }
        else {
            Destroy(gameObject); // Destroy duplicates
        }
    }

    /// <summary>
    /// Starts the selected game mode and disables the passed UI.
    /// </summary>
    /// <param name="UI">The UI GameObject to disable when the game starts</param>
    /// <param name="mode">The selected game mode</param>
    public void StartGame(GameObject UI, GameMode mode) {
        Debug.Log("start game");

        // Hide the UI before starting the game
       // UIManager.UiInstance.DisableUI(UI);

        // Handle logic for the selected game mode
        switch (mode) {
            case GameMode.FireFly:
                Debug.Log("FireFly mode started");
                FireFlyWaveManager.FireFlyInstance.StartWaves();
                SoundManager.soundInstance.PlayMusic("BackgroundMusic");
                SoundManager.soundInstance.PlayAmbience("ForestAmbience");
                break;

            case GameMode.BalanceTest:
                Debug.Log("BalanceTest mode started");
                // Add BalanceTest start logic here
                break;
        }
    }

    /// <summary>
    /// Starts the FireFly game mode. Called from UI buttons.
    /// </summary>
    public void StartFireFly(GameObject UI) {
        StartGame(UI, GameMode.FireFly);
    }

    /// <summary>
    /// Starts the BalanceTest game mode. Called from UI buttons.
    /// </summary>
    public void BalanceTest(GameObject UI) {
        StartGame(UI, GameMode.BalanceTest);
    }

    public void SetActiveMap(GameObject map) {
        map.SetActive(true);
    }

    public void DisableMap(GameObject map) {
        map.SetActive(false);
    }
}
