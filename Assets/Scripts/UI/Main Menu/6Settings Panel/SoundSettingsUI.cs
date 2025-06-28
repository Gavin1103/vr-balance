using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SoundSettingsUI : MonoBehaviour {
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private TextMeshProUGUI volumeText;

    private void Start() {
        // Update text at start
        UpdateVolumeText(volumeSlider.value);

        // Update text whenever slider changes
        volumeSlider.onValueChanged.AddListener(UpdateVolumeText);
    }

    private void UpdateVolumeText(float value) {
        int percent = Mathf.RoundToInt(value * 100);
        volumeText.text = percent + "%";
    }
}
