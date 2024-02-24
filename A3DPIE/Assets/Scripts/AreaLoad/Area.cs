using System.Collections;
using System.Collections.Generic;
using UnityEngine;



//easily-passable name for each loadable area
public enum ELoadArea
{
    F0,
    F1,
    F2,
    F0F2STAIRWELL,
    F0F1STAIRWELL
}



//container for data about each loadable area
[System.Serializable]
public class Area
{
    public ELoadArea areaName;
    public GameObject reference;    //the actual in-game parent of all objects constituting that area
    public FogData fogData;         //fog appearance in this area
    public ELoadArea[] coloadedAreas;   //areas that must be loaded when this area is loaded (areas that are visible from this area)
}