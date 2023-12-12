using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCreationManager : MonoBehaviour
{
    public static CharacterCreationManager instance;
    public Gradient skinToneRange, eyeColorRange, lipColorRange, hairColorRange, clothColorRange, metalColorRange, emissiveColorRange;
    public BodyPart[] heads, torsos, armsL, armsR, handsL, handsR, legs;
    public CharacterMaterial[] characterMaterials;

    //TEMP VARIABLES -- used during character generation
    EMaterialType[] materialTypes;



    void Start()
    {
        instance = this;
    }



    public Body GenerateCharacter(Body pregeneratedInfo)
    {
        materialTypes = new EMaterialType[0];

        Body newBody;

        if (pregeneratedInfo != null)
        {
            newBody = pregeneratedInfo;
        }

        else
        {
            newBody = new Body();
        }

        newBody.stature = UnityEngine.Random.Range(.9f, 1.1f);
        newBody = GenerateColors(newBody);
        newBody = GenerateBodyParts(newBody);

        return newBody;
    }



    Body GenerateBodyParts(Body body)
    {
        body.head = GenerateBodyPart(body.head, heads);
        body.torso = GenerateBodyPart(body.torso, torsos);
        body.armL = GenerateBodyPart(body.armL, armsL);
        body.armR = GenerateBodyPart(body.armR, armsR);
        body.handL = GenerateBodyPart(body.handL, handsL);
        body.handR = GenerateBodyPart(body.handR, handsR);
        body.legs = GenerateBodyPart(body.legs, legs);

        return body;
    }



    BodyPart GenerateBodyPart(BodyPart bodyPart, BodyPart[] partPool)
    {
        /*
        //check if we havent already set a body part to be used
        if (bodyPart == null)
        {
            //if so just grab a random part
            bodyPart = partPool[UnityEngine.Random.Range(0, partPool.Length)];
        }

        //otherwise use what we already got
        return bodyPart;
        */

        return partPool[UnityEngine.Random.Range(0, partPool.Length)];
    }



    Body GenerateColors(Body body)
    {
        /*
        for (int i = 0; i < materialTypes.Length; i++)
        {
            switch (materialTypes[i])
            {
                case EMaterialType.SKIN:
                    body.skinTone = skinToneRange.Evaluate(UnityEngine.Random.Range(0f,1f));
                    break;
                case EMaterialType.EYES:
                    body.eyeColor = eyeColorRange.Evaluate(UnityEngine.Random.Range(0f, 1f));
                    break;
                case EMaterialType.LIPS:
                    body.lipColor = lipColorRange.Evaluate(UnityEngine.Random.Range(0f, 1f));
                    break;
                case EMaterialType.HAIR:
                    body.hairColor = hairColorRange.Evaluate(UnityEngine.Random.Range(0f, 1f));
                    break;
                case EMaterialType.METAL:
                    body.metalColor = metalColorRange.Evaluate(UnityEngine.Random.Range(0f, 1f));
                    break;
                case EMaterialType.EMISSIVE:
                    body.emissiveColor = emissiveColorRange.Evaluate(UnityEngine.Random.Range(0f, 1f));
                    break;
                default:
                    break;
            }
        }
        */

        body.skinTone = skinToneRange.Evaluate(UnityEngine.Random.Range(0f, 1f));
        body.eyeColor = eyeColorRange.Evaluate(UnityEngine.Random.Range(0f, 1f));
        body.lipColor = lipColorRange.Evaluate(UnityEngine.Random.Range(0f, 1f));
        body.hairColor = hairColorRange.Evaluate(UnityEngine.Random.Range(0f, 1f));
        body.metalColor = metalColorRange.Evaluate(UnityEngine.Random.Range(0f, 1f));
        body.emissiveColor = emissiveColorRange.Evaluate(UnityEngine.Random.Range(0f, 1f));
        body.clothColorHead = GenerateClothColor();
        body.clothColorTorso = GenerateClothColor();
        body.clothColorLegs = GenerateClothColor();

        return body;
    }



    Color GenerateClothColor()
    {
        return clothColorRange.Evaluate(UnityEngine.Random.Range(0f, 1f));
    }



    public Material GetBaseMaterialByType(EMaterialType materialType)
    {
        for (int i = 0; i < characterMaterials.Length; i++)
        {
            if (materialType == characterMaterials[i].materialType)
            {
                return characterMaterials[i].material;
            }
        }

        return null;
    }
}
