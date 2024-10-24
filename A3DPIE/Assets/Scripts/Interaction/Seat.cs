using System.Collections;
using System.Collections.Generic;
using UnityEngine;



//seat that the player and characters can sit in
//player sitting logic is handled here while character sitting logic is in Character
public class Seat : MonoBehaviour, IInteractable
{
    public string interactionName
    {
        get
        {
            return name;
        }
    }

    //always a seat
    public EInteractionType interactionType
    {
        get
        {
            return EInteractionType.SEAT;
        }
    }

    //doesn't cost anything to sit in a seat
    public int kartetCost
    {
        get
        {
            return 0;
        }
    }

    //only targetable if nobody's sitting in the seat already
    public bool targetable
    {
        get
        {
            return !occupied;
        }
    }

    public string name = "Seat";
    public Transform seatPosition;  //where characters sit relative to
    public bool occupied = false;



    void Start()
    {
        //use default transform for seat info when none is set
        if (seatPosition == null)
        {
            seatPosition = transform;
        }
    }



    //occupy seat when interacted with
    public void Interact()
    {
        Sit();
    }



    //seats don't care if you look at them
    public void OnTargetedChanged(bool isTargeting)
    {
    }



    //put the player in the seat
    public void Sit()
    {
        //stand up from current seat if sitting
        if (PlayerMovement.instance.state == EPlayerState.SEATED)
        {
            //just set this seat to unoccupied rather than standing then sitting again
            //otherwise, our "standing" position to return to when we stand is set to wherever the player is sitting right now
            PlayerMovement.instance.currentSeat.occupied = false;
        }

        occupied = true;

        //snap to seat
        //PlayerMovement and CameraController handle the movement and camera for sitting
        PlayerMovement.instance.SetPlayerState(EPlayerState.SEATED);
        PlayerMovement.instance.currentSeat = this;
        CameraController.instance.transform.SetParent(seatPosition);
        CameraController.instance.transform.localPosition = new Vector3(0f, 0f, 0f);
    }



    //remove the player from this seat
    public void Stand()
    {
        occupied = false;

        //return to MOVABLE, which we assume the player was prior to sitting
        CameraController.instance.transform.SetParent(CameraFollow.instance.transform);
        CameraController.instance.transform.localPosition = new Vector3(0f, 0f, 0f);
        PlayerMovement.instance.SetPlayerState(EPlayerState.MOVABLE);
    }
}
