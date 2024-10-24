using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[RequireComponent(typeof(SphereCollider))]
public class CharDistCull : MonoBehaviour
{
    private bool isCulled = false;
    public bool cullable = true;
    public float cullDistance = 30.0f;
    public List<GameObject> cullObjects;
    private SphereCollider coll;

    void Start()
    {
        coll = GetComponent<SphereCollider>();
        coll.radius = cullDistance;
        cullObjects = new List<GameObject>();
    }



    void OnEnable()
    {
        SolveCullState();
    }



    public void AddCullableObject(GameObject obj)
    {
        cullObjects.Add(obj);
    }



    void SolveCullState()
    {
        bool solvedCull = false;

    if (PlayerMovement.instance == null) {
        solvedCull = false;
    }

    else if (PlayerMovement.instance.state == EPlayerState.CUTSCENE) {
        solvedCull = false;
    }

    else {
        solvedCull = !IsPlayerInCollRadius();
    }

        SetCharacterCulled(solvedCull);
    }



    bool IsPlayerInCollRadius()
    {
        if (PlayerMovement.instance != null)
        {
            Vector3 playerPos = PlayerMovement.instance.transform.position;
            return cullDistance == Vector3.Distance(playerPos, transform.position);
        }

        //cant find player yet. dont cull, just to be safe
        return true;
    }



    void SetCharacterCulled(bool newCull)
    {
        if (newCull == isCulled || !cullable)
        {
            return;
        }

        isCulled = newCull;

        foreach (GameObject obj in cullObjects)
        {
            obj.SetActive(!isCulled);
        }
    }



    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            SetCharacterCulled(false);
        }
    }



    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            SetCharacterCulled(true);
        }
    }
}
