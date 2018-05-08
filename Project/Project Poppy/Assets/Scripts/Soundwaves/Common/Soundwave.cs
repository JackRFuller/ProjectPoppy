using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soundwave : Entity
{
    private Vector3 originalSize;
    [SerializeField] private float growthSpeed;

    protected virtual void Start()
    {
        originalSize = transform.localScale;
    }

    public void ResetSoundwave(Vector3 position)
    {
        SetScale(originalSize);
        SetPosition(position);
        ShowObject();
    }

    public virtual void GrowSoundWave()
    {
        float speed = growthSpeed * Time.deltaTime;
        Vector2 newSize = new Vector2(transform.localScale.x + speed, transform.localScale.y + speed);

        SetScale(newSize);
    }

    public virtual void StopSoundwave()
    {
        HideObject();
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
       
    }
}
