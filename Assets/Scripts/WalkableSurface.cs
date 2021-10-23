using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkableSurface : DrawableObject
{
    //private MeshCollider col;
    private BoxCollider2D col;
    private bool canTrigger = true;

    protected override void Start()
    {
        base.Start();
        col = GetComponent<BoxCollider2D>();
        col.isTrigger = true;
    }

    protected override void FullyColored()
    {
        if (canTrigger)
        {
            col.isTrigger = false;
            staticColor = true;
            canTrigger = false;
            ShowTrueColor();
            _event.Invoke();
            PlaySoundEffect();
        }
    }

    protected override void OnPartiallyColored()
    {
        if (canTrigger)
        {
            col.isTrigger = true;
            staticColor = false;
            ShowRGBColor();
        }
    }
}
