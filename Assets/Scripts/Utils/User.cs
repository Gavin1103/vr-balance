using UnityEngine;

public static class User {
    private const string TokenKey = "Login-Token";
    private const string UsernameKey = "Username";

    public static void SetLogin(string token, string username) {
        PlayerPrefs.SetString(TokenKey, token);
        PlayerPrefs.SetString(UsernameKey, username);
        PlayerPrefs.Save();
    }

    public static string GetToken() {
        return PlayerPrefs.GetString(TokenKey, "");
    }

    public static string GetUsername() {
        return PlayerPrefs.GetString(UsernameKey, "");
    }

    public static bool IsLoggedIn() {
        return !string.IsNullOrEmpty(GetToken());
    }

    public static void Logout() {
        PlayerPrefs.DeleteKey(TokenKey);
        PlayerPrefs.DeleteKey(UsernameKey);
        PlayerPrefs.Save();
    }
}