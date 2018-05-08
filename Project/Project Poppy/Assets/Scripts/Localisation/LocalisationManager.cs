using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class LocalisationManager : MonoBehaviour
{
    public static LocalisationManager instance;

    private Dictionary<string, string> localisedText;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if (Application.systemLanguage == SystemLanguage.English)
        {
            LoadLocalisedText("English.json");
        }        
    }

    public void LoadLocalisedText(string filename)
    {
        localisedText = new Dictionary<string, string>();
        string filePath = Path.Combine(Application.streamingAssetsPath, filename);

        if (File.Exists(filePath))
        {
            string dataAsJson = File.ReadAllText(filePath);
            LocalisationData loadedData = JsonUtility.FromJson<LocalisationData>(dataAsJson);

            for (int i = 0; i < loadedData.items.Length; i++)
            {
                localisedText.Add(loadedData.items[i].keys.ToString(), loadedData.items[i].value);
            }

            Debug.Log(filename + " Data Loaded");
        }
        else
        {
            Debug.LogError("Cannot Find File");
        }
    }

    public string GetLocalisedValue(string key)
    {
        string result = "Missing Text";
        if(localisedText.ContainsKey(key))
        {
            result = localisedText[key];
        }

        return result;
    }   
}
