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

    Animator animator;
    Collider collider;



    void Start()
    {
        animator = GetComponent<Animator>();
        collider = GetComponent<Collider>();
    }



    public void Interact()
    {
        Open(!open);
    }



    public void OnTargetedChanged(bool isTargeting)
    {
    }



    void Open(bool isOpening)
    {
        open = isOpening;
        collider.enabled = !isOpening;
        animator.SetBool("open", isOpening);

        if (open)
        {
            StartCoroutine(DoorTimer());
        }
    }



    IEnumerator DoorTimer()
    {
        yield return new WaitForSeconds(stayOpenTime);
        Open(false);
    }
}
