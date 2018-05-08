using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    protected void ShowObject()
    {
       gameObject.SetActive(true);
    }

    protected void HideObject()
    {
        gameObject.SetActive(false);
    }

    protected void SetPosition(Vector3 position)
    {
        transform.position = position;
    }

    protected void SetRotation(Vector3 rotation)
    {
        transform.rotation = Quaternion.Euler(rotation);
    }

    protected void SetScale(Vector3 scale)
    {
        transform.localScale = scale;
    }
	
}
