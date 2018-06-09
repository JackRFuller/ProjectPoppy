using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehaviour : Entity
{
    protected Camera camera;
    protected CameraController cameraController;

    protected virtual void Start()
    {
        camera = GetComponent<Camera>();
        cameraController = GetComponent<CameraController>();
    }
}
