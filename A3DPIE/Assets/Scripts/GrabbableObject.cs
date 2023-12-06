using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class GrabbableObject : MonoBehaviour, IInteractable
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
            return type;
        }
    }

    public EInteractionType type = EInteractionType.GRABBABLE;
    public string name;

    Rigidbody rb;



    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }



    public void Interact()
    {
        Grab();
    }



    public void Grab()
    {
        TryLeaveConveyorBelt();

        rb.isKinematic = true;
        transform.SetParent(CameraController.instance.grabPoint, false);
    }



    public void Drop()
    {
        transform.SetParent(null);
        rb.isKinematic = false;

    }



    private void TryLeaveConveyorBelt()
    {
        //leave conveyor belt if currently on it
        MoveAlongSpline moveAlongSpline = gameObject.GetComponent<MoveAlongSpline>();
        if (moveAlongSpline != null)
        {
            moveAlongSpline.barConveyorBelt.LeaveConveyorBelt(gameObject);
        }
    }
}
