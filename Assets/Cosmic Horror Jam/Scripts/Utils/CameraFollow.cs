using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;
    public Vector3 offset;

    // Update is called once per frame
    void Update()
    {
        //Camera.main.transform.position = player.position + offset;
        //Camera.main.transform.rotation = new Quaternion(
        //    Quaternion.identity.x + 0.5f,
        //    Quaternion.identity.y,
        //    Quaternion.identity.z,
        //    Quaternion.identity.w);
    }
}
