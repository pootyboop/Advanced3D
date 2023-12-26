using System.Collections;
using System.Collections.Generic;
using UnityEngine;



//types of interactions
//UI script interprets these and uses them to prompt the player
//e.g. DIALOGUE = "Talk [E]", READABLE = "Read [E]" on the UI
public enum EInteractionType
{
    GENERIC,    //Interact
    DIALOGUE,   //Talk
    READABLE,   //Read
    GRABBABLE,  //Grab/Pick up
    SEAT,       //Sit
    DOOR        //Door
}



//interface for all objects the player can interact with
public interface IInteractable
{
    //the name of this interactable
    //e.g. John, Door, Spaceship
    string interactionName
    {
        get;
    }

    //what type of interactable this is
    EInteractionType interactionType
    {
        get;
    }

    //cost of the interaction. used for purchasing interactable items (like drinks)
    int kartetCost
    {
        get;
    }

    //whether the player can target this object (see an interaction UI prompt when looking at this object)
    bool targetable
    {
        get;
    }



    //called when this object is interacted with
    void Interact();

    //called when this object is targeted or untargeted
    void OnTargetedChanged(bool isTargeting);
}