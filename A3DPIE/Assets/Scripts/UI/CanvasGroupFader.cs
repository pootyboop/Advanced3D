using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



//fades in/out canvas groups
public class CanvasGroupFader : MonoBehaviour
{
    public CanvasGroup alpha;
    public float fadeSpeed = 1.0f;
    IEnumerator coroutine;



    //directly set the CanvasGroup's alpha
    //also stop fading if currently doing so
    public void SetAlpha(float newAlpha)
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }
        alpha.alpha = newAlpha;
    }



    //start fading
    public void FadeAlpha(bool fadeIn)
    {
        coroutine = FadeAlphaOverTime(fadeIn);
        StartCoroutine(coroutine);
    }



    //fade toward target state
    IEnumerator FadeAlphaOverTime(bool fadeIn)
    {
        float fadeAmount = fadeSpeed;

        //subtract fadeSpeed instead if fading out
        if (!fadeIn)
        {
            fadeAmount *= -1.0f;
        }

        while (
            (alpha.alpha < 1.0f && fadeIn)  //fading in and not at max alpha
            ||
            (alpha.alpha > 0.0f && !fadeIn) //fading out and not invisible
            )
        {
            alpha.alpha += fadeAmount * Time.deltaTime;
            yield return null;
        }
    }
}
