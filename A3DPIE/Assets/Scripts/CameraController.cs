using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraController : MonoBehaviour
{
    public static CameraController instance;

    public Image reticle;
    public Transform grabPoint;

    public float sensitivity = 300.0f;
    public Vector2 rot = new Vector2(0f, 0f);
    public bool canRotateView;
    float interactionRange = 4.0f;

    public IInteractable targetInteractable;

    void Start()
    {
        instance = this;

        //hide mouse
        SetMouseVisibility(false, true);
    }


    public void Setup()
    {

        rot = new Vector2(0f, 0f);
        transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
    }


    public void SetMouseVisibility(bool isVisible, bool canControlCharacterView)
    {
        Cursor.visible = isVisible;
        if (isVisible)
        {
            Cursor.lockState = CursorLockMode.None;
        }

        else
        {
            Cursor.lockState = CursorLockMode.Locked;
        }

        canRotateView = canControlCharacterView;
        reticle.gameObject.SetActive(canRotateView);
    }



    public void SyncedUpdate()
    {
        Look();
        CheckInteractable();
    }



    void Look()
    {
        if (canRotateView)
        {
            //credit:
            //https://youtu.be/f473C43s8nE
            //http://gyanendushekhar.com/2020/02/06/first-person-movement-in-unity-3d/

            Vector2 mousePos = new Vector2(0.0f, 0.0f);

            mousePos.x = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensitivity;
            mousePos.y = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensitivity;

            rot.y += mousePos.x;
            rot.x = Mathf.Clamp(rot.x - mousePos.y, -90f, 90f);

            transform.rotation = Quaternion.Euler(rot.x, rot.y, 0.0f);
            //transform.parent.gameObject.transform.Rotate(new Vector3(0f, rot.y, 0f));
        }
    }



    void CheckInteractable()
    {
        switch (PlayerMovement.instance.state)
        {
            case EPlayerState.DIALOGUE:
                return;
        }

        Ray ray = new Ray(transform.position, transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hitInfo, interactionRange))
        {
            if (hitInfo.collider.gameObject.TryGetComponent(out IInteractable hitInteractableObject))
            {
                if (hitInteractableObject != targetInteractable)
                {
                    StopTargetInteractable();
                    StartTargetInteractable(hitInteractableObject);
                }
            }

            else
            {
                StopTargetInteractable();
            }
        }

        else
        {
            StopTargetInteractable();
        }
    }



    void StartTargetInteractable(IInteractable newInteractable)
    {
        targetInteractable = newInteractable;
        UI.instance.ShowInteractionText(newInteractable);
    }



    public void StopTargetInteractable()
    {
        if (targetInteractable == null)
        {
            return;
        }

        targetInteractable = null;
        UI.instance.HideInteractionText();
    }



    public void TryInteract()
    {
        if (targetInteractable != null)
        {
            UI.instance.HideInteractionText();
            targetInteractable.Interact();
        }
    }
}
