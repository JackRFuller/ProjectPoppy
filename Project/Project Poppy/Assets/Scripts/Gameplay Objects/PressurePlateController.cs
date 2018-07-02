using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PressurePlateController : MonoBehaviour
{
    [SerializeField]
    private PlatformController[] m_linkedPlatforms;

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
            
            for(int i = 0; i < m_linkedPlatforms.Length; i++)
            {
                m_linkedPlatforms[i].ActivateBehaviour();
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

            for(int i = 0; i < m_linkedPlatforms.Length; i++)
            {
                m_linkedPlatforms[i].ActivateBehaviour();
            }    
        }
        
    }

}
