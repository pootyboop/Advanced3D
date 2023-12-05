using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform cameraOffset;

    // Update is called once per frame
    void Update()
    {
        transform.position = cameraOffset.position;
    }
}
