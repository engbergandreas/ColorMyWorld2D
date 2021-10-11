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

    private void Start()
    {
        col = GetComponent<BoxCollider2D>();
        ColorGun.Instance.rgbChannelEvent.AddListener(OnChannelChange);
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
