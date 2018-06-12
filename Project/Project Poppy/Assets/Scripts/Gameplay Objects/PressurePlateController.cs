using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PressurePlateController : MonoBehaviour
{
    [SerializeField]
    private TeleportController[] linkedDoors;

    private AudioSource plateAudio;
    private Animator plateAnimator;
    private bool isActive;

    private void Start()
    {
        plateAudio = this.GetComponent<AudioSource>();
        plateAnimator = this.GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "MoveableObject")
        {
            isActive = true;
            plateAnimator.SetBool("Activated", isActive);
            plateAudio.Play();

            for (int i = 0; i < linkedDoors.Length; i++)
            {
                linkedDoors[i].KeyActivated();
            }
        }       
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag == "MoveableObject")
        {
            isActive = false;
            plateAnimator.SetBool("Activated", isActive);
            plateAudio.Play();

            for (int i = 0; i < linkedDoors.Length; i++)
            {
                linkedDoors[i].KeyDeactivated();
            }
        }
        
    }

}
