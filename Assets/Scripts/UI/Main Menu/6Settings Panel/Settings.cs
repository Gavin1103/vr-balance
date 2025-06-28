using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.XR.OpenXR;
//using UnityEngine.XR.OpenXR.Features.Meta;

public class SettingsMenu : MonoBehaviour {
    // Account settings
    // Sound settings
    // Auto-detect or set user height to align UI/poses
    // Subtitles
    // Instruction speed
    // Boundary warning:
    //public void SuppressBoundary() {
    //    var feature = OpenXRSettings.Instance.GetFeature<BoundaryVisibilityFeature>();

    //    if (feature != null) {
    //        var result = feature.TryRequestBoundaryVisibility(
    //            XrBoundaryVisibility.VisibilitySuppressed
    //        );

    //        Debug.Log("Boundary suppression result: " + result);

    //        if ((int)result == BoundaryVisibilityFeature.XR_BOUNDARY_VISIBILITY_SUPPRESSION_NOT_ALLOWED_META) {
    //            Debug.LogWarning("Suppression not allowed. You must be rendering passthrough for this to work.");
    //        }
    //    } else {
    //        Debug.LogWarning("BoundaryVisibilityFeature not found.");
    //    }
    //}
}