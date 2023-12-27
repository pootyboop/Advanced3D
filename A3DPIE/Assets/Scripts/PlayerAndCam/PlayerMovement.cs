using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using UnityEngine;



//current movement/action state of the player
//used for state machine
public enum EPlayerState
{
    MOVABLE,
    CUTSCENE,
    DIALOGUE,
    SEATED,
    LADDER
}

/*
    Part of PlayerMovement.UpdateGrounded() is based on a Unity forums post.
    Author: Kurt-Dekker
    https://forum.unity.com/threads/how-to-correctly-setup-3d-character-movement-in-unity.981939/#post-6379746
    Accessed: 2/12/2023
*/



//controls player movement and generic behavior related to the player
public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement instance;

    //references from prefab
    CharacterController charController; //the unity CharacterController used for collision and Move()
    public Camera cam;  //player camera
    private CameraController camController; //camera controller script

    //public movement
    public EPlayerState state = EPlayerState.MOVABLE;    //used to determine what to do with movement input and interaction input
    public float gravity = 9.81f;   //fall speed
    public float moveSpeed = 5.0f;
    public float ladderSpeed = 0.3f;    //ladder climbing speed - INVERTED! reduce this number to increase speed
    public bool jumpEnabled = true; //this is never changed by script. as this game has no platforming, toggle in the inspector if jumping is not intended in the final game
    public float jumpHeight = 2.0f;

    //private movement
    private bool isMoving = false;
    private bool isGrounded = true;
    private float groundedTimer;    //used for coyote time and grounding the player without bouncing
    private float verticalVelocity; //current y velocity
    private float previousVerticalVelocity; //last frame's y velocity

    //interaction
    public Seat currentSeat;
    private Vector3 preSeatedPosition;  //player's previous standing position from before they sat
    private GrabbableObject grabbedObject;  //currently held grabbableobject



    void Start()
    {
        instance = this;
        SetupPlayer();  //kept separate from Start in case this needs to be moved to Awake or delayed later
    }



    //essentially Start, just pulled out so it's easier to move around
    public void SetupPlayer()
    {
        camController = cam.GetComponent<CameraController>();
        camController.Setup();  //call setup from here so cam and player stay synced

        charController = gameObject.GetComponent<CharacterController>();

        //make sure all the proper setup for the state selected in inspector is executed
        SetPlayerState(state);
    }



    void Update()
    {
        //move camera to mouse position if necessary
        UpdateCamera();

        //check if the player tried to interact or perform any other non-movement action
        UpdateInteractions();

        //make sure the player's grounded state is up to date
        UpdateGrounded();

        //move
        UpdateMovement();
    }



    private void OnTriggerEnter(Collider other)
    {
        switch (other.gameObject.tag)
        {
            //overlap ladder's hitbox to climb it
            case "Ladder":
                SetPlayerState(EPlayerState.LADDER);
                break;
            //overlap a loadable area to tell AreaLoadManager where we entered and to manage loaded areas as necessary
            case "LoadArea":
                AreaLoadManager.instance.EnteredAreaHitbox(other.gameObject);
                break;
            //overlap a hitbox which automatically triggers an interaction
            case "AutoInteractHitbox":
                other.gameObject.GetComponent<AutoInteractHitbox>().TriggerInteraction();
                break;
        }
    }



    private void OnTriggerExit(Collider other)
    {
        switch (other.gameObject.tag)
        {
            //leave a ladder to stop climbing it
            case "Ladder":
                SetPlayerState(EPlayerState.MOVABLE);
                break;
        }
    }



    //calls CameraController.SyncedUpdate() if necessary
    //won't call it if unable to move camera
    void UpdateCamera()
    {
        switch (state)
        {
            //EPlayerStates that don't require updating the camera
            case EPlayerState.DIALOGUE:
            case EPlayerState.CUTSCENE:
                break;
            default:
                camController.SyncedUpdate();
                break;
        }
    }



    //respond to the player's interaction/other inputs
    void UpdateInteractions()
    {
        //player trying to interact?
        if (Input.GetButtonDown("Interact"))
        {
            switch (state)
            {
                //interact with things while default, on a ladder, or seated
                case EPlayerState.MOVABLE:
                case EPlayerState.LADDER:
                case EPlayerState.SEATED:
                    camController.TryInteract();
                    break;
                //in dialogue, interact to continue
                case EPlayerState.DIALOGUE:
                    DialogueManager.instance.NextDialogue(true);
                    break;
                default:
                    break;
            }
        }

        //player trying to drop/cancel?
        if (Input.GetButtonDown("Drop"))
        {
            //stop sitting if seated
            if (state == EPlayerState.SEATED)
            {
                currentSeat.Stand();
                currentSeat = null;
            }

            //drop from ladder if on ladder
            else if (state == EPlayerState.LADDER)
            {
                SetPlayerState(EPlayerState.MOVABLE);
            }

            //drop grabbed object if holding one
            else if (grabbedObject != null)
            {
                grabbedObject.Drop();
            }
        }
    }



    //update the player's grounded state
    void UpdateGrounded()
    {
        //charController knows if the player is grounded or not before this script does
        //so when they disagree, use it for events for leaving the ground and landing

        //jumped or otherwise left the ground. now in the air
        if (!charController.isGrounded && isGrounded)
        {
            OnLeftGround();
        }

        //landed on the ground. now grounded
        else if (charController.isGrounded && !isGrounded)
        {
            OnLanded();
        }

        //now that the events are done they should be equal
        isGrounded = charController.isGrounded;

        //Kurt-Dekker's code begins here.
        if (isGrounded)
        {
            //this timer is for coyote time for gliding and jumping
            //mainly helps with slopes so the character walks naturally downhill
            groundedTimer = 0.2f;
        }

        if (groundedTimer > 0)
        {
            groundedTimer -= Time.deltaTime;
        }
        //Kurt-Dekker's code ends here.
    }



    //called when jumping or otherwise leaving the ground
    void OnLeftGround()
    {
        //reset velocity if the player was being pulled to the ground (which is done to take slopes smoothly)
        if (verticalVelocity < 0.0f)
        {
            verticalVelocity = 0.0f;
        }
    }



    //called when landing from midair
    void OnLanded()
    {

    }



    //enter cutscene state
    //called from cutscene
    public void EnterCutscene()
    {
        SetPlayerState(EPlayerState.CUTSCENE);
    }



    //exit cutscene state
    //called from cutscene
    public void ExitCutscene()
    {
        SetPlayerState(EPlayerState.MOVABLE);
    }



    //player movement/action state machine
    //switch player state between stuff like normal movement, climbing ladder, sitting down, in dialogue...
    //state should NOT be set outside of this function. this is the correct way to update state
    public void SetPlayerState(EPlayerState newState)
    {
        //PREVIOUS STATE
        switch (state)
        {
            case EPlayerState.SEATED:
                //do NOT leave the seat if talking while seated
                if (newState != EPlayerState.DIALOGUE)
                {
                    transform.position = preSeatedPosition;
                }
                break;
            //after cutscene, make inventory visible again and enable camera
            case EPlayerState.CUTSCENE:
                Inventory.instance.SetVisibility(true);
                cam.gameObject.SetActive(true);
                break;
            default:
                break;
        }

        state = newState;

        //NEW STATE
        switch (state)
        {
            //before cutscene, hide inventory and disable camera
            case EPlayerState.CUTSCENE:
                Inventory.instance.SetVisibility(true);
                camController.SetMouseVisibility(false, false);
                cam.gameObject.SetActive(false);
                break;
                break;
            case EPlayerState.DIALOGUE:
                camController.SetMouseVisibility(true, false);
                break;
            case EPlayerState.MOVABLE:
            case EPlayerState.LADDER:
                camController.SetMouseVisibility(false, true);
                break;
            //quickly save the player's position before snapping to seat
            case EPlayerState.SEATED:
                preSeatedPosition = transform.position;
                camController.SetMouseVisibility(false, true);
                break;
        }
    }



    //move the player according to their current movement state
    void UpdateMovement()
    {
        //used by all movement functions
        Vector3 move = new Vector3(0.0f, 0.0f, 0.0f);

        //get movement vector by state
        switch (state)
        {
            //REGULAR MOVEMENT
            case EPlayerState.MOVABLE:
                move = Move();
                break;

            //CLIMBING LADDER
            case EPlayerState.LADDER:
                move = Ladder();
                break;

            //OTHERWISE, DO NOT MOVE
            default:
                break;
        }

        //if any movement has been made...
        if (move != Vector3.zero)
        {
            //move by the move vector
            isMoving = true;
            charController.Move(move * Time.deltaTime);
        }

        else
        {
            isMoving = false;
        }

        //used next update for calculating the difference in velocity between frames
        previousVerticalVelocity = verticalVelocity;
    }



    //returns a vector for regular movement with speed modifiers and jump included
    //does not the vector by time, UpdateMovement() does that instead
    private Vector3 Move()
    {
        //X AND Z MOVEMENT
        Vector3 move = ApplySpeedModifiers(GetDesiredMvmt());

        //Y MOVEMENT
        move.y = SolveYMovement();

        return move;
    }



    //makes the raw left-right-forward-backward regular movement from WASD/joystick/etc input...
    //...into a camera-rotation-relative vector for the player to move in
    //does not apply speed modifiers or jump
    private Vector3 GetDesiredMvmt()
    {
        //get the camera's forward and right vectors, normalized WITHOUT the Y value so the player moves on the X and Z axes
        Vector3 forward = StripYFromNormalizedVector(cam.transform.forward);
        Vector3 right = StripYFromNormalizedVector(cam.transform.right);

        //multiply by player movement input (meaning the character moves relative to its rotation)
        forward *= Input.GetAxis("Vertical");
        right *= Input.GetAxis("Horizontal");

        //combine into one final vector, multiply by movement speed, and we're done
        return (forward + right) * moveSpeed;
    }



    //remove Vector3.y and normalize to prevent the player from floating off the ground when looking slightly upward
    private Vector3 StripYFromNormalizedVector(Vector3 vectorIn)
    {
        vectorIn.y = 0.0f;  //remove the Y value so the player can't move upward (without jumping)
        return vectorIn.normalized; //re-normalize without the Y value
    }



    //apply any movement speed modifiers
    //add any non-core movement speed modifiers here
    private Vector3 ApplySpeedModifiers(Vector3 move)
    {
        return move;
    }



    //determine what the player's current Y velocity should be...
    //...by taking jump input, gravity, and grounded state into account
    //DO NOT call more than once per Update()
    private float SolveYMovement()
    {
        //GRAVITY/GROUNDED
        switch (state)
        {
            //states to apply gravity in
            //this switch will never be reached if the player's state does not move using SolveYMovement()
            case EPlayerState.MOVABLE:
            case EPlayerState.DIALOGUE:
            case EPlayerState.CUTSCENE:

                //prevent bouncing by pulling the player toward the ground
                //this helps with downward slope movement
                if (isGrounded && verticalVelocity < 0)
                {
                    verticalVelocity = -10f;
                }

                //apply gravity
                verticalVelocity -= gravity * Time.deltaTime;
                break;

            default:
                break;
        }



        //JUMPING
        if (
            Input.GetButtonDown("Jump") &&                          //pressing jump button?
            jumpEnabled &&                                          //jumping enabled?
            (groundedTimer > 0.0f || state == EPlayerState.LADDER)      //grounded OR on ladder? (you can jump off of a ladder without being grounded - handled in Ladder())
            )
        {
            //jump
            groundedTimer = 0.0f;   //reset grounded timer

            //"reset" verticalVelocity by setting it to jump height
            //this way you don't have less of a jump if other factors screwed with verticalVelocity
            verticalVelocity = Mathf.Sqrt(jumpHeight * gravity);
        }

        //finally, return verticalVelocity. you can technicalyl access this globally anyway but this is more readable
        return verticalVelocity;
    }



    //move up or down a ladder
    //jump off to stop climbing and return to regular movement
    private Vector3 Ladder()
    {
        Vector3 move = new Vector3(0.0f, 0.0f, 0.0f);

        //jump to leave ladder, even if not grounded
        //will perform a regular-height jump
        if (Input.GetButtonDown("Jump"))
        {
            //jump
            move.y = SolveYMovement();

            //stop climbing ladder
            SetPlayerState(EPlayerState.MOVABLE);
            return move;
        }

        //W to go up
        else if (Input.GetAxis("Vertical") > 0)
        {
            move = Vector3.up / ladderSpeed;
            return move;
        }

        //S to go down
        else if (Input.GetAxis("Vertical") < 0)
        {
            move = Vector3.down / ladderSpeed;
        }

        return move;
    }



    //store a reference to the currently grabbed GrabbableObject
    //most GrabbableObject code for player grabbing is handled on CameraController
    public void GrabObject(GrabbableObject grabbableObject)
    {
        grabbedObject = grabbableObject;
    }
}