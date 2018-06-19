using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PlayerAudioController : MonoBehaviour
{
    private AudioSource playerAudio;

    [Header("Footsteps")]
    [SerializeField]
    private AudioClip[] footstepClips;
    private int footstepIndex;

    private void Start()
    {
        playerAudio = this.GetComponent<AudioSource>();
    }

    public void PlayFootstep()
    {
        int footstep = 0;

        do
        {
            footstep = Random.Range(0, footstepClips.Length);
        }
        while (footstep == footstepIndex);

        footstepIndex = footstep;

        playerAudio.clip = footstepClips[footstepIndex];
        playerAudio.Play();
    }
}
