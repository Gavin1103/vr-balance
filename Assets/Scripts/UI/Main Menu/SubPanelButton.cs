using UnityEngine;
using UnityEngine.UI;

public class SubPanelButton : MonoBehaviour {
    public GameObject associatedPanel;

    public void ShowContent() {
        if (associatedPanel != null) {
            associatedPanel.SetActive(true);
        }
    }

    public void HideContent() {
        if (associatedPanel != null) {
            associatedPanel.SetActive(false);
        }
    }
}