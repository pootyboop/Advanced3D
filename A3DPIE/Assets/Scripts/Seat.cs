using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seat : MonoBehaviour, IInteractable
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
            return EInteractionType.SEAT;
        }
    }

    public bool targetable
    {
        get
        {
            return !occupied;
        }
    }

    public string name = "Seat";
    public Transform seatPosition;
    public bool occupied = false;



    void Start()
    {
        //default seat info when none is set
        if (seatPosition == null)
        {
            seatPosition = transform;
        }
    }



    public void Interact()
    {
        Sit();
    }



    public void Sit()
    {
        //stand up from current seat
        if (PlayerMovement.instance.state == EPlayerState.SEATED)
        {
            //just set this seat to unoccupied rather than standing then sitting again
            //otherwise, our "standing" position to return to when we stand is set to wherever the player is sitting right now
            PlayerMovement.instance.currentSeat.occupied = false;
        }

        occupied = true;

        PlayerMovement.instance.SetPlayerState(EPlayerState.SEATED);
        PlayerMovement.instance.currentSeat = this;
        PlayerMovement.instance.transform.SetParent(seatPosition);
        PlayerMovement.instance.transform.localPosition = new Vector3(0f, 0f, 0f);
    }



    public void Stand()
    {
        occupied = false;

        PlayerMovement.instance.transform.SetParent(null);
        PlayerMovement.instance.SetPlayerState(EPlayerState.MOVABLE);
    }
}
