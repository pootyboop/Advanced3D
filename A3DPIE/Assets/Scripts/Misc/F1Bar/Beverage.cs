using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beverage : MonoBehaviour
{
    public Material[] drinkMaterials = new Material[4];

    private GrabbableObject grabbableObject;
    private MeshRenderer meshRenderer;

    void Start()
    {
        grabbableObject = GetComponent<GrabbableObject>();
        meshRenderer = GetComponent<MeshRenderer>();
        RandomSelectDrinkType();
    }

    void RandomSelectDrinkType()
    {

        string drinkName = "Glass";

        int drinkID = UnityEngine.Random.Range(0, 4);

        switch (drinkID)
        {
            case 0:
                drinkName += " of Bollet Voenni";
                break;
            case 1:
                drinkName += " of Ecas Cisc Asca";
                break;
            case 2:
                drinkName += " of Lup Binet";
                break;
            case 3:
                drinkName += " of Pical Anlon";
                break;

        }

        grabbableObject.name = drinkName;

        Material[] mats = meshRenderer.materials;
        mats[1] = drinkMaterials[drinkID];
        meshRenderer.materials = mats;
    }
}
