using UnityEngine;
using TMPro;

public class FeedbackText : MonoBehaviour {
    public float floatUpSpeed = 0.5f;
    public float fadeDuration = 1f;

    private float timer = 0f;
    private CanvasGroup canvasGroup;
    private Vector3 moveDirection = Vector3.up;

    void Start() {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    void Update() {
        transform.position += moveDirection * floatUpSpeed * Time.deltaTime;

        timer += Time.deltaTime;
        canvasGroup.alpha = 1f - (timer / fadeDuration);

        if (timer >= fadeDuration) {
            Destroy(gameObject);
        }
    }

    public void Setup(string text, Color color) {
        GetComponentInChildren<TextMeshProUGUI>().text = text;
        GetComponentInChildren<TextMeshProUGUI>().color = color;
    }
}