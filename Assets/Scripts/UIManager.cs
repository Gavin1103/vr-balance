using UnityEngine;

public class UIManager : MonoBehaviour {
    // Singleton instance to access the UIManager globally
    public static UIManager Instance { get; private set; }

    private void Awake() {
        Instance = this;
    }

    /// <summary>
    /// Disables the given UI GameObject (e.g., menu, canvas, etc.)
    /// </summary>
    /// <param name="UIGameobject">UI GameObject to disable</param>
    public void DisableUI(GameObject UIGameobject) {
        UIGameobject.SetActive(false);
    }

    /// <summary>
    /// Enables the given UI GameObject
    /// </summary>
    /// <param name="UIGameobject">UI GameObject to enable</param>
    public void EnableUI(GameObject UIGameobject) {
        UIGameobject.SetActive(true);
    }
}
