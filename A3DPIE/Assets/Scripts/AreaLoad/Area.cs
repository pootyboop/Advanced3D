using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public enum ELoadArea
{
    F0,
    F1,
    F2,
    F0F2STAIRWELL,
    F0F1STAIRWELL
}



[System.Serializable]
public class Area
{
    public ELoadArea areaName;
    public GameObject reference;
    public ELoadArea[] coloadedAreas;
}