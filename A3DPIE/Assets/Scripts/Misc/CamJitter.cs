using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamJitter : MonoBehaviour
{
    public float jitterStrength = 0.1f;

    void Start()
    {
        
    }

    void Update()
    {
        transform.localPosition = new Vector3(RandomJitter(), RandomJitter(), RandomJitter());
    }

    float RandomJitter() {
        return Random.Range(jitterStrength * -1, jitterStrength);
    }
}
