
#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using UnityEditor.Animations;
using System.IO;

public class AnimatorCreatorTool
{
    [MenuItem("Tools/ARENA/Create Base AnimatorControllers")]
    public static void CreateControllers()
    {
        string folder = "Assets/Animators";
        if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);

        CreateController("Kael_Controller"); 
        CreateController("Lyra_Controller"); 
        CreateController("Enemy_Controller");

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        EditorUtility.DisplayDialog("Animators", "Base AnimatorControllers created in Assets/Animators. Assign your Mixamo clips to states.", "OK");
    }

    static void CreateController(string name)
    {
        string path = $"Assets/Animators/{name}.controller";
        if (File.Exists(path)) return;

        var controller = AnimatorController.CreateAnimatorControllerAtPath(path);
        var rootStateMachine = controller.layers[0].stateMachine;

        // Create states
        var idle = rootStateMachine.AddState("Idle");
        var run = rootStateMachine.AddState("Run");
        var attack = rootStateMachine.AddState("Attack");
        var die = rootStateMachine.AddState("Die");

        // Parameters
        controller.AddParameter("Speed", AnimatorControllerParameterType.Float);
        controller.AddParameter("Attack", AnimatorControllerParameterType.Trigger);
        controller.AddParameter("Die", AnimatorControllerParameterType.Trigger);

        // Default transition Idle -> Run by Speed
        var trans = idle.AddTransition(run);
        trans.hasExitTime = false;
        trans.AddCondition(AnimatorConditionMode.Greater, 0.1f, "Speed");
        var transBack = run.AddTransition(idle);
        transBack.hasExitTime = false;
        transBack.AddCondition(AnimatorConditionMode.LessOrEqual, 0.1f, "Speed");

        // AnyState to Attack and Die
        var anyState = rootStateMachine.anyStatePosition;
        rootStateMachine.AddAnyStateTransition(attack).AddCondition(AnimatorConditionMode.If, 0, "Attack");
        rootStateMachine.AddAnyStateTransition(die).AddCondition(AnimatorConditionMode.If, 0, "Die");
    }
}
#endif
