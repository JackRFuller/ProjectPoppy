using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeBellSoundwave : Soundwave
{
    protected override void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "DynamicObject")
        {
            other.SendMessage("HitByLifeBell",SendMessageOptions.DontRequireReceiver);
        }
    }
}
