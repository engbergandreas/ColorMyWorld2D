using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO: Subclass of colorable object? -> direct access to desired color instead of having its own
//TODO: visualize when its possible to walk through or not

[RequireComponent(typeof(BoxCollider2D))]
public class ChannelCollision : MonoBehaviour
{
    public RGBChannel canWalkThroughInChannel;

    public bool solidInChannel = false;

    private BoxCollider2D col;

    public SpriteRenderer sr;
    public Sprite fullColor, blue_red, blue_green, red_green;

    private void Start()
    {
        col = GetComponent<BoxCollider2D>();
        ColorGun.Instance.rgbChannelEvent.AddListener(OnChannelChange);
        GetCorrectSprite();
    }

    private void GetCorrectSprite()
    {
        Sprite sprite = fullColor;
        switch (canWalkThroughInChannel)
        {
            case RGBChannel.Red:
                if (solidInChannel)
                {
                    sprite = fullColor;
                    sr.color = new Color(1, 0, 0, 0.5f);
                }
                else
                {
                    sprite = blue_green;
                    sr.color = new Color(1, 1, 1, 0.5f);
                }
                break;
            case RGBChannel.Green:
                if (solidInChannel) {
                    sprite = fullColor;
                    sr.color = new Color(0, 1, 0, 0.5f);
                }
                else
                {
                    sprite = blue_red;
                    sr.color = new Color(1, 1, 1, 0.5f);
                }
                break;
            case RGBChannel.Blue:
                if (solidInChannel)
                {
                    sprite = fullColor;
                    sr.color = new Color(0, 0, 1, 0.5f);
                }
                else
                {
                    sprite = red_green;
                    sr.color = new Color(1, 1, 1, 0.5f);
                }
                break;
            default:
                break;
        }
        sr.sprite = sprite;
    }

    private void OnChannelChange(RGBChannel newChannel)
    {
        col.isTrigger = (newChannel == canWalkThroughInChannel); //true if player is in the same channel as obj otherwise false

        //if we are supposed to be solid in the correct channel switch the trigger to off instead
        if (solidInChannel)
            col.isTrigger = !col.isTrigger;

        //if(newChannel == canWalkThroughInChannel)
        //{
        //    col.isTrigger = true;
        //}else
        //{
        //    col.isTrigger = false;
        //}
    }
}
