using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalisedText : MonoBehaviour
{
    [SerializeField]
    private LocalisationKeys.StringKeys stringKey;
    private string text;


    private void Start()
    {
        string textKey = stringKey.ToString();
        text = LocalisationManager.instance.GetLocalisedValue(textKey);
    }



}
