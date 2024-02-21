using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//this manages the actual creation of the character (with help from CharacterCreationManager)
//so this generates the actual meshes, materials, etc. that represent this character in-game
public class CharacterCreator : MonoBehaviour
{
    public CharDistCull culler;

    public Transform rootBone, grabL, grabR;    //grabL and grabR are 
    public SkinnedMeshRenderer originalRenderer;    //this won't actually render anything but it drives the animation and rigging for all body parts



    public bool randomlyGenerate = true;    //disable this to manually input body parts via..
    public Body body;                       //this master Body class in the inspector

    public GameObject head, torso, armL, armR, handL, handR, legs, hair;    //these hold references to the skinned meshes for each body part



    void Start()
    {
        GenerateCharacter();
        UpdateHandTransforms();
    }



    //generate the character!
    public void GenerateCharacter()
    {
        //grab random properties (material colors, meshes) from CharacterCreationManager
        if (randomlyGenerate)
        {
            body = CharacterCreationManager.instance.GenerateCharacter(body);
        }

        //the rest of this is actually generating the character meshes using whatever properties we have (set randomly or manually)

        //stature sets the scale of the character (not just the height) to prevent stretching
        transform.localScale = transform.localScale * body.stature;

        head = AddBodyPart(body.head, "Head");
        torso = AddBodyPart(body.torso, "Torso");

        armL = AddBodyPart(body.armL, "Arm Left");
        armR = AddBodyPart(body.armR, "Arm Right");

        handL = AddBodyPart(body.handL, "Hand Left");
        handR = AddBodyPart(body.handR, "Hand Right");

        legs = AddBodyPart(body.legs, "Legs");
        hair = AddBodyPart(body.hair, "Hair");
    }



    //generates and adds one body part skinned mesh to the character
    GameObject AddBodyPart(BodyPart bodyPart, string partName)
    {
        //create the object and parent it to the character
        GameObject newBodyPartObj = new GameObject(partName);
        newBodyPartObj.transform.SetParent(transform, false);
        if (culler != null)
        {
            culler.AddCullableObject(newBodyPartObj);
        }

        //add the skinned mesh
        SkinnedMeshRenderer renderer = newBodyPartObj.AddComponent<SkinnedMeshRenderer>();
        renderer.sharedMesh = bodyPart.mesh;

        //set the bounds as tight as i safely can without knowing the exact proportions of the character
        //these should still be relative to the root bone as all body parts use the same skeleton with the root centered between the character's feet
        renderer.localBounds = new Bounds(new Vector3(0f, 0f, 0f), new Vector3(0.1f, 0.3f, 0.1f));
        //follow the original renderer's rig
        renderer.bones = originalRenderer.bones;
        renderer.rootBone = rootBone;

        //set up the materials needed for this body part
        Material[] materials = new Material[bodyPart.materialTypes.Length];

        for (int i = 0; i < materials.Length; i++)
        {
            materials[i] = CharacterCreationManager.instance.GetBaseMaterialByType(bodyPart.materialTypes[i]);
        }

        //assign the materials
        //would just assign them one by one but renderers don't like that for some reason
        renderer.materials = materials;

        //assign colors to each material
        //using for instead of foreach so i can access both arrays
        for (int i = 0; i < renderer.materials.Length; i++)
        {
            switch (bodyPart.materialTypes[i])
            {
                case EMaterialType.EYES:    //eyes use a custom shader to change just the iris color
                    renderer.materials[i].SetColor("_Eye_Color", GetColorByPartMaterial(partName, bodyPart.materialTypes[i]));
                    break;
                case EMaterialType.METAL:   //these material types use my master shader graph, so set the Tint property
                case EMaterialType.EMISSIVE:
                    renderer.materials[i].SetColor("_Tint", GetColorByPartMaterial(partName, bodyPart.materialTypes[i]));
                    break;
                case EMaterialType.CLOTH:
                    renderer.materials[i].SetColor("_Tint", GetColorByPartMaterial(partName, bodyPart.materialTypes[i]));
                    renderer.materials[i].SetTexture("_Texture", body.clothTexture);
                    break;
                default:    //by default, assume it's a default unity material and just set the color property
                    renderer.materials[i].color = GetColorByPartMaterial(partName, bodyPart.materialTypes[i]);
                    break;
            }
        }

        return newBodyPartObj;
    }



    //grabs the corresponding color by the material type
    //also uses the part type for special cases
    //(e.g. using the same cloth color for torsos and arms so arms are colored as sleeves of the torso)
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

                //3 types of cloth (head, torso/arms, and legs) so characters aren't limited to solid color tracksuits
            case EMaterialType.CLOTH:
                switch (partName)
                {
                    //case "Hair":
                    //    return body.clothColorHead;
                    case "Legs":
                        return body.clothColorLegs;
                    default:
                        return body.clothColorTorso;
                }

            default:
                //if nothing else, just return black since we're probably dealing with UNSET which is used for the solid black inside of characters' mouths
                return new Color(0f, 0f, 0f);
        }
    }



    //just sets up the grab points so characters can grab objects at the correct point on their hands
    void UpdateHandTransforms()
    {
        Character character = GetComponent<Character>();
        character.SetGrabTransforms(grabL, grabR);
    }
}
