using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



//fades in/out canvas groups
public class CanvasGroupFader : MonoBehaviour
{
    public CanvasGroup alpha;
    public float fadeSpeed = 0.1f;



    //start fading
    public void FadeAlpha(bool fadeIn)
    {
        StartCoroutine(FadeAlphaOverTime(fadeIn));
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
            alpha.alpha += fadeAmount;
            yield return new WaitForSeconds(0.05f); //arbitrary value
        }
    }
}
