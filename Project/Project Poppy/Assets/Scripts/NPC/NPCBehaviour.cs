using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCBehaviour : Interactable
{
    private UIManager uiMananger;

    [SerializeField]
    private LocalisationKeys.StringKeys[] stringKeys;
    private int stringIndex = 0;

    private PlayerInputController playerInput;

    protected override void Start()
    {
        uiMananger = this.transform.root.GetComponent<UIManager>();
        base.Start();
    }

    public override void OnPlayerInteract()
    {
        string textKey = stringKeys[stringIndex].ToString();
        uiMananger.ShowDialogueText(textKey);

        stringIndex++;
        if(stringIndex == stringKeys.Length)
        {
            stringIndex = 0;

            HideInteractPrompt();

        }

    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Character")
        {
            if (playerInput == null)
                playerInput = collision.GetComponent<PlayerInputController>();

            playerInput.SetCurrentInteractable(this);

            base.OnTriggerEnter2D(collision);
        }
    }
}
