using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PlatformController))]
public class PlatformControllerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        PlatformController controllerScript = (PlatformController) target;

        string activationButtonName = null;
        activationButtonName = !controllerScript.CanActivate ? "Add Activation Component" : "Remove Activation Component";
        if (GUILayout.Button(activationButtonName))
        {
            if (!controllerScript.CanActivate)
            {
                controllerScript.AddOrRemoveBehaviours(0,true);
            }
            else
            {
                controllerScript.AddOrRemoveBehaviours(0, false);
            }
        }

        string rotatingButtonName = null;
        rotatingButtonName = !controllerScript.CanRotate ? "Add Rotating Component" : "Remove Rotating Component";
        if (GUILayout.Button(rotatingButtonName))
        {
            if (!controllerScript.CanRotate)
            {
                controllerScript.AddOrRemoveBehaviours(1, true);
            }
            else
            {
                controllerScript.AddOrRemoveBehaviours(1, false);
            }
        }

        string movingButtonName = null;
        movingButtonName = !controllerScript.CanMove ? "Add Moving Component" : "Remove Moving Component";
        if (GUILayout.Button(movingButtonName))
        {
            if (!controllerScript.CanMove)
            {
                controllerScript.AddOrRemoveBehaviours(2, true);
            }
            else
            {
                controllerScript.AddOrRemoveBehaviours(2, false);
            }
        }

        if(GUI.changed)
            EditorUtility.SetDirty(controllerScript);
    }
}
