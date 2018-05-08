using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private LevelManager levelManager;
    private CameraMovementBehaviour movementBehaviour;

    private void Start()
    {
        levelManager = this.transform.root.GetComponent<LevelManager>();
        movementBehaviour = this.GetComponent<CameraMovementBehaviour>();
        movementBehaviour.enabled = false;
    }

    public void MoveToPosition(Vector3 taregtPos, Vector3 targetRot)
    {
        movementBehaviour.InitMovement(taregtPos, targetRot);
    }

    public void CameraIsInNewPosition()
    {
        levelManager.SpawnPlayerAtNewPosition();
    }

}
