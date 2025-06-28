using UnityEngine;
using UnityEditor;

public class RaiseToTPoseGenerator : MonoBehaviour
{
    //[MenuItem("Tools/Generate RaiseToTPose Animation")]
    //public static void GenerateAnimation()
    //{
    //    GameObject selected = Selection.activeGameObject;

    //    if (selected == null || selected.GetComponent<Animator>() == null)
    //    {
    //        Debug.LogError("Select a GameObject with an Animator.");
    //        return;
    //    }

    //    AnimationClip clip = new AnimationClip();
    //    clip.name = "RaiseToTPose";

    //    // Arm bone paths – adjust these if your model uses different names
    //    string leftArmPath = "BoneRoot/Hip/Pelvis/Waist/Spine01/Spine02/L_Clavicle/L_Upperarm";
    //    string rightArmPath = "BoneRoot/Hip/Pelvis/Waist/Spine01/Spine02/R_Clavicle/R_Upperarm";

    //    // Rotate arms down at frame 0, up at frame 30
    //    AddArmKeyframes(clip, leftArmPath, true);
    //    AddArmKeyframes(clip, rightArmPath, false);

    //    AssetDatabase.CreateAsset(clip, "Assets/Animations/RaiseToTPose.anim");
    //    AssetDatabase.SaveAssets();

    //    Debug.Log("RaiseToTPose animation created at Assets/Animations/");
    //}

    //static void AddArmKeyframes(AnimationClip clip, string bonePath, bool isLeft)
    //{
    //    // Z-rotation from arms down to T-pose
    //    float startZ = 0f;
    //    float endZ = isLeft ? 90f : -90f;

    //    AnimationCurve curveX = AnimationCurve.Constant(0f, 1f, 0f);
    //    AnimationCurve curveY = AnimationCurve.Constant(0f, 1f, 0f);
    //    AnimationCurve curveZ = AnimationCurve.Linear(0f, startZ, 1f, endZ);

    //    clip.SetCurve(bonePath, typeof(Transform), "localEulerAnglesRaw.x", curveX);
    //    clip.SetCurve(bonePath, typeof(Transform), "localEulerAnglesRaw.y", curveY);
    //    clip.SetCurve(bonePath, typeof(Transform), "localEulerAnglesRaw.z", curveZ);
    //}
}

