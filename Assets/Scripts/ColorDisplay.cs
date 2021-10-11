using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ColorDisplay : MonoBehaviour
{
    public Image imgprefab;
    public RectTransform container;


    private List<Color> pickedUpColors = new List<Color> { Color.red, Color.green, Color.blue }; //TODO change to actual picked up colors
    private List<Image> uiColors = new List<Image>(); //internal list of created ui colors
    private int colorSpacing = 10;
    private int colorCounter = 0;
    // Start is called before the first frame update
    void Start()
    {
        //for(int i =0; i < pickedUpColors.Count; i++)
        //{
        //    CreateColorTransform(imgprefab, container,pickedUpColors[i], i);
        //}
        ColorGun.Instance.rgbChannelEvent.AddListener(OnChangedColor);
    }

    public void AddColorToUI(RGBChannel channel)
    {
        Color c = ColorGun.Instance.RGBChannelToColor(channel);
        CreateColorTransform(imgprefab, container, c, colorCounter);
        colorCounter++;
    }

    private void CreateColorTransform(Image prefab, RectTransform _container, Color _color, int index)
    {
        Image obj = Instantiate(prefab, _container);
        RectTransform objRecTransform = obj.GetComponent<RectTransform>();
        float prefabWidth = objRecTransform.rect.width;
        objRecTransform.anchoredPosition = new Vector2(10 + prefabWidth / 2 + colorSpacing * index + prefabWidth * index, 0);

        _color.a = 0.2f; //set alpha to 0.5
        obj.color = _color;

        int nr = index + 1;
        obj.GetComponentInChildren<TextMeshProUGUI>().SetText(nr.ToString());
        uiColors.Add(obj);
    }

    private void OnChangedColor(RGBChannel newChannel)
    {
        foreach(var img in uiColors)
        {
            var c = img.color;
            c.a = 0.2f;
            img.color = c;
        }

        switch (newChannel)
        {
            case RGBChannel.Red:
                uiColors[0].color = Color.red;
                break;
            case RGBChannel.Green:
                uiColors[1].color = Color.green;
                break;
            case RGBChannel.Blue:
                uiColors[2].color = Color.blue;
                break;
            default:
                break;
        }


    }

}
