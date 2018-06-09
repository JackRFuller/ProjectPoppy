using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Canvas mainCanvas;
    public Canvas MainCanvas {get {return mainCanvas;}}

    [SerializeField]
    private DialogueUIController dialogueController;

    public void ShowDialogueText(string key)
    {
        string text = LocalisationManager.instance.GetLocalisedValue(key);
        dialogueController.ShowText(text);
    }   
	
}
