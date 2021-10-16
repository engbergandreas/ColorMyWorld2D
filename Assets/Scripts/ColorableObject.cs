using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Objects that should shift color on channel change
/// </summary>
public class ColorableObject : MonoBehaviour
{
    public RGBChannel desiredColor;
    public int pointsToGive = 10;
    public bool showTrueColorOnCorrectChannel = false;

    protected Color desiredColorasColor;  
    protected Color ShaderColorMultiplier;
    protected Renderer _renderer;
    /// <summary>
    /// If static color is true the object does not change color when player changes channel
    /// </summary>
    protected bool staticColor = false;
    // Start is called before the first frame update
    protected virtual void Start()
    {
        switch (desiredColor)
        {
            case RGBChannel.Red:
                desiredColorasColor = Color.red;
                break;
            case RGBChannel.Green:
                desiredColorasColor = Color.green;

                break;
            case RGBChannel.Blue:
                desiredColorasColor = Color.blue;
                break;
            default:
                break;
        }

        //GetComponent<Renderer>().material.mainTexture = texture;
        _renderer = GetComponent<Renderer>();
        //Get all the materials on the obj
        var materials = _renderer.materials;
        foreach (var material in materials)
        {
            //Enable shader keywords so they can be changed later
            material.EnableKeyword("_PaintedTex");
            material.EnableKeyword("_Color");
            material.EnableKeyword("_Mix");
            //material.SetTexture("_PaintedTex", drawableTexture);
        }

        ColorGun.Instance.colorChannelEvent.AddListener(OnChangedColorGun);
    }

    //private void OnDestroy()
    //{
    //    ColorGun.Instance.colorChannelEvent.RemoveListener(OnChangedColorGun);
    //}

    //TODO: find more optimal way to change all textures color in the shader
    public void OnChangedColorGun(Color color)
    {
        ShaderColorMultiplier = color;

        if (staticColor)
            return;

        //Renderer _renderer = GetComponent<Renderer>();
        //_renderer.material.SetColor("_Color", ShaderColorMultiplier);
        
        var materials = _renderer.materials;
        foreach (var material in materials)
        {
            if (color == desiredColorasColor && showTrueColorOnCorrectChannel)
            {
                material.SetColor("_Color", Color.white);
            }
            else
                material.SetColor("_Color", ShaderColorMultiplier);
        }

    }

    protected void ShowTrueColor()
    {
        showTrueColorOnCorrectChannel = true;
        var materials = _renderer.materials;
        foreach (var material in materials)
        {
            material.SetColor("_Color", Color.white);
            material.SetFloat("_Mix", 0.0f);
        }
    }
    protected void ShowRGBColor()
    {
        showTrueColorOnCorrectChannel = false;
        var materials = _renderer.materials;
        foreach (var material in materials)
        {
            material.SetColor("_Color", ShaderColorMultiplier);
            material.SetFloat("_Mix", 0.5f);
        }
    }
}
