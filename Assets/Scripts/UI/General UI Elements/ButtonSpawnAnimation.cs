using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ButtonSpawnAnimation : MonoBehaviour {
    [SerializeField] private float spawnTime = 0.25f;
    private float duration = 0.5f;
    private float zOffset = -50f;

    private Vector3 originalLocalPosition;
    private Vector3 offsettedLocalPosition;
    private CanvasGroup canvasGroup;
    private Button button;

    void Awake() {
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
            canvasGroup = gameObject.AddComponent<CanvasGroup>();

        button = GetComponent<Button>();
        if (button != null)
            button.interactable = false;
    }

    void Start() {
        originalLocalPosition = transform.localPosition;
        transform.localPosition = originalLocalPosition + new Vector3(0, 0, zOffset);
        offsettedLocalPosition = transform.localPosition;

        canvasGroup.alpha = 0f;
        StartCoroutine(SpawnAnimation());
    }

    IEnumerator SpawnAnimation() {
        yield return new WaitForSeconds(spawnTime);

        if (button != null)
            button.interactable = true;

        SoundManager.soundInstance.PlaySFX("Button Hover");
        float elapsed = 0f;

        while (elapsed < duration) {
            float t = elapsed / duration;
            transform.localPosition = Vector3.Lerp(offsettedLocalPosition, originalLocalPosition, t);
            canvasGroup.alpha = Mathf.Lerp(0f, 1f, t);
            elapsed += Time.deltaTime;
                        LayoutRebuilder.ForceRebuildLayoutImmediate(transform.parent.GetComponent<RectTransform>());

            yield return null;
        }

        // transform.localPosition = originalLocalPosition;
        canvasGroup.alpha = 1f;
    }
}