using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavigablePoint : MonoBehaviour
{
    public ELoadArea[] areas;

    private bool added = false;

    void Start()
    {
        if (!added) {
            added = true;
            NavigationManager.instance.AddNavigablePoint(this);
        }
    }
}