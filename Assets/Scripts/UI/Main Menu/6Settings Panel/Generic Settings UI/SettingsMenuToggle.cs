using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsMenuToggle : MonoBehaviour {
    [SerializeField] private Toggle toggle;

    private void Start() {
        toggle.isOn = SettingsManager.Instance.CurrentSettings.vfxEnabled;

        toggle.onValueChanged.AddListener(OnToggleChanged);
    }

    
    private void OnToggleChanged(bool value) {
        SettingsManager.Instance.CurrentSettings.vfxEnabled = value;
        SettingsManager.Instance.SaveSettings();
    }
}