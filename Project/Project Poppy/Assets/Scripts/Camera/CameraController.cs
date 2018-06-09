using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private LevelManager levelManager;
    private CameraMovementBehaviour movementBehaviour;
    public CameraMovementBehaviour CameraMovementBehaviour { get { return movementBehaviour;} }
    private CameraScrollingBehaviour scrollingBehaviour;
    public CameraScrollingBehaviour ScrollingBehaviour { get { return scrollingBehaviour;} }

    [Header("Player")]
    [SerializeField]
    private PlayerController playerController;
    public PlayerController PlayerController { get { return playerController;} }

    private void Start()
    {
        levelManager = Manager.Instance.LevelManager;
        movementBehaviour = GetComponent<CameraMovementBehaviour>();
        scrollingBehaviour = GetComponent<CameraScrollingBehaviour>();
    }
}
