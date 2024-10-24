using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations.Rigging;



/*
    Character.LookLerp() is based upon a solution from the Unity forum
    Author: superpig
    https://discussions.unity.com/t/vector3-lerp-works-outside-of-update/20618/4
    Accessed: 10/12/2023
*/



//animations the character can perform
//this can be set in the inspector to easily set character animations
public enum ECharacterState
{
    STANDING = 0,
    SITTING = 1,
    LEANING = 2,
    PLAYINGHARP = 3,    //band
    PLAYINGDIDGERIDOO = 4,
    PLAYINGDRUM = 5,
    BOXING1 = 6,    //boxing/fighting ring
    BOXING2 = 7,
    DANCING = 8,
    ARMSCROSSED = 9,
    DRINKING = 10,
    WALKING = 11
}



//manages character logic and animations
[RequireComponent(typeof(Animator))]
public class Character : MonoBehaviour
{
    private Animator animator;
    private float lookAtPlayerWeight = 3.0f;    //how much the head IK influences the head rotation

    private IEnumerator lookAtCoroutine;    //reference so i can stop this coroutine later
    public Transform lookAtTransform;       //the transform the head rotates to look at
    public MultiAimConstraint lookAtTransformParent;    //part of the animation rigging package
    private Vector3 defaultLookAt;          //default position for the character to look at. by default, in front of their face
    private float lookAtTime = 0.3f;        //how fast the character rotates to look at the player



    public ECharacterState state = ECharacterState.STANDING;    //current base animation/activity



    //most characters will look at the player when looked at, but some like the band or fighters will be preoccupied and don't look
    public bool looksAtPlayerBeforeInteracting = true;
    public bool looksAtPlayerWhenTalking = true;
    
    [HideInInspector]
    public bool lookingAtPlayer = false;    //whether the character is looking at the player or not. DON'T DIRECT SET!
    private float timeBetweenDrinks = 9.0f;  //time between DRINKING characters taking a sip of their drink
    private float timeBetweenDrinksRandom = 3.0f;   //drink time randomly chooses somewhere between + or - this amount of time


    private float seatRadius = 0.75f;   //the size of the radius that sitting characters check in for the nearest seat to sit in. keep this low
    public Transform butt; //where to sit relative to
    float approxButtPos = 0.95f;

    CharacterNavigation charNav;
    private IEnumerator fadeDialogueAnimLayerCoroutine;


    public Transform heldObjectL, heldObjectR;  //held objects in each hand
    private Transform grabL, grabR;             //where held objects snap to

    private bool startCalled = false;



    //using OnEnable instead of Start so characters begin performing their action again once deactivated then activated again by the area loading system
    void OnEnable()
    {
        animator = GetComponent<Animator>();
        defaultLookAt = lookAtTransform.localPosition;
        if (startCalled) {
            SetCharacterState(state);   //start performing whatever action this character is meant to
        }
    }



    private void Start() {
        startCalled = true;
        SetCharacterState(state);   //start performing whatever action this character is meant to
    }



    //called when the character enters/exits dialogue with the player
    public void SetInDialogue(bool inDialogue)
    {
        //start dialogue animations
        animator.SetBool("inDialogue", inDialogue);
        

        if (inDialogue) {
            LerpDialogueLayer(1.0f, 0.0f, 0.5f);
        }

        else {
            LerpDialogueLayer(0.0f, 1.0f, 0.5f);
        }

        //look at player
        if (!looksAtPlayerBeforeInteracting && !lookingAtPlayer && looksAtPlayerWhenTalking)
        {
            LookAtPlayer(true);
        }

        if (state == ECharacterState.WALKING) {
            if (charNav == null) {
                charNav = GetComponent<CharacterNavigation>();
            }

            charNav.SetInDialogue(inDialogue);
        }
    }



    //updates dialogue animations based on who's speaking
    public void SetDialogueState(ESpeaker speaker)
    {
        animator.SetInteger("speakerEnum", (int)speaker);
    }



    //set the base animation/activity
    public void SetCharacterState(ECharacterState characterState)
    {

        state = characterState;
        animator.SetInteger("characterStateEnum", (int)state);  //the animator has an int that matches the character state enum

        switch (state)
        {
            case ECharacterState.SITTING:   //these states are all seated, so find the seat the character is meant to sit in
            case ECharacterState.PLAYINGDIDGERIDOO:
            case ECharacterState.PLAYINGDRUM:
            case ECharacterState.PLAYINGHARP:
                TryFindSeat();
                break;
            case ECharacterState.DRINKING:
                GetDrink();
                break;
            case ECharacterState.WALKING:
                SetupWalking();
                break;
            default:
                break;
        }
    }



    void SetupWalking() {
        looksAtPlayerBeforeInteracting = false; //this is super buggy with movement, just disable on all walking characters

        charNav = GetComponent<CharacterNavigation>();
        charNav.enabled = true;
        charNav.StartWalking();
    }



    public void GetDrink() {
        GameObject charDrink = Instantiate(CharRefsHelper.instance.charDrink);
        GrabObject(charDrink.transform, true);

        charDrink.transform.localPosition = new Vector3(0.0009f, 0.0013f, -0.00065f);
        charDrink.transform.localEulerAngles = new Vector3(90f, 0f, 0f);

        charDrink.gameObject.GetComponent<Collider>().enabled = false;

        StartCoroutine(DrinkingTimer());
    }



    //looks for the nearest seat in a small radius and sits in the first one it finds
    public void TryFindSeat()
    {
        Collider[] possibleSeats = Physics.OverlapSphere(transform.position, seatRadius);
        foreach (Collider coll in possibleSeats)
        {
            //just grab the first one we find. assuming seats aren't too close together.
            //in-editor, seated characters are placed near unoccupied seats with a small radius to check inside
            Seat seat = coll.gameObject.GetComponent<Seat>();

            if (seat != null)
            {
                Sit(seat);
                return;
            }
        }
    }



    //align the character with the seat
    void Sit(Seat seat)
    {
        Vector3 buttPos = butt.position;
        Vector3 seatPos = seat.transform.position;

        transform.position = new Vector3(seatPos.x, seatPos.y - transform.lossyScale.y * approxButtPos, seatPos.z);

        transform.rotation = Quaternion.Euler(seat.transform.eulerAngles);

        seat.occupied = true;
    }



    //setup the grab transforms that the character snaps grabbed items to
    public void SetGrabTransforms(Transform leftGrab, Transform rightGrab)
    {
        grabL = leftGrab;
        grabR = rightGrab;

        //grab anything the character had tried to grab before the grab transforms were ready
        GrabObject(heldObjectL, false);
        GrabObject(heldObjectR, true);
    }



    //snap an object to a hand of choice
    public void GrabObject(Transform newObject, bool isRightHand)
    {
        if (newObject == null)
        {
            return;
        }

        //RIGHT
        if (isRightHand)
        {
            heldObjectR = newObject;
            newObject.SetParent(grabR, true);
            newObject.position = grabR.position;
            newObject.rotation = grabR.rotation;
        }

        //LEFT
        else
        {
            heldObjectL = newObject;
            newObject.SetParent(grabL, true);
            newObject.position = grabL.position;
            newObject.rotation = grabL.rotation;
        }
    }



    //handles rotating the head to look at the player and rotating back to the default rotation
    public void LookAtPlayer(bool lookAtPlayer)
    {
        if (lookAtPlayer)
        {
            //ignore cancelling the last coroutine if this is the first time the character looks at the player
            if (lookAtCoroutine != null)
            {
                StopCoroutine(lookAtCoroutine);
            }

            //start lerping to the player
            lookAtCoroutine = LookLerp(lookAtTransform.localPosition, transform.InverseTransformPoint(CameraController.instance.transform.position), true);
            StartCoroutine(lookAtCoroutine);
        }

        else
        {
            //stop rotating to the player
            if (lookAtCoroutine != null) {
                StopCoroutine(lookAtCoroutine);

                //start lerping back to default rotation
                lookAtCoroutine = LookLerp(lookAtTransform.localPosition, defaultLookAt, false);
                StartCoroutine(lookAtCoroutine);
            }
        }
    }


    
    //lerp the head IK target from the default position to the camera and vice versa
    IEnumerator LookLerp(Vector3 start, Vector3 end, bool parentToPlayerCam)
    {
        //unparent from cam if returning to default rotation
        if (!parentToPlayerCam)
        {
            lookingAtPlayer = false;
        }

        //move the target
        //CODE BASED ON SUPERPIG'S SOLUTION STARTS HERE.
        float startTime = Time.time;
        while (Time.time < startTime + lookAtTime)
        {
            lookAtTransform.localPosition = Vector3.Lerp(start, end, (Time.time - startTime) / lookAtTime);
            yield return null;
        }

        lookAtTransform.localPosition = end;
        //CODE BASED ON SUPERPIG'S SOLUTION ENDS HERE.

        //parent to cam if the target reached the cam
        //parenting to the cam means the character's view follows the camera even if the player moves
        if (parentToPlayerCam)
        {
            lookingAtPlayer = true;
            StartCoroutine(CamFollowWorkaround());
        }
    }


    //workaround for a bug with the unity Animation Rigging package
    //can't reparent the source object without breaking the multi-aim constraint
    //so just set this position to follow the camera
    IEnumerator CamFollowWorkaround()
    {
        while (lookingAtPlayer)
        {
            lookAtTransform.localPosition = transform.InverseTransformPoint(CameraController.instance.transform.position);
            yield return null;
        }
    }



    IEnumerator DrinkingTimer() {
        while (true) {

            float time = 0.0f;
            float randomizedDrinkTime = timeBetweenDrinks + (UnityEngine.Random.Range(timeBetweenDrinksRandom * -1f, timeBetweenDrinksRandom));
            while (time < randomizedDrinkTime) {
                time += Time.deltaTime;
                yield return null;
            }
            animator.SetTrigger("sipDrink");
            yield return null;
        }
    }



    void LerpDialogueLayer(float to, float from, float lerpTime) {
        if (fadeDialogueAnimLayerCoroutine != null) {
            StopCoroutine(fadeDialogueAnimLayerCoroutine);
        }

        fadeDialogueAnimLayerCoroutine = LerpDialogueAnimLayer(to, from, lerpTime);
        StartCoroutine(fadeDialogueAnimLayerCoroutine);
    }



    IEnumerator LerpDialogueAnimLayer(float to, float from, float lerpTime) {
        float time = 0.0f;
        while (time < lerpTime) {
            time += Time.deltaTime;
            
            animator.SetLayerWeight(1, Mathf.Lerp(from, to, time / lerpTime));
            yield return null;
        }

        animator.SetLayerWeight(1, to);
    }
}