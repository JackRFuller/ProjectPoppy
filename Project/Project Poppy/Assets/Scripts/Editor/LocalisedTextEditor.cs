using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class LocalisedTextEditor : EditorWindow
{
    public LocalisationData localisationData;

    [MenuItem("Window/Localisation Text Editor")]
    private static void Init()
    {
        EditorWindow.GetWindow(typeof(LocalisedTextEditor)).Show();
    }

    private void OnGUI()
    {
        if(localisationData != null)
        {
            SerializedObject serializedObject = new SerializedObject(this);
            SerializedProperty serializedProperty = serializedObject.FindProperty("localisationData");

            EditorGUILayout.PropertyField(serializedProperty, true);
            serializedObject.ApplyModifiedProperties();

            if(GUILayout.Button("Save data"))
            {
                SaveGameData();
            }
        }

        if (GUILayout.Button("Load Data"))
        {
            LoadGameData();
        }

        if(GUILayout.Button("Create New Data"))
        {
            CreateNewData();
        }
    }

    private void LoadGameData()
    {
        string filePath = EditorUtility.OpenFilePanel("Select localisation data file", Application.streamingAssetsPath, "json");

        if(!string .IsNullOrEmpty(filePath))
        {
            string dataAsJson = File.ReadAllText(filePath);

            localisationData = JsonUtility.FromJson<LocalisationData>(dataAsJson);
        }
    }

    private void SaveGameData()
    {
        string filePath = EditorUtility.SaveFilePanel("Save localisation data file", Application.streamingAssetsPath, "", "json");

        if(!string.IsNullOrEmpty(filePath))
        {
            if(!string.IsNullOrEmpty(filePath))
            {
                string dataAsJson = JsonUtility.ToJson(localisationData);
                File.WriteAllText(filePath, dataAsJson);
            }
        }

    }

    private void CreateNewData()
    {
        localisationData = new LocalisationData();

    }
    
}
