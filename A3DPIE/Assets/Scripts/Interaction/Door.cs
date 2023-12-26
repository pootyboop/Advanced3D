using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;



//handles doors opening and closing
//doors work together with the area loading system
public class Door : MonoBehaviour, IInteractable
{

    public string interactionName
    {
        get
        {
            return name;
        }
    }

    //doors are always doors
    public EInteractionType interactionType
    {
        get
        {
            return EInteractionType.DOOR;
        }
    }

    //doors don't cost money to open
    public int kartetCost
    {
        get
        {
            return 0;
        }
    }

    //doors can always be targeted since their hitboxes are disabled when they're open (meaning you can't interact with them anyway)
    public bool targetable
    {
        get
        {
            return true;
        }
    }

    public string name = "Door";
    public float stayOpenTime = 7.0f;   //how long the door stays open before auto-closing

    bool open = false;

    public ELoadArea loadArea1, loadArea2;  //the loadable areas on either side of this door

    Animator animator;
    Collider collider;



    void Start()
    {
        animator = GetComponent<Animator>();
        collider = GetComponent<Collider>();
    }



    //open the door when interacted with
    public void Interact()
    {
        SetOpen(!open);
    }



    //doors don't care if you're looking at them or not
    public void OnTargetedChanged(bool isTargeting)
    {
    }



    //open or close the door
    void SetOpen(bool isOpening)
    {
        open = isOpening;
        collider.enabled = !isOpening;  //collider immediately toggles when door opens/closes
        animator.SetBool("open", isOpening);

        if (open)
        {
            //timer for closing the door automatically
            StartCoroutine(DoorTimer());

            //load the areas on both sides of the door since the player can see both
            AreaLoadManager.instance.SetAreaLoaded(loadArea1, true);
            AreaLoadManager.instance.SetAreaLoaded(loadArea2, true);
        }

        else
        {
            //try to unload both sides of the door
            //AreaLoadManager will prevent the area the player's currently in from getting unloaded
            //potential for an area loading bug if the player mad dashes away from the door...
            //...but by that point, the area will probably not be loaded anymore anyway
            AreaLoadManager.instance.SetAreaLoaded(loadArea1, false);
            AreaLoadManager.instance.SetAreaLoaded(loadArea2, false);
        }
    }



    //timer before the door auto-closes
    IEnumerator DoorTimer()
    {
        yield return new WaitForSeconds(stayOpenTime);
        SetOpen(false);
    }
}
