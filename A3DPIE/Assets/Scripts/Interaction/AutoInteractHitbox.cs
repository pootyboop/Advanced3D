using System.Collections;
using System.Collections.Generic;
using UnityEngine;



//adds an interactable to interaction hitboxes
//when the player enters these, they auto-interact with whatever's stored here
public class AutoInteractHitbox : MonoBehaviour
{
    //IInteractable interactionToTrigger; //this would be the public variable, but unity's inspector hates interfaces and won't display a field
    public GameObject interactable; //so i'm referencing a gameobject instead and just grabbing the component

    public void TriggerInteraction()
    {
        interactable.GetComponent<IInteractable>().Interact();
    }
}