using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



//handles the player's HUD, pause screen, and other screenspace canvas UI
public class UI : MonoBehaviour
{
    public static UI instance;

    public GameObject interactionBG, interactionName, interactionAction, cancelActionText, pauseScreen;    //interaction prompt references

    public Image fadeToBlackPanel;  //a huge black image that covers the screen used for fading to-from black
    public float fadeToBlackSpeed = 0.45f; //how fast to fade to/from black

    private bool paused;    //currently paused?
    private EPlayerState previousState = EPlayerState.MOVABLE;  //state to return player to after pausing

    private IEnumerator fadeToBlackCoroutine;


    void Start()
    {
        instance = this;
    }



    //displays a pop-up UI prompt to interact with an interactable...
    //...displaying the interactable's name, the type of interaction you can perform, and any currency it'll cost to interact with it
    public void ShowInteractionText(IInteractable interactable)
    {
        interactionBG.SetActive(true);

        //set the interaction UI name to the name of the interactable and some extra hyphens for   - Style Points -
        interactionName.GetComponent<TMPro.TextMeshProUGUI>().text = "- " + interactable.interactionName + " -";



        string interactionActionText;
        
        //free interaction (99% of interactions) so don't worry about showing the cost
        //just say what type of interaction this is (e.g. Talk, Read, Inspect)
        if (interactable.kartetCost == 0)
        {
            interactionActionText = GetInteractionText(interactable.interactionType) + " (E)";
        }

        //paid interaction, tell the player they will be buying something by interacting and the cost of the interaction
        else
        {
            //mark this interaction as a purchase and add the cost of the item
            interactionActionText = "Buy (E)" + //always say Buy so it's very clear this costs money
                " for <color=#FFDD00><b>" + //separate cost with the kartet (currency) UI color
                interactable.kartetCost +   //cost of interaction
                " <s>KT</s></b>";   //KT = kartet, as shown in the inventory UI, so this indicate the interaction will cost currency
        }

        //set the actual UI text to whatever we settled on
        interactionAction.GetComponent<TMPro.TextMeshProUGUI>().text = interactionActionText;
    }



    //display the text prompt for ending/stopping/cancelling an interaction
    public void ShowCancelText(IInteractable interactable)
    {
        if (interactable == null)
        {
            HideCancelText();
            return;
        }

        cancelActionText.SetActive(true);
        cancelActionText.GetComponent<TMPro.TextMeshProUGUI>().text = GetCancelText(interactable.interactionType) + " (Q)";

    }



    //get the interaction verb for the type of interactable being targeted
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
            case EInteractionType.SPACESHIPDEPART:
                return "Depart";
            case EInteractionType.INSPECTABLE:
                return "Inspect";
        }
    }



    //get the interaction verb for cancelling/ending the interaction with the current interactable
    private string GetCancelText(EInteractionType interactionType)
    {
        switch (interactionType)
        {
            default:
                return "Cancel";
            case EInteractionType.DIALOGUE:
                return "End Conversation";
            case EInteractionType.READABLE:
                return "Stop Reading";
            case EInteractionType.GRABBABLE:
                return "Drop";
            case EInteractionType.SEAT:
                return "Stand";
            case EInteractionType.DOOR:
                return "Close";
        }
    }



    //hide the interaction text
    //doesn't wipe text, which will be overwritten next time
    public void HideInteractionText()
    {
        interactionBG.SetActive(false);
    }



    public void HideCancelText()
    {
        cancelActionText.SetActive(false);
    }



    //fade camera to/from black
    public void FadeToBlack(bool fadeIn)
    {
        StopFadeToBlack();
        fadeToBlackCoroutine = FadeBlack(fadeIn);
        StartCoroutine(fadeToBlackCoroutine);
    }




    public void StopFadeToBlack() {
        if (fadeToBlackCoroutine != null) {
            StopCoroutine(fadeToBlackCoroutine);
        }
    }



    //coroutine for fading to/from black over time
    IEnumerator FadeBlack(bool fadeIn)
    {
        //fading in? start at full black
        if (fadeIn)
        {
            fadeToBlackPanel.color = MakeBlackWithAlpha(1.0f);
        }

        //fading out? start at no black
        else
        {
            fadeToBlackPanel.color = MakeBlackWithAlpha(0.0f);
        }



        while (
            (fadeToBlackPanel.color.a <= 1.0f && !fadeIn)   //not fully faded out
            ||
            (fadeToBlackPanel.color.a >= 0.0f && fadeIn)    //not fully faded in
            )
        {
            float newAddTime = fadeToBlackSpeed * Time.deltaTime;   //new time to scale the added alpha by, scaled by speed

            //remove alpha instead if fading in
            if (fadeIn)
            {
                newAddTime *= -1.0f;
            }

            //add/remove the new alpha amount
            SetFadeToBlack(fadeToBlackPanel.color.a + newAddTime);

            //wait a bit then go again
            yield return null;
        }

        fadeToBlackCoroutine = null;
    }



    public void SetFadeToBlack(float newAlpha) {
        fadeToBlackPanel.color = MakeBlackWithAlpha(newAlpha);
    }



    //converts alpha to a black Color with the given alpha
    Color MakeBlackWithAlpha(float alpha)
    {
        return new Color(0.0f, 0.0f, 0.0f, alpha);
    }



    //just switch paused state to whatever it currently isn't
    public void TogglePause()
    {
        SetPaused(!paused);
    }



    //open or close the pause screen
    public void SetPaused(bool newPause)
    {
        paused = newPause;
        if (paused)
        {
            previousState = PlayerMovement.instance.state;
            PlayerMovement.instance.SetPlayerState(EPlayerState.PAUSED);
            pauseScreen.SetActive(true);
            Time.timeScale = 0.0f;
        }

        else
        {
            Time.timeScale = 1.0f;
            pauseScreen.SetActive(false);
            PlayerMovement.instance.SetPlayerState(previousState);
        }
    }
}
