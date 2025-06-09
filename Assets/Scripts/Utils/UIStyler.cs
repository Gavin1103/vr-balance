using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public static class UIStyler {
    public static Color SelectedColor = Color.magenta;
    public static readonly Color DisabledColor = new Color(74f / 255f, 20f / 255f, 20f / 255f);

    private static readonly List<(Selectable selectable, bool isSelected)> styledSelectables = new(); // Selectables are buttons, toggles, etc.
    private static readonly List<Graphic> styledGraphics = new(); // Graphics are images, text, etc.

    public static void ApplyStyle(Selectable selectable, bool isSelected, bool authorityToOverride = true) {
        if (selectable == null) return;

        int index = styledSelectables.FindIndex(pair => pair.selectable == selectable);
        if (index >= 0)
            if (authorityToOverride)
                styledSelectables[index] = (selectable, isSelected);
            else
                isSelected = styledSelectables[index].isSelected;
        else
            styledSelectables.Add((selectable, isSelected));

        ApplySelectableColors(selectable, isSelected);
    }

    public static void RegisterGraphic(Graphic graphic) {
        if (graphic == null || styledGraphics.Contains(graphic)) return;
        styledGraphics.Add(graphic);
        graphic.color = SelectedColor;
    }

    public static void ChangeTheme(Color color) {
        SelectedColor = color;

        foreach (var (selectable, isSelected) in styledSelectables)
            if (selectable != null)
                ApplySelectableColors(selectable, isSelected);

        foreach (var graphic in styledGraphics)
            if (graphic != null)
                graphic.color = SelectedColor;
    }

    private static void ApplySelectableColors(Selectable selectable, bool isSelected) {
        Color baseColor = isSelected ? SelectedColor : GetLighter(SelectedColor, 0.05f);

        var cb = selectable.colors;
        cb.normalColor = isSelected ? SelectedColor : AddBrightness(baseColor, 0.2f);
        cb.highlightedColor = AddBrightness(cb.normalColor, 0.15f);
        cb.pressedColor = isSelected ? GetDarker(SelectedColor, 0.3f) : GetDarker(baseColor, 0.2f);
        cb.selectedColor = SelectedColor;
        cb.disabledColor = DisabledColor;
        selectable.colors = cb;

        ApplyFontStyleToSelectable(selectable, isSelected);
    }

    private static void ApplyFontStyleToSelectable(Selectable selectable, bool isSelected) {
        TMP_Text tmpText = selectable.GetComponentInChildren<TMP_Text>();
        if (tmpText != null)
            tmpText.fontStyle = isSelected ? FontStyles.Bold : FontStyles.Normal;

        Text legacyText = selectable.GetComponentInChildren<Text>();
        if (legacyText != null)
            legacyText.fontStyle = isSelected ? FontStyle.Bold : FontStyle.Normal;
    }

    // Utilities:
    public static Color AddBrightness(Color color, float amount) => new(
        Mathf.Clamp01(color.r + amount),
        Mathf.Clamp01(color.g + amount),
        Mathf.Clamp01(color.b + amount),
        color.a
    );

    public static Color GetLighter(Color color, float factor) => Color.Lerp(color, Color.white, factor);
    public static Color GetDarker(Color color, float factor) => Color.Lerp(color, Color.black, factor);
}
