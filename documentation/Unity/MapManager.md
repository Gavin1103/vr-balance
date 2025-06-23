# 1 | Map System

The map system handles switching between different map environments in the application. Each map has a name, description, icon, a 3D GameObject in the scene, a theme color, and optionally a skybox material. Only one map is active at a time. When a new map is selected, the currently active one is disabled, and the UI updates accordingly.

---

## 1.1 | `MapEntry`

Each map is defined by the `MapEntry` class:

```csharp
[System.Serializable]
public class MapEntry {
    public string name;
    public string description;
    public GameObject mapObject;
    public Sprite icon;
    public Color themeColor;
    public TMP_FontAsset subtitleFont;
    public TMP_FontAsset uiFont;
    public Material skyboxMaterial;
}
```

---

## 1.2 | Workflow Overview

- All maps are defined in a list.
- A button is generated for each map in the UI.
- When a button is clicked, the selected map is activated, and the previous one is deactivated.
- The theme color and font are updated across the interface.
- Skybox or solid background is set depending on the map.

---

## 1.3 | How it works

In `Start()`, buttons for each map are created automatically:

```csharp
for (int i = 0; i < maps.Count; i++) {
    int index = i;
    GameObject buttonGO = Instantiate(mapButtonPrefab, mapButtonsContainer);
    MapButton mapButton = buttonGO.GetComponent<MapButton>();
    mapButton.Setup(maps[i].name, () => OnMapButtonPressed(index), maps[i].icon);
}
```

When a button is clicked, `EnableMap(index)` is called to:

- Disable the previous map
- Enable the new one
- Apply UI style and font
- Update the camera background

---

## 1.4 | UI and Camera Handling

- If the map has no `skyboxMaterial`, the camera background switches to a solid color (for passthrough).
- Font updates are applied globally using `FindObjectsByType`.
- The UI theme color is updated with a helper method (`UIStyler.ChangeTheme()`).

---

## 1.5 | Title and Description Animation

Map title and description are animated using a coroutine that gradually transitions text:

```csharp
StartCoroutine(AnimateTextTransition(mapInfoTitle, currentTitle, newTitle, 0.015f, false));
```

This creates a smooth typing effect, including a trailing underscore `_` to mimic typing feedback.