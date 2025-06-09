using UnityEngine;

public class Logout : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject startMenu;

    public void LogoutUser()
    {

        // Delete current token and username, so the game forgets that user is logged in
        PlayerPrefs.DeleteKey("Login-Token");
   
        mainMenu.SetActive(false);
        startMenu.SetActive(true);
    }
}
