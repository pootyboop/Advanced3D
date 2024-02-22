using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuCamSway : MonoBehaviour
{
    Vector3 originalPos;
    public float swayTime = 5.0f;
    public float swayAmplitude = 2.0f;

    void Start()
    {
        originalPos = transform.localPosition;
        StartCoroutine(Sway());
    }



    IEnumerator Sway() {

        while (true) {

            float time = 0.0f;
            Vector3 newPos = GetNewPos();
            Vector3 oldPos = transform.localPosition;


            while (time < swayTime) {
                time += Time.deltaTime;
                transform.localPosition = Vector3.Lerp(oldPos, newPos, Mathf.SmoothStep(0.0f, 1.0f, time / swayTime));
                yield return null;
            }



            yield return null;
        }
    }



    Vector3 GetNewPos() {
        return new Vector3(RandSway(originalPos.x), RandSway(originalPos.y), RandSway(originalPos.z));
    }



    float RandSway(float axis) {
        return axis + Random.Range(swayAmplitude * -1f, swayAmplitude);
    }
}
