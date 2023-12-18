using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;



public enum ECharacterState
{
    STANDING = 0,
    SITTING = 1,
    LEANING = 2,
    BOXING1 = 3,
    BOXING2 = 4,
    MUSIC1 = 5,
    MUSIC2 = 6,
    MUSIC3 = 7
}



[RequireComponent(typeof(Animator))]
public class Character : MonoBehaviour
{
    private Animator animator;
    private float lookAtPlayerWeight = 3.0f;

    private IEnumerator lookAtCoroutine;
    public Transform lookAtTransform;
    public MultiAimConstraint lookAtTransformParent;
    private Vector3 defaultLookAt;
    private float lookAtTime = 0.3f;

    public ECharacterState state;

    public bool looksAtPlayerBeforeInteracting = true;
    public bool lookingAtPlayer = false;

    private float seatRadius = 0.75f;



    void Start()
    {
        animator = GetComponent<Animator>();
        defaultLookAt = lookAtTransform.position;

        SetCharacterState(state);
    }



    public void SetInDialogue(bool inDialogue)
    {
        animator.SetBool("inDialogue", inDialogue);

        //only do this if we werent already looking at player
        if (!looksAtPlayerBeforeInteracting && !lookingAtPlayer)
        {
            LookAtPlayer(true);
        }
    }



    public void SetDialogueState(ESpeaker speaker)
    {
        animator.SetInteger("speakerEnum", (int)speaker);
    }



    public void SetCharacterState(ECharacterState characterState)
    {

        state = characterState;
        animator.SetInteger("characterStateEnum", (int)state);

        switch (state)
        {
            case ECharacterState.SITTING:
                TryFindSeat();
                break;
            default:
                break;
        }
    }



    public void TryFindSeat()
    {
        Collider[] possibleSeats = Physics.OverlapSphere(transform.position, seatRadius);
        for (int i = 0; i < possibleSeats.Length; i++)
        {
            //just grab the first one we find. assuming seats aren't too close together.
            Seat seat = possibleSeats[i].gameObject.GetComponent<Seat>();

            if (seat != null)
            {
                Sit(seat);
                return;
            }
        }
    }



    void Sit (Seat seat)
    {
        transform.position = seat.transform.position;
        Vector3 eulerAngles = seat.transform.eulerAngles;
        eulerAngles = new Vector3(eulerAngles.x, eulerAngles.y, eulerAngles.z);
        transform.rotation = Quaternion.Euler(eulerAngles);

        seat.occupied = true;
    }



    public void LookAtPlayer(bool lookAtPlayer)
    {
        if (lookAtPlayer)
        {
            //lookAtTransform.position = CameraController.instance.transform.position;
            if (lookAtCoroutine != null)
            {
                StopCoroutine(lookAtCoroutine);
            }
            lookAtCoroutine = LookLerp(lookAtTransform.position, CameraController.instance.transform.position, true);
            StartCoroutine(lookAtCoroutine);
        }

        else
        {
            //lookAtTransform.position = defaultLookAt;
            StopCoroutine(lookAtCoroutine);
            lookAtCoroutine = LookLerp(lookAtTransform.position, defaultLookAt, false);
            StartCoroutine(lookAtCoroutine);
        }
    }


    //adapted from https://discussions.unity.com/t/vector3-lerp-works-outside-of-update/20618/4
    //superpig
    IEnumerator LookLerp(Vector3 start, Vector3 end, bool parentToPlayerCam)
    {
        //unparent from cam
        if (!parentToPlayerCam)
        {
            lookingAtPlayer = false;

            //lookAtTransform.SetParent(lookAtTransformParent.transform, true);
            //lookAtTransformParent.offset
        }



        //move
        float startTime = Time.time;
        while (Time.time < startTime + lookAtTime)
        {
            lookAtTransform.position = Vector3.Lerp(start, end, (Time.time - startTime) / lookAtTime);
            yield return null;
        }

        lookAtTransform.position = end;



        //parent to cam
        if (parentToPlayerCam)
        {
            lookingAtPlayer = true;
            StartCoroutine(CamFollowWorkaround());

            //lookAtTransform.SetParent(CameraController.instance.transform, true);
        }
    }


    //workaround for a bug with the unity Animation Rigging package
    //can't reparent the source object without breaking the multi-aim constraint
    //so just set this position to follow the camera
    IEnumerator CamFollowWorkaround()
    {
        while (lookingAtPlayer)
        {
            lookAtTransform.position = CameraController.instance.transform.position;
            yield return new WaitForSeconds(0.1f);
        }
    }
}