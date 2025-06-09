using Models.User;
using Service;
using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using DTO.Request.User;


public class LoginManager : MonoBehaviour
{
    private string _identifier;
    private string _pincode;

    // Text
    public TextMeshProUGUI errorMessage;
    public TMP_InputField UserInputField;
    public TMP_InputField PinInputField;

    // UI menus
    public GameObject loginMenu;
    public GameObject mainMenu;

    private UserService userService;

    // Checks users in json list
    public List<string> existingUserList = new List<string>();

    void Awake()
    {
        userService = new UserService();
    }

    // Checks the data on both text fields to ensure if the user can login
    public void CheckLoginData()
    {
        if (_identifier != null && _pincode != null)
        {
            StartCoroutine(userService.Login(
                new UserLoginDTO { identifier = _identifier, pincode = _pincode },
                // Login completed
                onSuccess: ApiResponse =>
                {
                    // Saves token and username
                    PlayerPrefs.SetString("Login-Token", ApiResponse.data.token);

                    // Save user in the json file
                    AddUserToJsonFile();

                    LoginSucceeded();
                },
                // Error message
                onError: error =>
                {
                    if (error == null || string.IsNullOrEmpty(error.message)) {
                        errorMessage.text = "Er is een onbekende fout opgetreden.";
                    }
                    else {
                        errorMessage.text = error.message;
                    }

                    StartCoroutine(RemoveErrorText());
                }
            ));
        }
        else
        {
            errorMessage.text = "Username of pincode is niet ingevuld";
            StartCoroutine(RemoveErrorText());
        }
    }

    private void LoginSucceeded()
    {
        errorMessage.text = "Login gelukt!";
        StartCoroutine(LoadingMenu());
        StartCoroutine(RemoveErrorText());
    }

    // After a set amount of time, remove the error text
    IEnumerator RemoveErrorText()
    {
        yield return new WaitForSeconds(3f);
        errorMessage.text = "";
    }

    // Waits a few seconds to load main menu
    IEnumerator LoadingMenu()
    {
        yield return new WaitForSeconds(3f);
        loginMenu.SetActive(false);
        mainMenu.SetActive(true);

        // Reset all text so logging in again in the same session won't allow the fieldboxes to be filled
        ResetInputFields();
    }

    public void ResetInputFields()
    {
        // Reset all text so logging in again in the same session won't allow the fieldboxes to be filled
        errorMessage.text = "";
        UserInputField.text = "";
        PinInputField.text = "";
        
        _pincode = null;
        _identifier = null;
    }
    
    public void AddUserToJsonFile()
    {
        string path = Application.persistentDataPath + "/UserList.json";
        // Checks file for users
        if (File.Exists(path))
        {
            string existingJson = File.ReadAllText(path);
            UsernameList loaded = JsonUtility.FromJson<UsernameList>(existingJson);
            existingUserList = loaded.Usernames;
        }

        // Check if user exist or not
        if (!existingUserList.Contains(_identifier))
        {
            existingUserList.Add(_identifier);
        }

        // Saves the whole list in Json file, adding the new user to it
        UsernameList list = new UsernameList { Usernames = existingUserList };
        string newJson = JsonUtility.ToJson(list);
        File.WriteAllText(path, newJson);
    }

    // Makes sure the text written on the field boxes are saved
    public void ReadIdentifier(string user)
    {
        _identifier = user;
    }

    public void ReadPincode(string pass)
    {
        _pincode = pass;
    }

    public void SetIdentifier(string username)
    {
        _identifier = username;
        UserInputField.text = username;
        
        UserInputField.interactable = false;    }

    public void SetInputFieldOn()
    {
        UserInputField.interactable = true;
    }
}