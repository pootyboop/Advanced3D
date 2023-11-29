using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;



public class PlayerMovement : MonoBehaviour
{

    //==================================================================VARIABLES==================================================================\\

    //references
    CharacterController charController;
    public Camera cam;
    public GameObject clothCollision;               //collides with cloths (e.g. cloths on doors) since CharacterController doesn't have a referenceable capsule collider
                                                    //probably could've done this with collision channels but whatever
    private CapsuleCollider clothCapsule;           //the actual collider

    //public movement
    public float moveSpeed = 5.0f;
    public float gravity = 9.81f;
    public bool canJump = true;
    public float jumpHeight = 1.0f;

    //private movement
    private bool isMoving = false;
    private bool isGrounded = true;
    private float groundedTimer;
    private float verticalVelocity;
    private float previousVerticalVelocity;

    //=============================================================================================================================================\\



    void Start()
    {
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

        cam.GetComponent<CameraController>().Setup();

        charController = gameObject.GetComponent<CharacterController>();
    }



    void Update()
    {
        //check if the player tried to interact with a character
        UpdateInteractions();

        UpdateGrounded();
        Move();

        //this will be used next Update() for calculating the new verticalVelocity
        previousVerticalVelocity = verticalVelocity;
    }



    private void OnTriggerEnter(Collider other)
    {
        switch (other.gameObject.tag)
        {

        }
    }



    private void OnTriggerExit(Collider other)
    {
        switch (other.gameObject.tag)
        {

        }
    }



    void UpdateInteractions()
    {
        //player trying to interact?
        if (Input.GetButtonDown("Interact"))
        {
        
        }
    }



    void UpdateGrounded()
    {
        //charController knows isGrounded before this script does
        //so when they disagree, we can use it for events for leaving the ground and landing

        if (!charController.isGrounded && isGrounded)
        {

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



    private void OnLanded()
    {

    }



    void Move()
    {
        //credit for GetDesiredMvmt() and GetJumpHeight():
        //https://youtu.be/7kGCrq1cJew
        //https://forum.unity.com/threads/how-to-correctly-setup-3d-character-movement-in-unity.981939/#post-6379746

        Vector3 move;

        //X AND Z MOVEMENT
        move = GetDesiredMvmt();

        move = ApplySpeedModifiers(move);

        //Y MOVEMENT
        move.y = GetJumpHeight();

        //and finally, actually move
        charController.Move(move * Time.deltaTime);
    }



    Vector3 GetDesiredMvmt()
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

        UpdateIsMoving(horizontal, vertical);

        //multiply normalized vectors by player movement input (meaning the character moves relative to its rotation)
        Vector3 forwardRelativeInput = forward * vertical;
        Vector3 rightRelativeInput = right * horizontal;

        //and when all that's done, multiply by moveSpeed
        return (forwardRelativeInput + rightRelativeInput) * moveSpeed;
    }



    //TLDR: lets the footstep manager know if we're moving
    void UpdateIsMoving (float horizontal, float vertical)
    {
        if (!isMoving && (vertical != 0.0f || horizontal != 0.0f))
        {
            isMoving = true;
        }

        else if (isMoving && vertical == 0.0f && horizontal == 0.0f)
        {

            isMoving = false;
        }
    }



    //fairly straightforward, just take the modifiers into account
    Vector3 ApplySpeedModifiers(Vector3 move)
    {
        return move;
    }



    float GetJumpHeight()
    {
        //prevent bouncing
        if (isGrounded && verticalVelocity < 0)
        {
            verticalVelocity = 0f;
        }

        //take gravity into account
        verticalVelocity -= gravity * Time.deltaTime;


        //if player is allowed to jump
        if (Input.GetButtonDown("Jump") && groundedTimer > 0 && canJump)
        {

            groundedTimer = 0;

            //jump
            verticalVelocity += Mathf.Sqrt(jumpHeight * 2 * gravity);
        }

        return verticalVelocity;
    }
}