using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinningStars : MonoBehaviour
{
    public bool autoSpin = true;
    public Vector3 rotationSpeed = new Vector3(0.0f, 1.0f, 0.0f);



    void Start()
    {
        if (autoSpin) {
        StartSpinning();
        }   
    }



    public void StartSpinning() {
        StartCoroutine(Spin());
    }



    IEnumerator Spin() {
        while (true) {
            transform.Rotate(rotationSpeed);
            yield return null;
        }
    }
}
