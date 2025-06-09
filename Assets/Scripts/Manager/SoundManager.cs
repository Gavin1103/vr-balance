using UnityEngine;

public class SoundManager : MonoBehaviour {
    public static SoundManager soundInstance { get; private set; }
    [Header("Audio Sources")]
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource ambientSource;

    [Header("Clip Libraries")]
    [SerializeField] private AudioClipLibrarySO sfxLibrary;
    [SerializeField] private AudioClipLibrarySO musicLibrary;
    [SerializeField] private AudioClipLibrarySO ambientLibrary;

    protected virtual void Awake() {
        soundInstance = this;
    }

    public void PlaySFX(string name) {
        var clip = sfxLibrary.GetClip(name);
        if (clip != null)
            sfxSource.PlayOneShot(clip);
        else
            Debug.LogWarning($"SFX clip '{name}' not found in the library.");
    }
    public void PlayMusic(string name) {
        var clip = musicLibrary.GetClip(name);
        if (clip != null) {
            musicSource.clip = clip;
            musicSource.Play();
        } else {
            Debug.LogWarning($"Music clip '{name}' not found in the library.");
        }
    }

    public void PlayAmbience(string name) {
        var clip = ambientLibrary.GetClip(name);
        if (clip != null) {
            ambientSource.clip = clip;
            ambientSource.Play();
        } else {
            Debug.LogWarning($"Ambient clip '{name}' not found in the library.");
        }
    }
    public void SetSFXVolume(float value) {
        sfxSource.volume = value;
    }
    public void SetMusicVolume(float value) {
        musicSource.volume = value;
    }

    public void SetAmbientVolume(float value) {
        ambientSource.volume = value;
    }

}
