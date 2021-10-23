using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneColorShaderHandler : MonoBehaviour
{
    int id;

    Color red = new Color(1.0f, 0.3f, 0.3f);
    Color green = new Color(0.3f, 1.0f, 0.3f);
    Color blue = new Color(0.3f, 0.3f, 1.0f);

    // Start is called before the first frame update
    void Start()
    {
        id = Shader.PropertyToID("_TestColor");
        Shader.SetGlobalColor(id, Color.white);
        ColorGun.Instance.rgbChannelEvent.AddListener(OnChangedChannel);
    }

    private void OnChangedChannel(RGBChannel channel)
    {
        switch (channel)
        {
            case RGBChannel.Red:
                Shader.SetGlobalColor(id, red);
                break;
            case RGBChannel.Green:
                Shader.SetGlobalColor(id, green);
                break;
            case RGBChannel.Blue:
                Shader.SetGlobalColor(id, blue);
                break;
            default:
                break;
        }
    }
}
