using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    public static UI instance;

    public GameObject interactionBG, interactionName, interactionAction;
    public Image fadeToBlackPanel;
    private float fadeToBlackSpeed = 0.45f;


    void Start()
    {
        instance = this;
    }



    public void ShowInteractionText(IInteractable interactable)
    {
        interactionBG.SetActive(true);

        interactionName.GetComponent<TMPro.TextMeshProUGUI>().text = "- " + interactable.interactionName + " -";



        string interactionActionText;
        
        if (interactable.kartetCost == 0)
        {
            interactionActionText = GetInteractionText(interactable.interactionType) + " (E)";
        }

        else
        {
            //mark this interaction as a purchase and add the cost of the item
            interactionActionText = "Buy (E)" +
                " for <color=#FFDD00><b>" +
                interactable.kartetCost +
                " <s>KT</s></b>";
        }

        interactionAction.GetComponent<TMPro.TextMeshProUGUI>().text = interactionActionText;
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



    public void FadeToBlack(bool fadeIn)
    {
        StartCoroutine(FadeBlack(fadeIn));
    }



    IEnumerator FadeBlack(bool fadeIn)
    {
        if (fadeIn)
        {
            fadeToBlackPanel.color = MakeBlackWithAlpha(1.0f);
        }

        else
        {
            fadeToBlackPanel.color = MakeBlackWithAlpha(0.0f);
        }

        while (
            (fadeToBlackPanel.color.a <= 1.0f && !fadeIn)
            ||
            (fadeToBlackPanel.color.a >= 0.0f && fadeIn)
            )
        {
            float newAddTime = fadeToBlackSpeed * Time.deltaTime;

            if (fadeIn)
            {
                newAddTime *= -1.0f;
            }

            fadeToBlackPanel.color = MakeBlackWithAlpha(fadeToBlackPanel.color.a + newAddTime);
            yield return new WaitForSeconds(0.01f);
        }
    }



    Color MakeBlackWithAlpha(float alpha)
    {
        return new Color(0.0f, 0.0f, 0.0f, alpha);
    }
}
