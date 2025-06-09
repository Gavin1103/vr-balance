using UnityEngine;
using UnityEngine.UI;

public class PanelButton : MonoBehaviour {
    public GameObject associatedPanel;

    private MainMenuUI menuUI;
    private Button button;

    void Start() {
        button = GetComponentInChildren<Button>();
        menuUI = FindAnyObjectByType<MainMenuUI>();
        button.onClick.AddListener(OnClick);
    }

    private void OnClick() {
        menuUI.SwitchToPanel(associatedPanel, button);
    }
}