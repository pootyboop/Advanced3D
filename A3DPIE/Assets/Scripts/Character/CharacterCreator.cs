using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCreator : MonoBehaviour
{
    public Transform rootBone;
    public SkinnedMeshRenderer originalRenderer;



    public bool randomlyGenerate = true;
    public Body body;

    public GameObject head, torso, armL, armR, handL, handR, legs;



    void Start()
    {
        GenerateCharacter();
    }



    public void GenerateCharacter()
    {

        if (randomlyGenerate)
        {
            body = CharacterCreationManager.instance.GenerateCharacter(body);
        }

        transform.localScale = transform.localScale * body.stature;

        head = AddBodyPart(body.head, "Head");
        torso = AddBodyPart(body.torso, "Torso");
        armL = AddBodyPart(body.armL, "Arm Left");
        armR = AddBodyPart(body.armR, "Arm Right");
        handL = AddBodyPart(body.handL, "Hand Left");
        handR = AddBodyPart(body.handR, "Hand Right");
        legs = AddBodyPart(body.legs, "Legs");
    }



    GameObject AddBodyPart(BodyPart bodyPart, string partName)
    {
        GameObject newBodyPartObj = new GameObject(partName);
        newBodyPartObj.transform.SetParent(transform, false);

        SkinnedMeshRenderer renderer = newBodyPartObj.AddComponent<SkinnedMeshRenderer>();
        renderer.sharedMesh = bodyPart.mesh;

        renderer.localBounds = new Bounds(new Vector3(0f, 1f, 0f), new Vector3(1f, 2f, 1f));
        renderer.bones = originalRenderer.bones;
        renderer.rootBone = rootBone;

        Material[] materials = new Material[bodyPart.materialTypes.Length];

        for (int i = 0; i < materials.Length; i++)
        {
            materials[i] = CharacterCreationManager.instance.GetBaseMaterialByType(bodyPart.materialTypes[i]);
            //materials[i].color = GetColorByPartMaterial(partName, bodyPart.materialTypes[i]);
        }

        renderer.materials = materials;

        for (int i = 0; i < renderer.materials.Length; i++)
        {
            switch (bodyPart.materialTypes[i])
            {
                case EMaterialType.EYES:
                    renderer.materials[i].SetColor("_Eye_Color", GetColorByPartMaterial(partName, bodyPart.materialTypes[i]));
                    break;
                case EMaterialType.METAL:
                    renderer.materials[i].SetColor("_Tint", GetColorByPartMaterial(partName, bodyPart.materialTypes[i]));
                    break;
                default:
                    renderer.materials[i].color = GetColorByPartMaterial(partName, bodyPart.materialTypes[i]);
                    break;
            }
        }

        return newBodyPartObj;
    }



    Color GetColorByPartMaterial(string partName, EMaterialType materialType)
    {
        switch (materialType)
        {
            case EMaterialType.SKIN:
                return body.skinTone;
            case EMaterialType.EYES:
                return body.eyeColor;
            case EMaterialType.LIPS:
                return body.lipColor;
            case EMaterialType.HAIR:
                return body.hairColor;
            case EMaterialType.METAL:
                return body.metalColor;
            case EMaterialType.EMISSIVE:
                return body.emissiveColor;
            case EMaterialType.LEATHER:
                return body.leatherColor;

            case EMaterialType.CLOTH:
                switch (partName)
                {
                    case "Head":
                        return body.clothColorHead;
                    case "Legs":
                        return body.clothColorLegs;
                    default:
                        return body.clothColorTorso;
                }

            default:
                //just return black since we're probably dealing with UNSET which is used for the black inside of characters' mouths
                return new Color(0f, 0f, 0f);
        }
    }
}
