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
        if (randomlyGenerate)
        {
            GenerateCharacter();
        }
    }



    public void GenerateCharacter()
    {
        body = CharacterCreationManager.instance.GenerateCharacter(body);

        transform.localScale = new Vector3(body.stature, body.stature, body.stature);

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
            renderer.materials[i].color = GetColorByPartMaterial(partName, bodyPart.materialTypes[i]);
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
                return new Color(1f, 1f, 1f);
        }
    }
}
