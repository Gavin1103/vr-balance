using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.IO;

public class LoadUserButton : MonoBehaviour
{
    // UI menus
    public GameObject startMenu;
    public GameObject loginMenu;
    LoginManager _loginManager;
    public GameObject loginManager;

    // Button references to spawn buttons
    public GameObject ButtonPrefab;
    public Transform ButtonContainer;

    public List<string> existingUserList = new List<string>();

    void Start()
    {
        LoadExistingUsers();
        _loginManager = loginManager.GetComponent<LoginManager>();
    }
    
    // Checks users in Json file and initializes buttons
    public void LoadExistingUsers()
    {
        string path = Application.persistentDataPath + "/UserList.json";

        //Debug.unityLogger.Log(path);
        // Verwijder eerst bestaande knoppen
        foreach (Transform child in ButtonContainer)
        {
            Destroy(child.gameObject);
        }

        // Laad gebruikers uit JSON
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            UsernameList loaded = JsonUtility.FromJson<UsernameList>(json);
            existingUserList = loaded.Usernames;
        }
        else
        {
            Debug.Log("No users found.");
            existingUserList.Clear();
        }

        // Maak knop voor elke gebruiker
        foreach (string username in existingUserList)
        {
            GameObject buttonObj = Instantiate(ButtonPrefab, ButtonContainer);
            Button button = buttonObj.GetComponent<Button>();
            TMP_Text buttonText = buttonObj.GetComponentInChildren<TMP_Text>();

            buttonText.text = username;
            button.onClick.AddListener(() => ChosenUser(username));
        }
    }

    // Choosing a specific initialized button focusses on that user
    public void ChosenUser(string chosenUser)
    {
        startMenu.SetActive(false);
        loginMenu.SetActive(true);
        _loginManager.SetIdentifier(chosenUser);
    }
}
