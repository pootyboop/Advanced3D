using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;



//just makes the camera follow the player without being parented to it
//the unity CharacterController has some really weird rotation bugs when the camera is a child of it...
//...so this bypasses that
public class CameraFollow : MonoBehaviour
{
    //where the camera will stay attached to
    //called an "offset" because this will reference a transform that is offset from the player's root transform
    public Transform cameraOffset;

    void Update()
    {
        transform.position = cameraOffset.position;
    }
}