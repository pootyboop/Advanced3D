using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]
public class CharacterNavigation : MonoBehaviour
{
    bool isWalking = false;
    bool inDialogue = false;
    private NavMeshAgent navMeshAgent;
    private Animator animator;
    public Vector3 destination;
    
    public ELoadArea[] areas;

    public float walkTimeMin = 12.0f;
    public float walkTimeMax = 20.0f;

    float currSpeed = 0.0f;



    public void StartWalking() {
        
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.enabled = true;
        animator = GetComponent<Animator>();

        isWalking = true;

        FindNewPoint();
    }



    void FindNewPoint() {

        if (areas != null) {

            if (areas.Length > 0) {
                destination = NavigationManager.instance.GetNavigablePointInArea(areas);
            }

            else {
                destination = NavigationManager.instance.GetRandomNavigablePoint();
            }
            
        }

        else {
            destination = NavigationManager.instance.GetRandomNavigablePoint();
        }



        navMeshAgent.SetDestination(destination);

        StartCoroutine(WalkTimer());
    }



    IEnumerator WalkTimer() {

        float time = 0.0f;
        float walkTime = UnityEngine.Random.Range(walkTimeMin, walkTimeMax);
        Vector3 prevPos = transform.position;

        while (time < walkTime) {

            if (inDialogue) {
                yield return null;
            }

            else {
                Vector3 currPos = transform.position;

                SetSpeed(Vector3.Distance(currPos, prevPos));

                prevPos = currPos;

                time += Time.deltaTime;
                yield return null;
            }
        }

        FindNewPoint();
    }



    public void SetInDialogue(bool newInDialogue) {
        inDialogue = newInDialogue;

        if (inDialogue) {
            SetSpeed(0.0f);
            navMeshAgent.ResetPath();
        }

        //resume path
        else {
            navMeshAgent.SetDestination(destination);
        }
    }



    public void SetSpeed(float newSpeed) {
        currSpeed = newSpeed;
        animator.SetFloat("speed", currSpeed);
    }
}