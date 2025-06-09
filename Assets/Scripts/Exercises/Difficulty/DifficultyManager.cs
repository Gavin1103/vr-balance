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

    public void ShowDropdown(bool show) {
        dropdownMenu.SetActive(show);
    }

    public void OnDropdownChanged() {
        int index = dropdownText.value;
        switch (index) {
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
    }

    public void SetAdvisedDifficulty(int index, Difficulty diff) {
        dropdownText.value = index;
        AdvisedDifficulty = diff;
        SelectedDifficulty = diff;
    }
}
