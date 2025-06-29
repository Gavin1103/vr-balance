using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class BalanceTestWarning : MonoBehaviour {
    public Button Button;
    void Start() {
        ColorBlock cb = Button.colors;
        cb.normalColor = UIStyler.DisabledColor;
        cb.highlightedColor = UIStyler.AddBrightness(cb.normalColor, 0.15f);
        cb.pressedColor = UIStyler.GetDarker(cb.normalColor, 0.2f);
    }

    public void OnHover() {
        SoundManager.Instance.PlaySFX("Button Hover");
    }
    public void OnClick() {
        SoundManager.Instance.PlaySFX("Button Click");
        ExerciseManager.Instance.SelectBalanceTestExercise();
    }
}