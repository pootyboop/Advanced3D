using System.Collections;
using System.Collections.Generic;
using UnityEngine;



//container for linking EMaterialTypes with the materials they use
[System.Serializable]
public class CharacterMaterial
{
    public EMaterialType materialType;
    public Material material;
}