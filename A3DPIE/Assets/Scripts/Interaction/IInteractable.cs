using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public enum EInteractionType
{
    GENERIC,
    DIALOGUE,
    READABLE,
    GRABBABLE,
    SEAT,
    DOOR
}



public interface IInteractable
{
    string interactionName
    {
        get;
    }

    EInteractionType interactionType
    {
        get;
    }

    bool targetable
    {
        get;
    }



    void Interact();

    void OnTargetedChanged(bool isTargeting);
}