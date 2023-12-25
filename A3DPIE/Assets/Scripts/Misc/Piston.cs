using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piston : MonoBehaviour
{
    public float startDelay = 0.0f;
    private Animator animator;

    void OnEnable()
    {
        animator = GetComponent<Animator>();
        animator.enabled = false;   //starts disabled, but this is here to make sure the enable delay still works when re-activating
        StartCoroutine(StartDelay());
    }



    IEnumerator StartDelay()
    {
        yield return new WaitForSeconds(startDelay);
        animator.enabled = true;
    }
}
