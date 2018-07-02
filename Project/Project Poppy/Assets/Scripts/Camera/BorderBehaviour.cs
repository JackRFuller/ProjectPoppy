using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BorderBehaviour : Entity
{
    private CameraController cameraController;
    [SerializeField] private int borderIndex;
    
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Vector3 playerPosition = other.transform.position;

            if(cameraController == null)
                cameraController = Manager.Instance.LevelManager.CameraController;

            cameraController.ScrollingBehaviour.TurnOnDirectionIndicator(borderIndex,playerPosition);
        }
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            cameraController.ScrollingBehaviour.TurnOffDirectionIndicator(borderIndex);
        }
    }
}
