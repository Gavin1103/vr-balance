using UnityEngine;

public class SoundManager : MonoBehaviour {
    public static SoundManager Instance { get; private set; }
    [Header("Audio Sources")]
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource ambientSource;

    [Header("Clip Libraries")]
    [SerializeField] private AudioClipLibrarySO sfxLibrary;
    [SerializeField] private AudioClipLibrarySO musicLibrary;
    [SerializeField] private AudioClipLibrarySO ambientLibrary;

    protected virtual void Awake() {
        Instance = this;
    }
    
    void Start() {
        var data = SettingsManager.Instance.CurrentSettings;
        SetSFXVolume(data.sfxVolume);
        SetMusicVolume(data.musicVolume);
        SetAmbientVolume(data.ambientVolume);
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

    public void StopAllSound() {
        sfxSource.enabled = false;
        musicSource.enabled = false;
        ambientSource.enabled = false;
    }

    public void EnableAllSound() {
        sfxSource.enabled = true;
        musicSource.enabled = true;
        ambientSource.enabled = true;
    }
    
    public void StopMusic() {
        musicSource.Stop();
        musicSource.clip = null;
    }

    public void StopAmbience() {
        ambientSource.Stop();
        ambientSource.clip = null;
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
