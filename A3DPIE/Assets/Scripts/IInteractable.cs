using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EInteractionType
{
    GENERIC,
    DIALOGUE,
    READABLE
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

    void Interact();
}