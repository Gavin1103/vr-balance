using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class LoginMenuUI : MonoBehaviour {
    [SerializeField] private GameObject loginMenu;
    [SerializeField] private GameObject mainMenu;

    void Start() {
        SoundManager.Instance.PlaySFX("Startup");
        User.Logout();
    }
    
    public void OnLoginButtonPressed() {
        loginMenu.SetActive(false);
        mainMenu.SetActive(true);
    }
}   