using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Logo : MonoBehaviour
{
    public TMP_Text text;
    public float fadeSpeed = 5.0f;
    public float moveSpeed = 50.0f;



    public void FadeIn()
    {
        StartCoroutine(Fade());
    }



    IEnumerator Fade()
    {
        float startAlpha = text.color.a;
        float time = 0f;

        while (time < fadeSpeed)
        {
            time += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, 1.0f, time / fadeSpeed);
            text.color = new Color(text.color.r, text.color.g, text.color.b, alpha);

            transform.position = new Vector3(transform.position.x, transform.position.y + moveSpeed * Time.deltaTime, transform.position.z);

            yield return new WaitForSeconds(.05f);
        }
    }
}
