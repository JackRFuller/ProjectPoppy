using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Interactable : MonoBehaviour
{
    [SerializeField]
    protected GameObject interactPromptObj;

    protected virtual void Start()
    {
        interactPromptObj.SetActive(false);
    }

    public virtual void OnPlayerInteract()
    {

    }

    protected virtual void ShowInteractPrompt()
    {
        interactPromptObj.SetActive(true);
    }

    protected virtual void HideInteractPrompt()
    {
        interactPromptObj.SetActive(false);
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        ShowInteractPrompt();
    }

    protected virtual void OnTriggerExit2D(Collider2D collision)
    {
        HideInteractPrompt();
    }


}
