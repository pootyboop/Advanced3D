using System.Collections;
using System.Collections.Generic;
using UnityEngine;



//IInteractable that allows the player to leave Kallay Tirridor and end the game
public class SpaceshipDashboard : MonoBehaviour, IInteractable
{
    public string interactionName
    {
        get
        {
            return "Dashboard";
        }
    }

    public EInteractionType interactionType
    {
        get
        {
            return EInteractionType.SPACESHIPDEPART;
        }
    }

    public int kartetCost
    {
        get
        {
            return 0;
        }
    }

    public bool targetable
    {
        get
        {
            return true;
        }
    }



    //leave Kallay Tirridor and end the game
    public void Interact()
    {
        CutsceneManager.instance.PlayOutroCutscene();
    }



    //targeting doesn't change anything
    public void OnTargetedChanged(bool isTargeting)
    {
    }
}
