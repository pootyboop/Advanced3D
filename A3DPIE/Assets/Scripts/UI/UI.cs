using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI : MonoBehaviour
{
    public static UI instance;

    public GameObject interactionBG, interactionText;


    void Start()
    {
        instance = this;
    }



    public void ShowInteractionText(IInteractable interactable)
    {
        interactionBG.SetActive(true);

        //a little ugly but this keeps the interaction prompt all in one single TMPro. less public references
        string newInteractionText = "- " + interactable.interactionName + " -\n<size=70%><color=#FFC317>" + GetInteractionText(interactable.interactionType) + " (E)";
        interactionText.GetComponent<TMPro.TextMeshProUGUI>().text = newInteractionText;
    }



    private string GetInteractionText(EInteractionType interactionType)
    {
        switch (interactionType)
        {
            default:
                return "Interact";
            case EInteractionType.DIALOGUE:
                return "Talk";
            case EInteractionType.READABLE:
                return "Read";
            case EInteractionType.GRABBABLE:
                return "Grab";
            case EInteractionType.SEAT:
                return "Sit";
            case EInteractionType.DOOR:
                return "Open";
        }
    }



    public void HideInteractionText()
    {
        interactionBG.SetActive(false);
    }
}
