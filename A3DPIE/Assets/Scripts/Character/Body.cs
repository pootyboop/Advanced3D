using System.Collections;
using System.Collections.Generic;
using UnityEngine;



//container for the data used to generate a character
//contains all body part data, the character's stature, and all colors used to color body part materials
[System.Serializable]
public class Body
{
    //scale of the character
    public float stature = 1.0f;
    //all body parts
    public BodyPart head, torso, armL, armR, handL, handR, legs, hair;
    //consistent color for various materials
    public Color skinTone, eyeColor, lipColor, hairColor, metalColor, emissiveColor, leatherColor;
    //various colors for various clothing items
    //arms use same color as torso so they appear as sleeves
    public Color clothColorHead, clothColorTorso, clothColorLegs;
}