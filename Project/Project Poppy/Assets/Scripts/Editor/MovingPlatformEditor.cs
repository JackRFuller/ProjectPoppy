using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MovingPlatformBehaviour))]
[CanEditMultipleObjects]
public class MovingPlatformEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        DrawDefaultInspector();

        MovingPlatformBehaviour platformScript = (MovingPlatformBehaviour)target;

        if (GUILayout.Button("Set Point A"))
        {
            platformScript.SetPointA();
        }

        if (GUILayout.Button("Set Point B"))
        {
            platformScript.SetPointB();
        }

        if (GUILayout.Button("Reset to Point A"))
        {
            platformScript.ResetToStartPosition();
        }

        if (GUILayout.Button("Create Platform Path"))
        {
            platformScript.CreatePlatformPath();
        }

        if (GUILayout.Button("Set Platform Path"))
        {
            platformScript.SetPlatformPath();
        }
        serializedObject.ApplyModifiedProperties();
    }
}
