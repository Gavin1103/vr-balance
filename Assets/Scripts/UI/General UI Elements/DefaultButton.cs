using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class DefaultButton : MonoBehaviour {
    void Awake() {
        Selectable selectable = GetComponentInChildren<Selectable>();
        UIStyler.ApplyStyle(selectable, false, false);
    }

    public void OnHover() {
        SoundManager.soundInstance.PlaySFX("Button Hover");
    }
    public void OnClick() {
        SoundManager.soundInstance.PlaySFX("Button Click");
    }
}