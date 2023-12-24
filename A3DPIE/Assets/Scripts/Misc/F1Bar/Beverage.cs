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
        int cost = 0;

        int drinkID = UnityEngine.Random.Range(0, 4);

        switch (drinkID)
        {
            case 0:
                drinkName += " of Bollet Voenni";
                cost = 178;
                break;
            case 1:
                drinkName += " of Ecas Cisc Asca";
                cost = 100;
                break;
            case 2:
                drinkName += " of Lup Binet";
                cost = 80;
                break;
            case 3:
                drinkName += " of Pical Anlon";
                cost = 125;
                break;

        }

        grabbableObject.name = drinkName;
        grabbableObject.itemCost = cost;

        Material[] mats = meshRenderer.materials;
        mats[1] = drinkMaterials[drinkID];
        meshRenderer.materials = mats;
    }
}
