using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingIsland : MonoBehaviour
{
    float originalY = -6969.69f;
    public float floatAmount = 15f;
    public float floatTime = 8f;

    private IEnumerator floatCoroutine;



    void Start()
    {
        if (originalY == -6969.69f) {
            originalY = transform.localPosition.y;
        }
    }



    private void OnEnable() {

        if (originalY == -6969.69f) {
            originalY = transform.localPosition.y;
        }

        if (floatCoroutine != null) {
            StopCoroutine(floatCoroutine);
        }
        floatCoroutine = Float();
        StartCoroutine(floatCoroutine);
    }



    IEnumerator Float() {
        //start at random offset
        //float time = UnityEngine.Random.Range(0.0f, floatTime);
        float time = 0.0f;

        while (true) {
            float oldPos = transform.localPosition.y;
            float newPos = originalY + UnityEngine.Random.Range(floatAmount * -1.0f, floatAmount);


            while (time < floatTime) {
                time += Time.deltaTime;
                transform.localPosition = new Vector3(
                    transform.localPosition.x, 
                    Mathf.Lerp(oldPos, newPos, Mathf.SmoothStep(0.0f, 1.0f, time / floatTime)), 
                    transform.localPosition.z
                    );
                yield return null;
            }

            time = 0.0f;

            yield return null;
        }
    }
}
