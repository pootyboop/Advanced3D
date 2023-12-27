using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



//random glass of one of the drinks
public class Beverage : MonoBehaviour
{
    //the materials this drink can use
    public Material[] drinkMaterials = new Material[4];

    private GrabbableObject grabbableObject;
    private MeshRenderer meshRenderer;



    void Start()
    {
        grabbableObject = GetComponent<GrabbableObject>();
        meshRenderer = GetComponent<MeshRenderer>();
        RandomSelectDrinkType();
    }



    //randomly pick one of the drinks this can be a glass of
    //and set up the material, cost, and name appropriately
    void RandomSelectDrinkType()
    {
        //by default, it's a free empty glass
        string drinkName = "Glass";
        int cost = 0;

        //random drink ID
        int drinkID = UnityEngine.Random.Range(0, drinkMaterials.Length);

        //set the drink name and cost
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

        //pass the name and cost to the grabbable object
        grabbableObject.name = drinkName;
        grabbableObject.itemCost = cost;

        //add the material
        //have to do it this way since renderers won't let you set one element in the array
        Material[] mats = meshRenderer.materials;
        mats[1] = drinkMaterials[drinkID];
        meshRenderer.materials = mats;
    }
}
