using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class DefaultScrollbar : MonoBehaviour {
    public bool isSelected = false;
    void Awake() {
        Selectable selectable = GetComponentInChildren<Selectable>();
        UIStyler.ApplyStyle(selectable, isSelected, false);
    }

    public void OnHover() {
        SoundManager.Instance.PlaySFX("Button Hover");
    }
    public void OnClick() {
        SoundManager.Instance.PlaySFX("Button Click");
    }
}