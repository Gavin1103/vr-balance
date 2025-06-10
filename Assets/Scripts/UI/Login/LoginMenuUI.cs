using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class LoginMenuUI : MonoBehaviour {
    [SerializeField] private GameObject loginMenu;
    [SerializeField] private GameObject mainMenu;

    void Start() {
        StartCoroutine(PlayStartupAndThenMusic());
    }

    private IEnumerator PlayStartupAndThenMusic() {
        SoundManager.soundInstance.PlaySFX("Startup");
        yield return new WaitForSeconds(2f); // Wait for SFX to finish (adjust time)
        SoundManager.soundInstance.PlayMusic("Jazz_chill");
    }
}