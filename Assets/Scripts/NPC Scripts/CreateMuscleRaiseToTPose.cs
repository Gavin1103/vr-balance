using UnityEngine;
using UnityEditor;

public class CreateMuscleRaiseToTPose : MonoBehaviour
{
    [MenuItem("Tools/Generate Humanoid RaiseToTPose")]
    public static void GenerateHumanoidTPose()
    {
        GameObject selected = Selection.activeGameObject;
        if (selected == null || selected.GetComponent<Animator>() == null)
        {
            Debug.LogError("Select a GameObject with an Animator.");
            return;
        }

        Animator animator = selected.GetComponent<Animator>();
        if (!animator.isHuman || animator.avatar == null)
        {
            Debug.LogError("Selected object must have a Humanoid rig with a valid avatar.");
            return;
        }

        AnimationClip clip = new AnimationClip();
        clip.name = "RaiseToTPose_Muscle";

        // Create a 1-second clip (60 frames at 60 fps)
        //float startTime = 0f;
        //float endTime = 1f;

        // Set muscle values: Upper arms start down (Z = -60) and raise to Z = ±90
        AddMuscleCurve(clip, "LeftUpperArm.Stretched", 0f, -60f, 1f, 90f);
        AddMuscleCurve(clip, "RightUpperArm.Stretched", 0f, -60f, 1f, -90f);


        clip.legacy = false;
        clip.EnsureQuaternionContinuity();

        string folderPath = "Assets/Animations";
        if (!AssetDatabase.IsValidFolder(folderPath))
            AssetDatabase.CreateFolder("Assets", "Animations");

        AssetDatabase.CreateAsset(clip, folderPath + "/RaiseToTPose_Muscle.anim");
        AssetDatabase.SaveAssets();

        Debug.Log("Humanoid RaiseToTPose_Muscle animation created!");
    }

    private static void AddMuscleCurve(AnimationClip clip, string muscleProperty, float time0, float value0, float time1, float value1)
    {
        var binding = EditorCurveBinding.FloatCurve("", typeof(Animator), muscleProperty);
        var curve = new AnimationCurve(
            new Keyframe(time0, value0),
            new Keyframe(time1, value1)
        );
        AnimationUtility.SetEditorCurve(clip, binding, curve);
    }


    private static int MuscleIndex(HumanBodyBones bone)
    {
        // Maps upper arms to their main twist muscle index
        return bone switch
        {
            HumanBodyBones.LeftUpperArm => 55,  // Shoulder Twist Left
            HumanBodyBones.RightUpperArm => 56, // Shoulder Twist Right
            _ => throw new System.Exception("Unsupported bone for this tool."),
        };
    }
}
