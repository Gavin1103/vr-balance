// using System.Collections;
// using UnityEngine;

// public class AffordancePulse : MonoBehaviour
// {
//     public float pulseDuration = 0.5f;
//     public float pulseScale = 10f;

//     private Vector3 originalScale;
//     private bool pulsing = true;
//     private float originalOpacity;
//     private Material mat;

//     void Awake()
//     {
//         originalScale = transform.localScale;
//         mat = GetComponent<Renderer>().material;
//         originalOpacity = mat.color.a;
//     }

//     void OnEnable()
//     {
//         StartCoroutine(PulseLoop());
//     }

//     private IEnumerator PulseLoop()
//     {
//         while (pulsing)
//         {
//             yield return ScaleAndFade(originalScale * pulseScale, 0f, pulseDuration);
//             transform.localScale = originalScale;
//             SetAlpha(originalOpacity);
//         }
//     }

//     private IEnumerator ScaleAndFade(Vector3 targetScale, float targetAlpha, float duration)
//     {
//         Vector3 startScale = transform.localScale;
//         float startAlpha = mat.color.a;
//         float t = 0f;

//         while (t < duration)
//         {
//             float lerpT = t / duration;
//             transform.localScale = Vector3.Lerp(startScale, targetScale, lerpT);
//             SetAlpha(Mathf.Lerp(startAlpha, targetAlpha, lerpT));
//             t += Time.deltaTime;
//             yield return null;
//         }

//         transform.localScale = targetScale;
//         SetAlpha(targetAlpha);
//     }

//     private void SetAlpha(float a)
//     {
//         Color c = mat.color;
//         c.a = a;
//         mat.color = c;
//     }
    
//     public void SetPulseScale(float percentage, float minScale = 1f, float maxScale = 10f) {
//         percentage = Mathf.Clamp(percentage, 0f, 100f);
//         pulseScale = Mathf.Lerp(maxScale, minScale, percentage / 100f);
//     }
// }
