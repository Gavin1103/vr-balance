using System.Collections;
using UnityEngine;
using TMPro;

public class CloseEyesBehaviour : IMovementBehaviour {
    private float fadeInDuration = 2f;
    private float fadeOutDuration = 0.75f;

    private GameObject sphere;
    private Coroutine fadeCoroutine;

    public TextMeshProUGUI text;
    private Coroutine textFadeCoroutine;

    public CloseEyesBehaviour() { 
        sphere = GenericExerciseReferences.Instance.EyesClosedSphere;
        text = sphere.GetComponentInChildren<TMPro.TextMeshProUGUI>();
        var mat = text.fontMaterial;
        mat.SetFloat("_Surface", 1);
    }

    public override void OnMovementStart(ExerciseMovement movement) {
        sphere.SetActive(true);

        // Start fading in the sphere material
        var mono = GenericExerciseReferences.Instance as MonoBehaviour;
        if (mono != null) {
            var renderer = sphere.GetComponent<Renderer>();
            if (renderer != null) {
                SetMaterialTransparent(renderer.material);

                if (fadeCoroutine != null) mono.StopCoroutine(fadeCoroutine);
                fadeCoroutine = mono.StartCoroutine(FadeMaterialAlpha(renderer.material, 0f, 1f, fadeInDuration));
            }
        }
        FadeIn();
    }
    public override IEnumerator OnMovementUpdate(ExerciseMovement movement) {
        yield return null;
    }
    public override void OnMovementEnd(ExerciseMovement movement) {
        var sphere = GenericExerciseReferences.Instance.EyesClosedSphere;
        var mono = GenericExerciseReferences.Instance as MonoBehaviour;
        if (mono != null) {
            var renderer = sphere.GetComponent<Renderer>();
            if (renderer != null) {
                if (fadeCoroutine != null) mono.StopCoroutine(fadeCoroutine);
                fadeCoroutine = mono.StartCoroutine(FadeOutAndDisable(sphere, renderer.material, 1f, 0f, fadeOutDuration));
            }
        }
        FadeOut();
    }

    private IEnumerator FadeMaterialAlpha(Material mat, float from, float to, float duration) {
        float elapsed = 0f;
        Color color = mat.color;
        color.a = from;
        mat.color = color;
        while (elapsed < duration) {
            elapsed += Time.deltaTime;
            color.a = Mathf.Lerp(from, to, elapsed / duration);
            mat.color = color;
            yield return null;
        }
        color.a = to;
        mat.color = color;
    }
    
    private void SetMaterialTransparent(Material mat) {
        mat.SetFloat("_Mode", 3); // Transparent
        mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        mat.SetInt("_ZWrite", 0); // ZWrite Off
        mat.DisableKeyword("_ALPHATEST_ON");
        mat.EnableKeyword("_ALPHABLEND_ON");
        mat.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        mat.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;
    }

    private IEnumerator FadeOutAndDisable(GameObject obj, Material mat, float from, float to, float duration)
    {
        yield return FadeMaterialAlpha(mat, from, to, duration);
        obj.SetActive(false);
    }
    
    public void FadeIn() {
        if (textFadeCoroutine != null)
            ExerciseManager.Instance.StopCoroutine(textFadeCoroutine);
        textFadeCoroutine = ExerciseManager.Instance.StartCoroutine(FadeTextAlpha(0f, 1f, fadeInDuration));
    }

    public void FadeOut() {
        if (textFadeCoroutine != null)
            ExerciseManager.Instance.StopCoroutine(textFadeCoroutine);
        textFadeCoroutine = ExerciseManager.Instance.StartCoroutine(FadeTextAlpha(1f, 0f, fadeOutDuration));
    }

    private IEnumerator FadeTextAlpha(float fromAlpha, float toAlpha, float fadeDuration) {
        float elapsed = 0f;
        while (elapsed < fadeDuration) {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(fromAlpha, toAlpha, elapsed / fadeDuration);

            var c = text.faceColor;
            text.faceColor = new Color32(c.r, c.g, c.b, (byte)(alpha * 255));
            yield return null;
        }
        Color finalColor = text.color;
        text.color = new Color(finalColor.r, finalColor.g, finalColor.b, toAlpha);
    }
}