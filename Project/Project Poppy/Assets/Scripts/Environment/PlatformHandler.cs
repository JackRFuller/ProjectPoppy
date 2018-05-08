using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformHandler : MonoBehaviour
{
    protected BoxCollider2D platformCollider;
    protected MeshRenderer platformRenderer;
    protected Material platformStartingMaterial;

    protected virtual void Start()
    {
        platformCollider = this.GetComponent<BoxCollider2D>();
        platformRenderer = this.GetComponent<MeshRenderer>();
        platformStartingMaterial = platformRenderer.material;
    }
	
}
