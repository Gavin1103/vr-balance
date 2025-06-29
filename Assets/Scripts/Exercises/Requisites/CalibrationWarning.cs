using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class CalibrationWarning : MonoBehaviour {
    public Button Button;
    public MainMenuUI MainMenuUI;
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

        ExerciseManager.Instance.SelectedExerciseInfo.SetActive(false);
        ExerciseManager.Instance.Leaderboard.SetActive(false);
        ExerciseManager.Instance.ExercisesMenu.SetActive(true);

        MainMenuUI.OpenDashboard();
    }
}