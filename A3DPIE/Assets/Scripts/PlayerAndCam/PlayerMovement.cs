using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using UnityEngine;



public enum EPlayerState
{
    MOVABLE,
    CUTSCENE,
    DIALOGUE,
    SEATED,
    LADDER
}



public class PlayerMovement : MonoBehaviour
{

    //==================================================================VARIABLES==================================================================\\
    
    //singleton
    public static PlayerMovement instance;

    //references
    CharacterController charController;
    public Camera cam;
    private CameraController camController;
    private GrabbableObject grabbedObject;

    public EPlayerState state = EPlayerState.MOVABLE;    //what to do with movement input

    //public movement
    public float gravity = 9.81f;
    public float moveSpeed = 5.0f;
    public float ladderSpeed = 0.3f;
    public bool jumpEnabled = true;
    public float jumpHeight = 2.0f;

    //private movement
    private bool isMoving = false;
    private bool isGrounded = true;
    private float groundedTimer;
    private float verticalVelocity;
    private float previousVerticalVelocity;

    //misc
    public Seat currentSeat;
    private Vector3 preSeatedPosition;

    //=============================================================================================================================================\\



    void Start()
    {
        instance = this;
        SetupPlayer();
    }



    public void SetupPlayer()
    {

        /*
        //align to spawn point
        Transform spawnPoint = GameObject.FindGameObjectWithTag("SpawnPoint").transform;
        transform.position = spawnPoint.position;
        transform.rotation = spawnPoint.rotation;
        */

        camController = cam.GetComponent<CameraController>();
        camController.Setup();

        charController = gameObject.GetComponent<CharacterController>();

        //make sure all the proper setup for the state selected in inspector is executed
        SetPlayerState(state);
    }



    void Update()
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

        //check if the player tried to interact with a character
        UpdateInteractions();

        UpdateGrounded();
        UpdateMovement();
    }



    private void OnTriggerEnter(Collider other)
    {
        switch (other.gameObject.tag)
        {
            case "Ladder":
                SetPlayerState(EPlayerState.LADDER);
                break;
        }
    }



    private void OnTriggerExit(Collider other)
    {
        switch (other.gameObject.tag)
        {
            case "Ladder":
                SetPlayerState(EPlayerState.MOVABLE);
                break;
        }
    }



    void UpdateInteractions()
    {
        //player trying to interact?
        if (Input.GetButtonDown("Interact"))
        {
            switch (state)
            {
                case EPlayerState.MOVABLE:
                case EPlayerState.LADDER:
                case EPlayerState.SEATED:
                    camController.TryInteract();
                    break;
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
            if (state == EPlayerState.SEATED)
            {
                currentSeat.Stand();
                currentSeat = null;
            }

            else if (state == EPlayerState.LADDER)
            {
                SetPlayerState(EPlayerState.MOVABLE);
            }

            else if (grabbedObject != null)
            {
                grabbedObject.Drop();
            }
        }
    }



    void UpdateGrounded()
    {
        //charController knows isGrounded before this script does
        //so when they disagree, we can use it for events for leaving the ground and landing

        if (!charController.isGrounded && isGrounded)
        {
            OnLeftGround();
        }

        else if (charController.isGrounded && !isGrounded)
        {
            OnLanded();
        }

        //now that the events are done they should be equal
        isGrounded = charController.isGrounded;


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
    }



    void OnLeftGround()
    {
        if (verticalVelocity < 0.0f)
        {
            verticalVelocity = 0.0f;
        }
    }



    void OnLanded()
    {

    }



    public void EnterCutscene()
    {
        SetPlayerState(EPlayerState.CUTSCENE);
    }



    public void ExitCutscene()
    {
        SetPlayerState(EPlayerState.MOVABLE);
    }



    //switch player state between stuff like normal movement, climbing ladder, sitting down, in dialogue...
    //state should NOT be set outside of this function. this is the correct way to update state
    public void SetPlayerState(EPlayerState newState)
    {
        //PREVIOUS STATE
        switch (state)
        {
            case EPlayerState.SEATED:
                transform.position = preSeatedPosition;
                break;
            case EPlayerState.CUTSCENE:
                Inventory.instance.SetVisibility(true);
                camController.gameObject.SetActive(true);
                break;
            default:
                break;
        }

        state = newState;

        //NEW STATE
        switch (state)
        {
            case EPlayerState.CUTSCENE:
                Inventory.instance.SetVisibility(true);
                camController.SetMouseVisibility(false, false);
                camController.gameObject.SetActive(false);
                break;
                break;
            case EPlayerState.DIALOGUE:
                camController.SetMouseVisibility(true, false);
                break;
            case EPlayerState.MOVABLE:
            case EPlayerState.LADDER:
                camController.SetMouseVisibility(false, true);
                break;
            case EPlayerState.SEATED:
                preSeatedPosition = transform.position;
                camController.SetMouseVisibility(false, true);
                break;
        }
    }



    void UpdateMovement()
    {
        Vector3 move = new Vector3(0.0f, 0.0f, 0.0f);

        switch (state)
        {
            case EPlayerState.MOVABLE:
                move = Move();
                break;

            case EPlayerState.LADDER:
                move = Ladder();
                break;
        }

        //if any movement has been made
        if (move != Vector3.zero)
        {
            isMoving = true;
            charController.Move(move * Time.deltaTime);
        }

        else
        {
            isMoving = false;
        }

        //used next update for calculating the new verticalVelocity
        previousVerticalVelocity = verticalVelocity;
    }



    private Vector3 Move()
    {
        Vector3 move = GetDesiredMvmt();

        //credit for GetDesiredMvmt() and GetJumpHeight():
        //https://youtu.be/7kGCrq1cJew
        //https://forum.unity.com/threads/how-to-correctly-setup-3d-character-movement-in-unity.981939/#post-6379746

        //X AND Z MOVEMENT
        move = ApplySpeedModifiers(move);

        //Y MOVEMENT
        move.y = JumpGravity();

        return move;
    }



    private Vector3 GetDesiredMvmt()
    {
        //get the forward and right vectors
        Vector3 forward = Camera.main.transform.forward;
        Vector3 right = Camera.main.transform.right;

        //just take the x value
        forward.y = 0.0f;
        right.y = 0.0f;

        //normalize
        forward = forward.normalized;
        right = right.normalized;

        float vertical = Input.GetAxis("Vertical");
        float horizontal = Input.GetAxis("Horizontal");

        //multiply normalized vectors by player movement input (meaning the character moves relative to its rotation)
        Vector3 forwardRelativeInput = forward * vertical;
        Vector3 rightRelativeInput = right * horizontal;

        //and when all that's done, multiply by moveSpeed
        return (forwardRelativeInput + rightRelativeInput) * moveSpeed;
    }



    private Vector3 Ladder()
    {
        Vector3 move = new Vector3(0.0f, 0.0f, 0.0f);

        //jump to leave ladder, even if not grounded
        if (Input.GetButtonDown("Jump"))
        {
            move.y = JumpGravity();
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



    //fairly straightforward, just take the modifiers into account
    private Vector3 ApplySpeedModifiers(Vector3 move)
    {
        return move;
    }



    private float JumpGravity()
    {
        //gravity
        switch (state)
        {
            //when to apply gravity
            case EPlayerState.MOVABLE:
            case EPlayerState.DIALOGUE:
            case EPlayerState.CUTSCENE:

                //prevent bouncing
                if (isGrounded && verticalVelocity < 0)
                {
                    verticalVelocity = -10f;
                }

                verticalVelocity -= gravity * Time.deltaTime;
                break;
        }



        //if player is allowed to jump
        if (
            Input.GetButtonDown("Jump") &&                          //pressing jump button?
            jumpEnabled &&                                          //jumping enabled?
            (groundedTimer > 0.0f || state == EPlayerState.LADDER)      //grounded OR on ladder?
            )
        {
            //jump
            groundedTimer = 0.0f;
            verticalVelocity = Mathf.Sqrt(jumpHeight * gravity);    //reset verticalVelocity
        }

        return verticalVelocity;
    }



    public void GrabObject(GrabbableObject grabbableObject)
    {
        grabbedObject = grabbableObject;
    }
}