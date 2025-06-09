using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
public class MainMenuUI : MonoBehaviour {
    [SerializeField] private RectTransform menuPanelScroll;
    [SerializeField] private Transform buttonContainer;
    [SerializeField] private GameObject dashboardPanel;
    [SerializeField] private Button dashboardButton;
    private PanelButton[] panelButtons;
    private GameObject currentlySelectedPanel;
    private Button currentlySelectedButton;
    private PanelButton currentlySelectedPanelButton; // Tracks the currently selected PanelButton

    void Start() {
        panelButtons = FindObjectsByType<PanelButton>(FindObjectsSortMode.None);
        OpenDashboard();
    }

    public void OpenDashboard() => SwitchToPanel(dashboardPanel, dashboardButton);

    public void SwitchToPanel(GameObject panelToActivate, Button buttonToHighlight) {
        // Unselect subbuttons of the previously selected panel
        if (currentlySelectedPanelButton != null) {
            currentlySelectedPanelButton.HideSubButtons();
        }

        // Set the new active panel
        SetActivePanel(panelToActivate);

        // Update the currently selected PanelButton
        currentlySelectedPanelButton = GetPanelButtonByPanel(panelToActivate);

        // Toggle subbuttons and highlight the new button
        ToggleSubButtons(panelToActivate, buttonToHighlight);
        HighlightButton(buttonToHighlight);
        AdjustPanelHeight();
    }

    private void SetActivePanel(GameObject panelToActivate) {
        if (currentlySelectedPanel != null)
            currentlySelectedPanel.SetActive(false);

        panelToActivate.SetActive(true);
        currentlySelectedPanel = panelToActivate;
    }

    private void HighlightButton(Button buttonToHighlight) {
        if (currentlySelectedButton != null && currentlySelectedButton != buttonToHighlight)
            UIStyler.ApplyStyle(currentlySelectedButton, false);

        UIStyler.ApplyStyle(buttonToHighlight, true);
        currentlySelectedButton = buttonToHighlight;
    }

    private void ToggleSubButtons(GameObject panelToActivate, Button buttonToHighlight) {
        foreach (var panelBtn in panelButtons) {
            // Only expand sub-buttons on the selected panel
            if (panelBtn.associatedPanel == panelToActivate) {
                panelBtn.ShowSubButtons();
            } else {
                panelBtn.HideSubButtons();
            }
        }
    }

    private void AdjustPanelHeight() {
        float totalHeight = 20;

        foreach (Transform child in buttonContainer) {
            if (child.gameObject.activeSelf)
                totalHeight += 70;
        }

        menuPanelScroll.sizeDelta = new Vector2(menuPanelScroll.sizeDelta.x, totalHeight);
    }

    private PanelButton GetPanelButtonByPanel(GameObject panel) {
        foreach (var panelBtn in panelButtons) {
            if (panelBtn.associatedPanel == panel) {
                return panelBtn;
            }
        }
        return null;
    }
}