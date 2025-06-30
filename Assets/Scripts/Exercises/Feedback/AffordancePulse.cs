using System.Collections;
using UnityEngine;

public class AffordancePulse : MonoBehaviour
{
    public float pulseDuration = 0.5f;
    public float pulseScale = 10f;
    public Vector3 axisMask = new Vector3(1, 1, 1); // (1, 1, 1) = all axes pulse, (1, 0, 0) = X only, etc.

    private Vector3 originalScale;
    private bool pulsing = true;
    private float originalOpacity;
    private Material mat;

    void Awake()
    {
        originalScale = transform.localScale;
        mat = GetComponent<Renderer>().material;
        originalOpacity = mat.color.a;

        mat.SetFloat("_Mode", 3); // 3 = Transparent in Standard shader
        mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        mat.SetInt("_ZWrite", 0);
        mat.DisableKeyword("_ALPHATEST_ON");
        mat.EnableKeyword("_ALPHABLEND_ON");
        mat.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        mat.renderQueue = 3000;

    }

    void OnEnable()
    {
        StartCoroutine(PulseLoop());
    }

    private IEnumerator PulseLoop()
    {
        while (pulsing)
        {
            yield return ScaleAndFade(originalScale * pulseScale, 0f, pulseDuration);
            transform.localScale = originalScale;
            SetAlpha(originalOpacity);
        }
    }

    private IEnumerator ScaleAndFade(Vector3 targetScale, float targetAlpha, float duration)
    {
        Vector3 startScale = transform.localScale;
        float startAlpha = mat.color.a;
        float t = 0f;

        while (t < duration)
        {
            float lerpT = t / duration;
            Vector3 lerped = Vector3.Lerp(startScale, targetScale, lerpT);
            transform.localScale = Vector3.Scale(lerped, axisMask) + Vector3.Scale(originalScale, Vector3.one - axisMask);

            SetAlpha(Mathf.Lerp(startAlpha, targetAlpha, lerpT));
            t += Time.deltaTime;
            yield return null;
        }

        transform.localScale = targetScale;
        SetAlpha(targetAlpha);
    }

    private void SetAlpha(float a)
    {
        Color c = mat.color;
        c.a = a;
        mat.color = c;
    }
    
    public void SetPulseScale(float percentage, float minScale = 1f, float maxScale = 10f) {
        percentage = Mathf.Clamp(percentage, 0f, 100f);
        pulseScale = Mathf.Lerp(maxScale, minScale, percentage / 100f);
    }
}
