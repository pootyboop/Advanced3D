using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public enum EMaterialType
{
    UNSET,
    SKIN,
    EYES,
    LIPS,
    HAIR,
    CLOTH,
    METAL,
    EMISSIVE
}



[System.Serializable]
public class BodyPart
{
    public Mesh mesh;
    public EMaterialType[] materialTypes;
}