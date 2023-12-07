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
            return EInteractionType.GRABBABLE;
        }
    }

    public bool targetable
    {
        get
        {
            return !grabbed;
        }
    }

    public string name;

    bool grabbed = false;

    Rigidbody rb;
    Collider collider;



    void Start()
    {
        rb = GetComponent<Rigidbody>();
        collider = GetComponent<Collider>();
    }



    public void Interact()
    {
        if (grabbed)
        {
            Drop();
        }

        else
        {
            Grab();
        }
    }



    public void Grab()
    {
        grabbed = true;

        TryLeaveConveyorBelt();

        rb.isKinematic = true;
        transform.SetParent(CameraController.instance.grabPoint, false);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.Euler(Vector3.zero);
        collider.enabled = false;

        PlayerMovement.instance.GrabObject(this);
    }



    public void Drop()
    {
        grabbed = false;

        transform.SetParent(null);
        rb.isKinematic = false;
        collider.enabled = true;
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
