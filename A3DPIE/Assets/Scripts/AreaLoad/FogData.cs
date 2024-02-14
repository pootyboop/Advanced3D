using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FogData
{
    public float attenuationDistance = 450f;
    [ColorUsage(true, true)]
    public Color HDRColor;
}
