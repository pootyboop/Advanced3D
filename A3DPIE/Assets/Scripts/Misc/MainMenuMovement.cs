using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(KitOrel))]
public class MainMenuMovement : MonoBehaviour
{

    public float speed = 1f;
    public float speedRandomRange = 1f;

    void Start()
    {
        GetComponent<KitOrel>().StartTrailRenderers();
    }

    void FixedUpdate()
    {
        transform.position = transform.position + transform.forward * (speed + Random.Range(-1 * speedRandomRange, speedRandomRange));
    }
}
