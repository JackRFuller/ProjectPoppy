using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeBellSoundwave : Soundwave
{
    protected override void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "DynamicPlatform")
        {
            if(!other.isTrigger)
                other.SendMessage("ActivateBehaviour",SendMessageOptions.DontRequireReceiver);
        }
    }
}
