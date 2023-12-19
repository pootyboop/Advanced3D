using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Body
{
    public float stature = 1.0f;
    public BodyPart head, torso, armL, armR, handL, handR, legs, hair;
    public Color skinTone, eyeColor, lipColor, hairColor, metalColor, emissiveColor, leatherColor;    //consistent color for various materials
    public Color clothColorHead, clothColorTorso, clothColorLegs; //various colors for various clothing items
                                                                    //arms use same color as torso
}