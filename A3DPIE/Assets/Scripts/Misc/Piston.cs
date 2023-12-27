using System.Collections;
using System.Collections.Generic;
using UnityEngine;



//animates pistons
public class Piston : MonoBehaviour
{
    public float startDelay = 0.0f; //used to offset pistons so they don't all have to slam down at the exact same time
    private Animator animator;



    //OnEnable instead of Start so the start delay is applied every time the object is re-activated
    //otherwise the area loading system would ignore the start delay and all pistons would slam down at the same time
    void OnEnable()
    {
        animator = GetComponent<Animator>();
        animator.enabled = false;   //starts disabled, but this is here to make sure the start delay still works when re-activating
        StartCoroutine(StartDelay());
    }



    //wait before starting the animator
    IEnumerator StartDelay()
    {
        yield return new WaitForSeconds(startDelay);
        animator.enabled = true;
    }
}
