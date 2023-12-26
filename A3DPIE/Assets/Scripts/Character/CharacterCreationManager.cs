using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//this generates random character data
//also holds info about what materials to use for each material type so CharacterCreator can reference it
public class CharacterCreationManager : MonoBehaviour
{
    public static CharacterCreationManager instance;
    //these gradients hold all the color info for random characters
    //gradients are GREAT for this since i can easily change the prominence of values by changing the gradient in the inspector
    public Gradient skinToneRange, eyeColorRange, lipColorRange, hairColorRange, clothColorRange, metalColorRange, emissiveColorRange, leatherColorRange;
    //stature range (how big/small the character can be). stature is 1 by default
    public float minStature = .9f;
    public float maxStature = 1.1f;
    //these arrays hold all body parts random characters can use
    public BodyPart[] heads, torsos, armsL, armsR, handsL, handsR, legs, hairs;
    //the materials to use for each material type
    //here for CharacterCreator to reference when generating skinned meshes
    //otherwise each character would need to reference all these materials
    public CharacterMaterial[] characterMaterials;



    void Start()
    {
        instance = this;
    }



    //randomly generate a Body for the CharacterCreator to use
    public Body GenerateCharacter(Body pregeneratedInfo)
    {
        Body newBody;

        //if we already passed in info, leave it alone
        if (pregeneratedInfo != null)
        {
            newBody = pregeneratedInfo;
        }

        //otherwise start from scratch
        else
        {
            newBody = new Body();
        }

        newBody.stature = UnityEngine.Random.Range(minStature, maxStature); //random stature in range
        newBody = GenerateColors(newBody); //generate the color scheme for this character
        newBody = GenerateBodyParts(newBody); //generate all body parts

        return newBody;
    }



    //randomly grab a part from each pool for each necessary body part
    //no weighting involved, all parts are equally likely
    Body GenerateBodyParts(Body body)
    {
        body.head = GenerateBodyPart(body.head, heads);
        body.torso = GenerateBodyPart(body.torso, torsos);
        body.armL = GenerateBodyPart(body.armL, armsL);
        body.armR = GenerateBodyPart(body.armR, armsR);
        body.handL = GenerateBodyPart(body.handL, handsL);
        body.handR = GenerateBodyPart(body.handR, handsR);
        body.legs = GenerateBodyPart(body.legs, legs);
        body.hair = GenerateBodyPart(body.hair, hairs);

        return body;
    }



    //randomly grab a single body part from the specified pool
    BodyPart GenerateBodyPart(BodyPart bodyPart, BodyPart[] partPool)
    {
        return partPool[UnityEngine.Random.Range(0, partPool.Length)];
    }



    //grabs a random value from the respective gradient for each necessary color
    Body GenerateColors(Body body)
    {
        body.skinTone = skinToneRange.Evaluate(UnityEngine.Random.Range(0f, 1f));
        body.eyeColor = eyeColorRange.Evaluate(UnityEngine.Random.Range(0f, 1f));
        body.lipColor = lipColorRange.Evaluate(UnityEngine.Random.Range(0f, 1f));
        body.hairColor = hairColorRange.Evaluate(UnityEngine.Random.Range(0f, 1f));
        body.metalColor = metalColorRange.Evaluate(UnityEngine.Random.Range(0f, 1f));
        body.emissiveColor = emissiveColorRange.Evaluate(UnityEngine.Random.Range(0f, 1f));
        body.leatherColor = leatherColorRange.Evaluate(UnityEngine.Random.Range(0f, 1f));
        body.clothColorHead = GenerateClothColor();
        body.clothColorTorso = GenerateClothColor();
        body.clothColorLegs = GenerateClothColor();

        return body;
    }



    //same as the rest, just grabs a random color from the gradient
    Color GenerateClothColor()
    {
        return clothColorRange.Evaluate(UnityEngine.Random.Range(0f, 1f));
    }



    //finds the actual material to use for the specified material type
    //this data is set in the inspector
    public Material GetBaseMaterialByType(EMaterialType materialType)
    {
        foreach (CharacterMaterial charMat in characterMaterials)
        {
            if (materialType == charMat.materialType)
            {
                return charMat.material;
            }
        }

        return null;
    }
}