using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoginMenuUI : MonoBehaviour {
    [SerializeField] private GameObject loginMenu;
    [SerializeField] private GameObject mainMenu;

    void Start() {
        SoundManager.soundInstance.PlaySFX("Startup");
    }
    
    public void OnLoginButtonPressed() {
        loginMenu.SetActive(false);
        mainMenu.SetActive(true);
    }
}