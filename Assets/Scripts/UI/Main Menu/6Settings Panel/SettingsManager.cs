using UnityEngine;
using System.IO;

public class SettingsManager : MonoBehaviour {
    public static SettingsManager Instance { get; private set; }
    private string path;

    public SettingsData CurrentSettings { get; private set; } = new SettingsData();

    private void Awake() {
        Instance = this;
        path = Application.persistentDataPath + "/settings.json";

        LoadSettings();
    }

    public void SaveSettings() {
        string json = JsonUtility.ToJson(CurrentSettings);
        File.WriteAllText(path, json);
    }

    public void LoadSettings() {
        if (File.Exists(path)) {
            string json = File.ReadAllText(path);
            CurrentSettings = JsonUtility.FromJson<SettingsData>(json);
        }
    }
}