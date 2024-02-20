using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



//controls/manages the default player camera and targeting/interacting with interactables
//does not control the cinematic cutscene camera
public class CameraController : MonoBehaviour
{
    public static CameraController instance;

    public Image reticle;   //dot in the center of the screen used to help the player target interactables
    public Transform grabPoint; //where GrabbableObjects snap to when picked up

    public float sensitivity = 300.0f;  //camera sensitivity
    public Vector2 rot = new Vector2(0f, 0f);   //current rotation
    public bool canRotateView;  //whether the camera accepts mouse input for rotation
    float interactionRange = 4.0f;  //how far the player can interact with objects from

    public IInteractable targetInteractable;    //the currently-aimed-at interactable



    void Start()
    {
        instance = this;

        //hide and lock mouse cursor
        SetMouseVisibility(false, true);
    }



    //called from PlayerMovement to ensure the player and camera stay in sync
    public void Setup()
    {
        transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
        rot = new Vector2(transform.eulerAngles.x, transform.eulerAngles.y);
    }



    //called from PlayerMovement.Update() rather than this script using its own Update() to avoid some weird camera movement issues
    public void SyncedUpdate()
    {
        //don't do anything if we can't look around
        //should never return here since PlayerMovement also makes this check...
        //...but it's here just in case something is done with the camera independently of the player
        if (!canRotateView)
        {
            return;
        }



        Look();
        CheckInteractable();
    }



    //rotate camera based on mouse position
    void Look()
    {
        Vector2 mousePos = new Vector2(0.0f, 0.0f);

        //get the mouse input scaled by time and sensitivity
        mousePos.x = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensitivity;
        mousePos.y = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensitivity;

        //rotate mouse input in a weird but necessary way for expected behavior
        rot.y += mousePos.x;
        //this clamp prevents the camera from rotating past straight up or straight down, which would disorient the player
        rot.x = Mathf.Clamp(rot.x - mousePos.y, -89.9f, 89.9f);

        //set the new camera rotation
        transform.rotation = Quaternion.Euler(rot.x, rot.y, 0.0f);
    }



    //check if an object is interactable and target it if so
    void CheckInteractable()
    {
        //raycast directly forward (where the reticle is) until reaching interactionRange distance
        Ray ray = new Ray(transform.position, transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hitInfo, interactionRange))
        {
            //if the hit object is interactable...
            if (hitInfo.collider.gameObject.TryGetComponent(out IInteractable hitInteractableObject))
            {
                //target it!
                TryTargetInteractable(hitInteractableObject);
            }

            //otherwise, it isn't interactable
            else
            {
                //if we're currently targeting something, we aren't looking at it anymore, so stop targeting it
                StopTargetInteractable();
            }
        }

        //nothing was hit. the player is looking off into the sky or there's nothing in their immediate view
        else
        {
            //so stop targeting anything that was targeted before
            StopTargetInteractable();
        }
    }



    //try to target an interactable
    void TryTargetInteractable(IInteractable interactable)
    {
        //don't re-target the currently targeted interactable if still looking at it
        if (interactable != targetInteractable)
        {
            //if it's a new interactable, stop targeting the old one
            StopTargetInteractable();

            //finally, check if this interactable actually allows us to target it
            if (interactable.targetable)
            {
                //if we can, then we can sucessfully target it
                StartTargetInteractable(interactable);
            }
        }
    }



    //forcefully targets an interactable
    //make checks before this function. it does not respect whether the interactable is targetable or not, it just targets it
    //it also does not tell the previous interactable that we stopped targeting it, so do that separately
    void StartTargetInteractable(IInteractable newInteractable)
    {
        targetInteractable = newInteractable;
        UI.instance.ShowInteractionText(newInteractable);   //display the UI prompt for interaction

        targetInteractable.OnTargetedChanged(true); //tell the new interactable we targeted it
    }



    //stops targeting an interactable and tells it we did so
    //safe to call whenever, even if not currently targeting something
    public void StopTargetInteractable()
    {
        //this function will be called in cases where there is no current targeted interactable...
        //...so do nothing if not currently targeting an interactable
        if (targetInteractable == null)
        {
            return;
        }

        //tell the current target interactable we stopped targeting it
        targetInteractable.OnTargetedChanged(false);

        targetInteractable = null;
        UI.instance.HideInteractionText();  //get rid of the UI interaction prompt
    }



    //try to interact with the target interactable
    public void TryInteract()
    {
        //first check if there even is a target interactable
        if (targetInteractable != null)
        {
            //if so, get rid of the interaction prompt since we have now done what it suggested
            UI.instance.HideInteractionText();
            //and interact with the target interactable. it will handle the interaction from here
            targetInteractable.Interact();
        }
    }



    //toggles the mouse cursor visibility and whether or not the player can move the camera
    public void SetMouseVisibility(bool isVisible, bool canControlCharacterView)
    {
        Cursor.visible = isVisible;
        if (isVisible)
        {
            Cursor.lockState = CursorLockMode.None;
        }

        else
        {
            Cursor.lockState = CursorLockMode.Locked;
        }

        canRotateView = canControlCharacterView;
        reticle.gameObject.SetActive(canRotateView);    //hide reticle when unable to control camera
    }



    //sets the mouse sensitivity
    public void SetMouseSensitivity(float newSensitivity)
    {
        sensitivity = newSensitivity;
    }
}
