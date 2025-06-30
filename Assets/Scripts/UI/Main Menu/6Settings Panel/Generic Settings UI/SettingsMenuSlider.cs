using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsMenuSlider : MonoBehaviour {
    [SerializeField] private VolumeType volumeType;
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private TextMeshProUGUI volumeText;

    private void Start() {
        float startValue = GetCurrentVolume();
        volumeSlider.value = startValue;
        UpdateVolumeText(startValue);

        volumeSlider.onValueChanged.AddListener(value => {
            UpdateVolumeText(value);
            SetVolume(value);
            SaveToSettings(value);
        });
    }

    private float GetCurrentVolume() {
        var settings = SettingsManager.Instance.CurrentSettings;
        return volumeType switch {
            VolumeType.SFX => settings.sfxVolume,
            VolumeType.Music => settings.musicVolume,
            VolumeType.Ambient => settings.ambientVolume,
            _ => 1f
        };
    }

    private void SetVolume(float value) {
        switch (volumeType) {
            case VolumeType.SFX: SoundManager.Instance.SetSFXVolume(value); break;
            case VolumeType.Music: SoundManager.Instance.SetMusicVolume(value); break;
            case VolumeType.Ambient: SoundManager.Instance.SetAmbientVolume(value); break;
        }
    }

    private void SaveToSettings(float value) {
        var settings = SettingsManager.Instance.CurrentSettings;
        switch (volumeType) {
            case VolumeType.SFX: settings.sfxVolume = value; break;
            case VolumeType.Music: settings.musicVolume = value; break;
            case VolumeType.Ambient: settings.ambientVolume = value; break;
        }
        SettingsManager.Instance.SaveSettings();
    }

    private void UpdateVolumeText(float value) {
        int percent = Mathf.RoundToInt(value * 100);
        volumeText.text = percent + "%";
    }
}