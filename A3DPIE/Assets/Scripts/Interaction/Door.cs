using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;


public class Door : MonoBehaviour, IInteractable
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
            return EInteractionType.DOOR;
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

    public string name = "Door";
    public float stayOpenTime = 7.0f;

    bool open = false;

    public ELoadArea loadArea1, loadArea2;

    Animator animator;
    Collider collider;



    void Start()
    {
        animator = GetComponent<Animator>();
        collider = GetComponent<Collider>();
    }



    public void Interact()
    {
        SetOpen(!open);
    }



    public void OnTargetedChanged(bool isTargeting)
    {
    }



    void SetOpen(bool isOpening)
    {
        open = isOpening;
        collider.enabled = !isOpening;
        animator.SetBool("open", isOpening);

        if (open)
        {
            StartCoroutine(DoorTimer());

            AreaLoadManager.instance.SetAreaLoaded(loadArea1, true);
            AreaLoadManager.instance.SetAreaLoaded(loadArea2, true);
        }

        else
        {
            AreaLoadManager.instance.SetAreaLoaded(loadArea1, false);
            AreaLoadManager.instance.SetAreaLoaded(loadArea2, false);
        }
    }



    IEnumerator DoorTimer()
    {
        yield return new WaitForSeconds(stayOpenTime);
        SetOpen(false);
    }
}
