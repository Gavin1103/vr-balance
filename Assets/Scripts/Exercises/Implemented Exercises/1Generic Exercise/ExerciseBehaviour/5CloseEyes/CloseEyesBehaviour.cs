using System.Collections;
using UnityEngine;

public class CloseEyesBehaviour : IMovementBehaviour {
    private float fadeDuration = 1.5f;
    private Coroutine fadeCoroutine;

    public CloseEyesBehaviour() { 
    }

    public override void OnMovementStart(ExerciseMovement movement) {
        var sphere = GenericExerciseReferences.Instance.EyesClosedSphere;
        sphere.SetActive(true);
        MapManager.Instance.CurrentActiveMap.SetActive(false);

        // Start fading in the sphere material
        var mono = GenericExerciseReferences.Instance as MonoBehaviour;
        if (mono != null) {
            var renderer = sphere.GetComponent<Renderer>();
            if (renderer != null) {
                if (fadeCoroutine != null) mono.StopCoroutine(fadeCoroutine);
                fadeCoroutine = mono.StartCoroutine(FadeMaterialAlpha(renderer.material, 0f, 1f, fadeDuration));
            }
        }
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
                fadeCoroutine = mono.StartCoroutine(FadeOutAndDisable(sphere, renderer.material, 1f, 0f, fadeDuration));
            }
        }
        MapManager.Instance.CurrentActiveMap.SetActive(true);
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

    private IEnumerator FadeOutAndDisable(GameObject obj, Material mat, float from, float to, float duration) {
        yield return FadeMaterialAlpha(mat, from, to, duration);
        obj.SetActive(false);
    }
}