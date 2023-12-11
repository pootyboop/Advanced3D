using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Character : MonoBehaviour
{
    private Animator animator;
    private float lookAtPlayerWeight = 3.0f;

    public bool looksAtPlayerBeforeInteracting = true;
    public bool lookingAtPlayer;

    void Start()
    {
        animator = GetComponent<Animator>();
    }



    void OnAnimatorIK()
    {
        Debug.Log("LOOKING");
        LookAtPlayer(lookingAtPlayer);
    }



    public void SetInDialogue(bool inDialogue)
    {
        animator.SetBool("inDialogue", inDialogue);

        //only do this if we werent already looking at player
        //if (!looksAtPlayerBeforeInteracting)
        //{
            lookingAtPlayer = inDialogue;
        //}
    }



    public void LookAtPlayer(bool lookAtPlayer)
    {
        if (lookAtPlayer)
        {
            animator.SetLookAtWeight(lookAtPlayerWeight);
            animator.SetLookAtPosition(CameraController.instance.transform.position);
        }

        else
        {
            animator.SetLookAtWeight(0f);
        }
    }
}