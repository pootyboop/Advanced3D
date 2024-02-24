using System.Collections;
using System.Collections.Generic;
using UnityEngine;



//type of material usable by character body part skinned meshes
//this is used instead of material references to save memory
public enum EMaterialType
{
    UNSET,
    SKIN,
    EYES,
    LIPS,
    HAIR,
    CLOTH,
    METAL,
    EMISSIVE,
    LEATHER
}



//container for the necessary data for each body part: the mesh itself and the material types it uses
[System.Serializable]
public class BodyPart
{
    public Mesh mesh;
    public EMaterialType[] materialTypes;
}