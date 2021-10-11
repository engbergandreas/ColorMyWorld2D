using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;



public class Trigger : MonoBehaviour
{
    public RGBChannel gunColor;

    public UnityEvent onTriggered;

    public UnityEvent<Vector3> onTriggeredVector;

    public UnityEvent<RGBChannel> onTriggerChannel;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.name == "Player")
        {
            onTriggered.Invoke();
            onTriggeredVector.Invoke(transform.position);
            onTriggerChannel.Invoke(gunColor);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Player")
        {
            onTriggered.Invoke();
            onTriggeredVector.Invoke(transform.position);
            onTriggerChannel.Invoke(gunColor);
        }
    }
}
