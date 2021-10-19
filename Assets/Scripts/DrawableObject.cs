using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(BoxCollider2D))]
public class DrawableObject : ColorableObject
{
    public Texture2D drawableTexture;
    public SpriteRenderer colorIcon;
    /// <summary>
    /// How much of the surface has to be painted before it is accepted as correct
    /// </summary>
    [Range(0, 1)]
    public float threshold = 0.70f;

    public int textureSizeMultiplier = 1;
    /// <summary>
    /// Should this object fire any events when completely drawn
    /// </summary>
    public UnityEvent _event; 

    protected bool canGivePoints = true;
    private int textureSize = 64;
    //private MeshRenderer meshRenderer;
    private SpriteRenderer spriteRenderer;

    protected override void Start()
    {
        base.Start();
        //Setup drawable texture properties
        float grayscale = 1.0f;
        textureSize *= textureSizeMultiplier;
        threshold /= textureSizeMultiplier;
        //meshRenderer = GetComponent<MeshRenderer>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        drawableTexture = new Texture2D(textureSize, textureSize);
        drawableTexture.wrapMode = TextureWrapMode.Clamp;

        if(colorIcon)
            colorIcon.color = desiredColorasColor;

        //Set all pixels to grayscale value
        for (int i = 0; i < drawableTexture.width; i++)
        {
            for (int j = 0; j < drawableTexture.height; j++)
            {
                Color showdesiredcoloronstart = ColorGun.Instance.RGBChannelToColor(desiredColor);
                showdesiredcoloronstart *= 0.2f;
                Color grayScale = new Color(grayscale, grayscale, grayscale);
                drawableTexture.SetPixel(i, j, grayScale);
            }
        }
        drawableTexture.Apply();

        //Get all the materials on the obj
        var materials = _renderer.materials;
        foreach (var material in materials)
        {
            material.SetTexture("_PaintedTex", drawableTexture);
        }
    }

    /// <summary>
    /// Color target at hitpoint, with color: color
    /// </summary>
    /// <param name="hitPoint"></param>
    /// <param name="color"></param>
    /// <param name="_cam"></param>
    public Vector2Int ColorTarget(Vector3 hitPoint, Color color, Camera _cam)
    {
        Vector3 planeMin = _cam.WorldToScreenPoint(spriteRenderer.bounds.min);
        Vector3 planeMax = _cam.WorldToScreenPoint(spriteRenderer.bounds.max);

        float xProportion = Mathf.InverseLerp(planeMin.x, planeMax.x, hitPoint.x);
        float yProportion = Mathf.InverseLerp(planeMin.y, planeMax.y, hitPoint.y);

        int xPoint = Mathf.RoundToInt(xProportion * drawableTexture.width);
        int yPoint = Mathf.RoundToInt(yProportion * drawableTexture.height);

        //ColorArea(xPoint, yPoint, color, mask);
        //Debug.Log(CalculateColorFraction());
        
        return new Vector2Int(xPoint, yPoint);
    }
    /// <summary>
    /// What happens when the obj has been fully colored 
    /// </summary>
    protected virtual void FullyColored()
    {
        if (canGivePoints)
        {
            PointSystem.Instance.AddPoints(pointsToGive);
            canGivePoints = false;
            ColorGun.Instance.colorChannelEvent.RemoveListener(OnChangedColorGun); //if it should stay the same color it is when accepted as correct
            staticColor = true;
            ShowTrueColor();
        }
        _event.Invoke();
    }
    protected virtual void OnPartiallyColored()
    {

    }

    /// <summary>
    /// Color an area around coordinate (x,y) with color: color
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="color"></param>
    private void ColorArea(int x, int y, Color color, Texture2D mask)
    {
        int r = 15;
        for (int i = x - r; i < x + r; i++)
        {
            //Values outside texture width are discarded
            if (i < 0 || i >= drawableTexture.width)
                continue;

            for (int j = y - r; j < y + r; j++)
            {
                //Values outside texture height are discarded
                if (j < 0 || j >= drawableTexture.height)
                    continue;

                //Get u,v coordinates of the mask scaled to 0 -> mask width
                int u = (int) Mathf.Floor(((float)i - (x - r)) / (2 * r) * mask.width);
                int v = (int) Mathf.Floor(((float)j - (y - r)) / (2 * r) * mask.height);

                //invert mask color, obs grayscale mask
                float maskColor = 1 - mask.GetPixel(u, v).r;
                float threshold = 0.2f;
                if (maskColor < threshold)
                    continue;

                Color currentColor = drawableTexture.GetPixel(i, j);
                drawableTexture.SetPixel(i, j, Color.Lerp(currentColor, color, 0.5f) * maskColor);
            }
        }
        drawableTexture.Apply();
    }

    public void ApplySplatter(Vector2Int texCoords, Texture2D mask, Color color)
    {
        ColorArea(texCoords.x, texCoords.y, color, mask);
        if (CalculateColorFraction() >= threshold) //very inefficient to calculate fraction every time we shoot at target
        {
            FullyColored();
        }
        else
        {
            OnPartiallyColored();
        }
    }

    /// <summary>
    /// Calculate how much of the texture is the correct color
    /// </summary>
    /// <returns></returns>
    private float CalculateColorFraction()
    {
        float tolerance = 0.3f; //Changes how tolerant the system is to the correct color eg 30% off true value;
        var pixels = drawableTexture.GetPixels32();
        int correctPixels = 0;

        for (int i = 0; i < pixels.Length; i++)
        {
            if (Vector4.Magnitude(pixels[i] - desiredColorasColor) <= tolerance)
            {
                correctPixels++;
            }
        }
        return (float)correctPixels / pixels.Length;
    }

    public void ResetToOriginal()
    {
        //TODO add reset obj to original state
        Debug.Log("TODO:");
    }
}



