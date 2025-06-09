using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DifficultyManager : MonoBehaviour {
    public static DifficultyManager Instance;

    public Difficulty SelectedDifficulty;
    [HideInInspector] public Difficulty AdvisedDifficulty = Difficulty.None;

    [SerializeField] private GameObject dropdownMenu;
    [SerializeField] private TMP_Dropdown dropdownText;

    private void Awake() {
        Instance = this;
    }

    public void ShowDropdown(bool show)
    {
        dropdownMenu.SetActive(show);
    }

    public void SetAdvisedDifficulty(int index, Difficulty diff)
    {
        dropdownText.value = index;
        AdvisedDifficulty = diff;
        SelectedDifficulty = diff;
        UpdateDropdownOptions();
    }

    public void OnDropdownChanged()
    {
        int index = dropdownText.value;
        switch (index)
        {
            case 0:
                SelectedDifficulty = Difficulty.Easy;
                break;
            case 1:
                SelectedDifficulty = Difficulty.Medium;
                break;
            case 2:
                SelectedDifficulty = Difficulty.Hard;
                break;
            default:
                Debug.LogWarning("Invalid dropdown index.");
                break;
        }

        UpdateDropdownWarning();
        UpdateDropdownOptions();
    }

    private void UpdateDropdownWarning() {
        // Only show warning if a harder difficulty is selected than advised
        if (IsHarderThanAdvised(SelectedDifficulty, AdvisedDifficulty)) {
            dropdownText.captionText.text = $"{SelectedDifficulty} (not advised)";
        } else {
            // Reset to normal label
            dropdownText.captionText.text = SelectedDifficulty.ToString();
        }
    }
    private void UpdateDropdownOptions() {
        for (int i = 0; i < dropdownText.options.Count; i++) {
            Difficulty diff = (Difficulty)i + 1; // Assuming 0 is None, so start from 1
            string label = diff.ToString();

            if (IsHarderThanAdvised(diff, AdvisedDifficulty)) {
                label += " (not advised)";
            }

            dropdownText.options[i].text = label;
        }

        // Refresh the label and the dropdown UI
        dropdownText.RefreshShownValue();
    }


    // Helper to compare difficulties
    private bool IsHarderThanAdvised(Difficulty selected, Difficulty advised)
    {
        return (int)selected > (int)advised && advised != Difficulty.None;
    }
}
