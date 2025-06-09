using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using UnityEngine.Serialization;
//using UnityEngine.XR.ARFoundation;

public class MapManager : MonoBehaviour {
    public static MapManager Instance { get; private set; }

    [System.Serializable]
    public class MapEntry {
        [FormerlySerializedAs("mapName")]
        public string name;
        [FormerlySerializedAs("mapDescription")]
        public string description;
        [FormerlySerializedAs("mapObject")]
        public GameObject mapObject;
        [FormerlySerializedAs("mapImage")]
        public Sprite icon;
        [FormerlySerializedAs("mapColor")]
        public Color themeColor;
    }
    public List<MapEntry> maps;
    // References and prefabs
    public Transform mapButtonsContainer;
    public GameObject mapButtonPrefab;
    public TextMeshProUGUI mapInfoTitle;
    public TextMeshProUGUI mapInfoDescription;
    
    [HideInInspector] public GameObject CurrentActiveMap; // HAS TO BE NULL
    private List<MapButton> mapButtons = new List<MapButton>();
    private MapButton currentActiveMapButton;

    void Start() {
        Instance = this;
        // Create map buttons
        for (int i = 0; i < maps.Count; i++) {
            int index = i;
            GameObject buttonGO = Instantiate(mapButtonPrefab, mapButtonsContainer);
            MapButton mapButton = buttonGO.GetComponent<MapButton>();
            mapButtons.Add(mapButton);            
            mapButton.Setup(
                maps[i].name, 
                () => OnMapButtonPressed(index), 
                maps[i].icon
            ); 
        }

        // Start at passthrough map
        EnableMap(0);
    }

    private void OnMapButtonPressed(int index) {
        EnableMap(index);
    }

    private void EnableMap(int index) {
        // Disable previous
        if (CurrentActiveMap != null) {
            CurrentActiveMap.SetActive(false);
            UIStyler.ApplyStyle(currentActiveMapButton.button, false);
        }

        // Enable selected map
        CurrentActiveMap = maps[index].mapObject;
        CurrentActiveMap.SetActive(true);
        currentActiveMapButton = mapButtons[index];

        // Apply style of enabled map
        UIStyler.ApplyStyle(mapButtons[index].button, true);
        UIStyler.ChangeTheme(maps[index].themeColor);

        // Set camera background type based on map name
        Camera mainCam = Camera.main;
        if (mainCam != null) {
            if (maps[index].name == "Passthrough") {
                mainCam.clearFlags = CameraClearFlags.SolidColor;
            } else {
                mainCam.clearFlags = CameraClearFlags.Skybox;
            }
        }

        string CleanText(string input) => input.EndsWith("_") ? input.Substring(0, input.Length - 1) : input; // The text might still end with a "_" before starting the next animation, which makes a very long "____etc" otherwise if you rapidly swap maps

        // Title & Description
        StopAllCoroutines();    
        mapInfoTitle.text = mapInfoTitle.text.Replace("_", "");
        mapInfoDescription.text = mapInfoDescription.text.Replace("_", "");
        StartCoroutine(AnimateTextTransition(mapInfoTitle, CleanText(mapInfoTitle.text), maps[index].name, 0.03f, false));
        StartCoroutine(AnimateTextTransition(mapInfoDescription, CleanText(mapInfoDescription.text), maps[index].description, 0.006f, true));
    }

    IEnumerator AnimateTextTransition(TextMeshProUGUI textElement, string from, string to, float cooldown, bool usePaddingOnText) {

        int maxLength = Mathf.Max(from.Length, to.Length);
        List<char> animated = new List<char>(from.PadRight(maxLength));
        string target = to.PadRight(maxLength);

        for (int i = 0; i < maxLength; i++) {
            if (!usePaddingOnText) { // This will make it so the text always wraps to the next line if the word will be too long.
                if (i >= to.Length) {
                    // Clear extra characters (when going from a longer word to a shorter one)
                    animated.RemoveAt(i);
                    maxLength--;
                    i--;
                } else {
                    animated[i] = to[i];
                }
            } else {
                animated[i] = target[i];
            }

            string animatedText = new string(animated.ToArray());
            if (i + 1 <= animatedText.Length)
                animatedText = animatedText.Insert(i + 1, "_");
            else
                animatedText += "_";

            textElement.text = animatedText;
            yield return new WaitForSeconds(cooldown);
        }

        textElement.text = to;
    }
}