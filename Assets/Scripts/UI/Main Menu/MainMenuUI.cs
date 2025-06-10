using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private RectTransform menuPanelScroll;
    [SerializeField] private Transform buttonContainer;
    [SerializeField] private GameObject dashboardPanel;
    [SerializeField] private Button dashboardButton;

    private GameObject currentlySelectedPanel;
    private Button currentlySelectedButton;

    void Start()
    {
        OpenDashboard();
        SoundManager.soundInstance.EnableAllSound();
        SoundManager.soundInstance.PlayMusic("Jazz_chill");

    }

    public void OpenDashboard() => SwitchToPanel(dashboardPanel, dashboardButton);

    public void SwitchToPanel(GameObject panelToActivate, Button buttonToHighlight)
    {
        SetActivePanel(panelToActivate);
        HighlightButton(buttonToHighlight);
        AdjustPanelHeight();
    }

    private void SetActivePanel(GameObject panelToActivate)
    {
        if (currentlySelectedPanel != null)
            currentlySelectedPanel.SetActive(false);

        panelToActivate.SetActive(true);
        currentlySelectedPanel = panelToActivate;
    }

    private void HighlightButton(Button buttonToHighlight)
    {
        if (currentlySelectedButton != null && currentlySelectedButton != buttonToHighlight)
            UIStyler.ApplyStyle(currentlySelectedButton, false);

        UIStyler.ApplyStyle(buttonToHighlight, true);
        currentlySelectedButton = buttonToHighlight;
    }

    private void AdjustPanelHeight()
    {
        float totalHeight = 20;

        foreach (Transform child in buttonContainer)
        {
            if (child.gameObject.activeSelf)
                totalHeight += 70;
        }

        menuPanelScroll.sizeDelta = new Vector2(menuPanelScroll.sizeDelta.x, totalHeight);
    }
}