using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;



public class Trigger : MonoBehaviour
{
    public RGBChannel gunColor;

    public bool KillPlayer = false;
    public bool fireEventOnce = false;

    public UnityEvent onTriggered;

    public UnityEvent<Vector3> onTriggeredVector;

    public UnityEvent<RGBChannel> onTriggerChannel;

    private bool eventFired = false;
    private void Start()
    {
        if (KillPlayer)
        {
            onTriggered.AddListener(Player.Instance.Kill);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(fireEventOnce)
        {
            if (eventFired)
                return;
        }

        if (other.name == "Player")
        {
            onTriggered.Invoke();
            onTriggeredVector.Invoke(transform.position);
            onTriggerChannel.Invoke(gunColor);
            eventFired = true;
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
