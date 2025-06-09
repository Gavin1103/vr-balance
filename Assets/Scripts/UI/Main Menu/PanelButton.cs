using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class PanelButton : MonoBehaviour {
    public GameObject associatedPanel;
    public List<Button> subButtons = new List<Button>();

    private MainMenuUI menuUI;
    private Button designatedButton;

    private Dictionary<Button, SubPanelButton> subButtonMap = new Dictionary<Button, SubPanelButton>();
    private Button currentlySelectedSubButton;

    void Start() {
        designatedButton = GetComponentInChildren<Button>();
        menuUI = FindAnyObjectByType<MainMenuUI>();
        designatedButton.onClick.AddListener(OnDesignatedButtonClick);

        foreach (var subButton in subButtons) {
            SubPanelButton subPanelButton = subButton.GetComponent<SubPanelButton>();
            if (subPanelButton != null) {
                subButtonMap[subButton] = subPanelButton;
                subButton.onClick.AddListener(() => OnSubPanelButtonClick(subButton));
            }
        }

        HideSubButtons();
    }

    private void OnDesignatedButtonClick() {
        menuUI.SwitchToPanel(associatedPanel, designatedButton);

        if (subButtons.Count > 0 && subButtonMap.ContainsKey(subButtons[0])) {
            ShowSubPanelContent(subButtonMap[subButtons[0]]);
            HighlightSubButton(subButtons[0]); // Select the first
        }
    }

    private void OnSubPanelButtonClick(Button subButton) {
        if (currentlySelectedSubButton == subButton) return;

        if (subButtonMap.ContainsKey(subButton)) {
            ShowSubPanelContent(subButtonMap[subButton]);
        }

        HighlightSubButton(subButton);
    }

    private void HighlightSubButton(Button subButton) {
        if (currentlySelectedSubButton != null) {
            UIStyler.ApplyStyle(currentlySelectedSubButton, false);
        }

        UIStyler.ApplyStyle(subButton, true);
        currentlySelectedSubButton = subButton;
    }

    public void ShowSubButtons() {
        foreach (var subButton in subButtons)
            subButton.gameObject.SetActive(true);
    }

    public void HideSubButtons() {
        if (currentlySelectedSubButton != null) {
            UIStyler.ApplyStyle(currentlySelectedSubButton, false);
            currentlySelectedSubButton = null;
        }

        foreach (var subButton in subButtons) {
            subButton.gameObject.SetActive(false);
        }
    }
    public void ShowSubPanelContent(SubPanelButton selectedSubPanelButton) {
        foreach (var subPanelButton in subButtonMap.Values) {
            if (subPanelButton == selectedSubPanelButton) {
                subPanelButton.ShowContent();
            } else {
                subPanelButton.HideContent();
            }
        }
    }
}