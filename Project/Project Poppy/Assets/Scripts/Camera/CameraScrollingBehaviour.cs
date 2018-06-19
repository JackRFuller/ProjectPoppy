using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraScrollingBehaviour : CameraBehaviour {
    
    [SerializeField]
	private Transform playerTransform;
    private bool isScrolling;
    public bool IsScrolling {get {return isScrolling;}}

    [Header("UI Elements")] [SerializeField]
    private RectTransform[] directionIndicatorTransforms;
    private Image[] directionIndicatorImages;


    protected override void Start()
    {
        base.Start();
        
        cameraController.CameraMovementBehaviour.reachDestination += RestartCameraScrollingBehaviour;

        directionIndicatorImages = new Image[directionIndicatorTransforms.Length];

        for (int i = 0; i < directionIndicatorTransforms.Length; i++)
        {
            directionIndicatorImages[i] = directionIndicatorTransforms[i].GetComponent<Image>();
            directionIndicatorImages[i].enabled = false;
        }
    }

    private void Update()
    {
        DetectPlayerPastEdgeOfScreen();
    }

    private void DetectPlayerPastEdgeOfScreen()
    {
        if(isScrolling)
            return;

        if(isVisible())
            return;

        if(!cameraController.PlayerController.MovementController.collisions.fullyGrounded)
            return;

        Vector3 screenPoint = camera.WorldToViewportPoint(playerTransform.position);

        Vector2 playerDirection = Manager.Instance.LevelManager.CurrentLevelIndex;

        if (screenPoint.x > 1)
        {
            playerDirection.x += 1;
        }
        else if (screenPoint.x < 0)
        {
            playerDirection.x -= 1;
        }
        else if (screenPoint.y < 0)
        {
            playerDirection.y -= 1;
        }
        else if (screenPoint.y > 1)
        {
            playerDirection.y += 1;
        }

        cameraController.CameraMovementBehaviour.InitCameraScrolling(playerDirection);
        Manager.Instance.LevelManager.SpawnInNextLevel(playerDirection);        

        isScrolling = true;       
    }

    private void RestartCameraScrollingBehaviour()
    {
        isScrolling = false;
    }

    private bool isVisible()
    {
        Vector3 screenPoint = camera.WorldToViewportPoint(playerTransform.position);
        bool _isVisible = screenPoint.z > 0 && screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1;
        return _isVisible;
    }

    public void TurnOnDirectionIndicator(int indicatorIndex, Vector3 playerPosition)
    {
        if(!Manager.Instance.LevelManager.PlayerController.MovementController.collisions.fullyGrounded)
            return;

        Vector2 newPosition = playerPosition;
        newPosition = camera.WorldToScreenPoint(newPosition);

        if (indicatorIndex == 0 || indicatorIndex == 1)
        {
            newPosition.x = directionIndicatorTransforms[indicatorIndex].position.x;
        }
        else if (indicatorIndex == 2 || indicatorIndex == 3)
        {
            newPosition.y = directionIndicatorTransforms[indicatorIndex].position.y;
        }
       
        directionIndicatorTransforms[indicatorIndex].position = newPosition;
        directionIndicatorImages[indicatorIndex].enabled = true;
    }


    public void TurnOffDirectionIndicator(int indicatorIndex)
    {
        directionIndicatorImages[indicatorIndex].enabled = false;
    }
}
