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
}
