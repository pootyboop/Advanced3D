using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;



//fades in the worldspace logo "Gata Guressi!" in cutscene
public class Logo : MonoBehaviour
{
    public TMP_Text text;
    public float fadeSpeed = 5.0f;  //how fast the logo fades in
    public float moveSpeed = 50.0f; //how fast the logo moves upward

    private IEnumerator fade;



    //start fading in the logo
    public void FadeIn()
    {
        fade = Fade();
        StartCoroutine(fade);
    }



    //fade in the logo over time
    IEnumerator Fade()
    {
        float startAlpha = text.color.a;    //instead of 0, start with the preexisting alpha in case it's changed in the inspector
        float time = 0f;    //how far into the animation we are

        while (time < fadeSpeed)
        {
            time += Time.deltaTime;

            float alpha = Mathf.Lerp(startAlpha, 1.0f, time / fadeSpeed);   //lerp the alpha
            text.color = new Color(text.color.r, text.color.g, text.color.b, alpha);

            //move the logo upward
            transform.position = new Vector3(transform.position.x, transform.position.y + moveSpeed * Time.deltaTime, transform.position.z);

            //wait a moment before changing fade again. won't be noticeable
            yield return new WaitForSeconds(.05f);
        }
    }



    public void HideLogo()
    {
        if (fade != null)
        {
            StopCoroutine(fade);
        }
        text.color = new Color(text.color.r, text.color.g, text.color.b, 0.0f);
    }
}
