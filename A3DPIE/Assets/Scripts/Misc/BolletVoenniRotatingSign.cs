using System.Collections;
using System.Collections.Generic;
using UnityEngine;



//rotates the Bollet Voenni signs on the F1 bar
public class BolletVoenniRotatingSign : MonoBehaviour
{
    public float rotateSpeed = 0.1f;

    void FixedUpdate()
    {
        transform.Rotate(0.0f, 0.0f, rotateSpeed);
    }
}
