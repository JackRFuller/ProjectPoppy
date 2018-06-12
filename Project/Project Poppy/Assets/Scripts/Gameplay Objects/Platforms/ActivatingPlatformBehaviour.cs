using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class ActivatingPlatformBehaviour : PlatformBehaviour
{
    private MeshRenderer platformRenderer;
    private BoxCollider2D platformCollider;

    [SerializeField]
    private PlatformState platformState;
    private enum PlatformState
    {
        Active,
        Disabled,
    }

    [SerializeField] private Material platformActiveMaterial;
    [SerializeField] private Material platformDisabledMaterial;

	public Vector3Lerping lerping;

    private void Start()
    {
        platformRenderer = GetComponent<MeshRenderer>();
        platformCollider = GetComponent<BoxCollider2D>();

        switch (platformState)
        {
            case PlatformState.Active:
                ActivatePlatform();
                break;
            case PlatformState.Disabled:
                DisablePlatform();
                break;
        }
    }

    public override void ActivateBehaviour()
    {
        switch (platformState)
        {
            case PlatformState.Active:
                DisablePlatform();
                platformState = PlatformState.Disabled;
                break;
            case PlatformState.Disabled:
                ActivatePlatform();
                platformState = PlatformState.Active;
                break;
        }
    }

    private void ActivatePlatform()
    {
        gameObject.layer = LayerMask.NameToLayer("Platform");
        platformRenderer.material = platformActiveMaterial;
    }

    private void DisablePlatform()
    {
        gameObject.layer = LayerMask.NameToLayer("Invisible");
        platformRenderer.material = platformDisabledMaterial;
    }


	

}
