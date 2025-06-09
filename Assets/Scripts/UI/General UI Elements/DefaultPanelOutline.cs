using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class DefaultPanelOutline : MonoBehaviour {
    void Start() {
        Image outline = GetComponentInChildren<Image>();
        UIStyler.RegisterGraphic(outline);
    }
}