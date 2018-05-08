using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueUIController : MonoBehaviour
{
    [SerializeField]
    private TMP_Text dialogueText;
    private char[] dialogueTextSplit;
    private char[] dialogueTextToPrintSplit;

    private bool hasFinishedPrintingText;
    private const float textHidingCooldown = 1.0f;
    private float textTimeStamp;

    private void Start()
    {
        dialogueText.enabled = false;
    }

    public void ShowText(string text)
    {  
        dialogueTextSplit = text.ToCharArray();
        dialogueTextToPrintSplit = new char[dialogueTextSplit.Length];       
        dialogueText.enabled = true;

        StartCoroutine(PrintDialogue());
    }

    IEnumerator PrintDialogue()
    {
        hasFinishedPrintingText = false;

        for (int i = 0; i < dialogueTextSplit.Length; i++)
        {
            dialogueTextToPrintSplit[i] = dialogueTextSplit[i];
            string text = new string(dialogueTextToPrintSplit);
            dialogueText.text = text;
            yield return new WaitForSeconds(0.05f);
        }

        textTimeStamp = Time.time + textHidingCooldown;
        hasFinishedPrintingText = true;

    }

    private void Update()
    {
        if(textTimeStamp <= Time.time && hasFinishedPrintingText)
        {
            HideDialogue();
        }
    }

    private void HideDialogue()
    {
       dialogueText.enabled = false;
       hasFinishedPrintingText = false;
    }
}
