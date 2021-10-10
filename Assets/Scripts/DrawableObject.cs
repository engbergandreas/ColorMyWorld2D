using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class DrawableObject : ColorableObject
{
    public Texture2D drawableTexture;
    
    /// <summary>
    /// How much of the surface has to be painted before it is accepted as correct
    /// </summary>
    [Range(0, 1)]
    public float threshold = 0.70f;

    /// <summary>
    /// Should this object fire any events when completely drawn
    /// </summary>
    public UnityEvent _event; 

    protected bool canGivePoints = true;
    private int textureSize = 64;
    private MeshRenderer meshRenderer;


    protected override void Start()
    {
        base.Start();
        //Setup drawable texture properties
        float grayscale = 0.2f;
        meshRenderer = GetComponent<MeshRenderer>();
        drawableTexture = new Texture2D(textureSize, textureSize);
        drawableTexture.wrapMode = TextureWrapMode.Clamp;

        //Set all pixels to grayscale value
        for (int i = 0; i < drawableTexture.width; i++)
        {
            for (int j = 0; j < drawableTexture.height; j++)
            {
                Color c = ColorGun.Instance.RGBChannelToColor(desiredColor);
                c *= 0.2f;
                drawableTexture.SetPixel(i, j, c);
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
    public void ColorTarget(Vector3 hitPoint, Color color, Camera _cam, Texture2D mask)
    {
        Vector3 planeMin = _cam.WorldToScreenPoint(meshRenderer.bounds.min);
        Vector3 planeMax = _cam.WorldToScreenPoint(meshRenderer.bounds.max);

        float xProportion = Mathf.InverseLerp(planeMin.x, planeMax.x, hitPoint.x);
        float yProportion = Mathf.InverseLerp(planeMin.y, planeMax.y, hitPoint.y);

        int xPoint = Mathf.RoundToInt(xProportion * drawableTexture.width);
        int yPoint = Mathf.RoundToInt(yProportion * drawableTexture.height);

        ColorArea(xPoint, yPoint, color, mask);
        //Debug.Log(CalculateColorFraction());
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
    /// What happens when the obj has been fully colored 
    /// </summary>
    protected virtual void FullyColored()
    {
        if (canGivePoints)
        {
            PointSystem.Instance.AddPoints(pointsToGive);
            canGivePoints = false;
            ColorGun.Instance.colorChannelEvent.RemoveListener(OnChangedColorGun); //if it should stay the same color it is when accepted as correct

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

                int u = (int) Mathf.Floor(((float)i - (x - r)) / (2 * r) * mask.width);
                int v = (int) Mathf.Floor(((float)j - (y - r)) / (2 * r) * mask.height);
                

                float maskColor = 1 - mask.GetPixel(u, v).r;
                if (maskColor < 0.2)
                    continue;

                Color currentColor = drawableTexture.GetPixel(i, j);
                drawableTexture.SetPixel(i, j, Color.Lerp(currentColor, color, 0.5f) * maskColor);
            }
        }
        drawableTexture.Apply();
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



