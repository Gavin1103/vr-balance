using UnityEngine;

public class PlayAudio : MonoBehaviour {
    public string sfxName;
    public string musicName;
    public string ambienceName;

    private void OnEnable() {
        if (!string.IsNullOrEmpty(sfxName))
            SoundManager.Instance.PlaySFX(sfxName);

        if (!string.IsNullOrEmpty(musicName))
            SoundManager.Instance.PlayMusic(musicName);

        if (!string.IsNullOrEmpty(ambienceName))
            SoundManager.Instance.PlayAmbience(ambienceName);
    }

    private void OnDisable() {
        // Stop music if this component started it
        if (!string.IsNullOrEmpty(musicName))
            SoundManager.Instance.StopMusic();

        // Stop ambience if this component started it
        if (!string.IsNullOrEmpty(ambienceName))
            SoundManager.Instance.StopAmbience();
    }
}