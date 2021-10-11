using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkableSurface : DrawableObject
{
    //private MeshCollider col;
    private BoxCollider2D col;

    protected override void Start()
    {
        base.Start();
        col = GetComponent<BoxCollider2D>();
        col.isTrigger = true;
    }

    protected override void FullyColored()
    {
        col.isTrigger = false;
        staticColor = true;
        ShowTrueColor();
        _event.Invoke();
    }

    protected override void OnPartiallyColored()
    {
        col.isTrigger = true;
        staticColor = false;
        ShowRGBColor();
    }
}
