using System.Collections;
using System.Collections.Generic;
using UnityEngine;



//physics object that the player can pick up and drop
[RequireComponent(typeof(Rigidbody))]
public class GrabbableObject : MonoBehaviour, IInteractable
{

    public string interactionName
    {
        get
        {
            return name;
        }
    }

    public EInteractionType interactionType
    {
        get
        {
            return EInteractionType.GRABBABLE;
        }
    }

    //first pickup can cost some amount of money
    //e.g. paying for a drink at the bar
    public int kartetCost
    {
        get
        {
            return itemCost;
        }
    }


    //only targetable if not currently grabbed AND if the player isn't holding something already
    public bool targetable
    {
        get
        {
            return (!grabbed && !PlayerMovement.instance.HasGrabbableObject());
        }
    }

    public string name;
    public int itemCost = 0; //the cost of the item in kartet when first grabbed
    public DialogueCharacter dialogueOnFirstGrab;   //begin a referenced conversation when the item is first grabbed
    private bool paid = false;  //whether the player has paid the cost for this item or not

    private bool grabbed = false;

    Rigidbody rb;
    Collider collider;



    void Start()
    {
        rb = GetComponent<Rigidbody>();
        collider = GetComponent<Collider>();
    }



    //grab or drop an object when interacted with
    public void Interact()
    {
        //this should technically never be called
        //the player should not be able to grab what they're already grabbing
        //if they somehow do, just drop the object
        if (grabbed)
        {
            Drop();
        }

        //this path should always execute
        else
        {
            Grab();
        }
    }



    //grabbable objects don't care whether you're looking at them or not
    public void OnTargetedChanged(bool isTargeting)
    {
    }



    //pick up the object and hold it in the player's hand
    public void Grab()
    {
        grabbed = true;

        //if on the conveyor belt, get off
        TryLeaveConveyorBelt();

        //pay for the item
        //once an item's cost has been paid, the player "owns" it...
        //...so they do not need to pay again if they drop it and re-grab it
        if (!paid && itemCost > 0)
        {
            Inventory.instance.PayKartet(itemCost);
            paid = true;
            itemCost = 0;   //set this to 0 so IInteractable doesn't think it still costs money
        }

        //the item is picked up with no cost to the player
        else
        {
            AudioManager.instance.PlayAudioByTag("pickup");
        }

        //disable physics/collisions
        rb.isKinematic = true;
        collider.enabled = false;

        //put the object in the player's "hand": a transform offset from the camera
        transform.SetParent(CameraController.instance.grabPoint, false);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.Euler(Vector3.zero);

        //PlayerMovement takes it from here
        PlayerMovement.instance.GrabObject(this);

        //on first grab, start conversation with a specified dialogue character if there is one
        if (dialogueOnFirstGrab != null)
        {
            dialogueOnFirstGrab.Interact();
            dialogueOnFirstGrab = null;
        }
    }



    //stop holding the item
    public void Drop()
    {
        grabbed = false;

        //the item drops from the position it was held at
        //rather than snapping to the floor or anything like that
        transform.SetParent(null);

        //re-enable physics/collisions
        rb.isKinematic = false;
        collider.enabled = true;

        //clear the player's grabbed object
        PlayerMovement.instance.GrabObject(null);
    }



    //if this grabbable object has a MoveAlongSpline component...
    //...free it from the conveyor belt it's following
    private void TryLeaveConveyorBelt()
    {
        //leave conveyor belt if currently on it
        MoveAlongSpline moveAlongSpline = gameObject.GetComponent<MoveAlongSpline>();
        if (moveAlongSpline != null)
        {
            moveAlongSpline.barConveyorBelt.LeaveConveyorBelt(gameObject);
        }
    }
}
