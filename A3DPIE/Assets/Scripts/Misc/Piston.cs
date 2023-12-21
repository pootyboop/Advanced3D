using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piston : MonoBehaviour
{
    public float startDelay = 0.0f;
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        StartCoroutine(StartDelay());
    }



    IEnumerator StartDelay()
    {
        yield return new WaitForSeconds(startDelay);
        animator.enabled = true;
    }
}
