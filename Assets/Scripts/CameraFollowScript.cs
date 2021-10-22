using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowScript : MonoBehaviour
{
    public Transform followTarget;
    public float yoffset = 0.0f;

    
    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(followTarget.position.x, followTarget.position.y + yoffset, transform.position.z);
    }
}
