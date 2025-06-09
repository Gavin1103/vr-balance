using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MapButton : MonoBehaviour {
    public TMP_Text label;
    public Image image;
    public Button button;

    public void Setup(string mapName, UnityEngine.Events.UnityAction callback, Sprite image) {
        label.text = mapName;
        this.image.sprite = image;

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(callback);
    }
}