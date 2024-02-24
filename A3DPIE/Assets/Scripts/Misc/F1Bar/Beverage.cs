using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



//random glass of one of the drinks
public class Beverage : MonoBehaviour
{
    //force set what drink this is
    public int drinkID = -1;

    //does drink cost money
    public bool costsKartet = true;

    //the materials this drink can use
    public Material[] drinkMaterials = new Material[4];
    private Material mat;
    private int matIndex = 2;
    private float drinkTime = 1.0f;
    private bool drank = false;

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

        //if set to -1, randomize ID
        if (drinkID == -1)
        {
            drinkID = UnityEngine.Random.Range(0, drinkMaterials.Length);
        }

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


        //set cost to 0 if it's free
        if (!costsKartet)
        {
            cost = 0;
        }

        //pass the name and cost to the grabbable object
        grabbableObject.name = drinkName;
        grabbableObject.itemCost = cost;

        //add the material
        //have to do it this way since renderers won't let you set one element in the array
        Material[] mats = meshRenderer.materials;
        mats[matIndex] = drinkMaterials[drinkID];
        mat = mats[matIndex];
        meshRenderer.materials = mats;
    }



    public void Drink() {
        drank = true;

        Material[] mats = meshRenderer.materials;
        mats[matIndex] = new Material(drinkMaterials[drinkID]);
        mat = mats[matIndex];
        meshRenderer.materials = mats;

        grabbableObject.name = "Empty Glass";

        StartCoroutine(DrinkOverTime());
    }



    IEnumerator DrinkOverTime() {
        float time = 0.0f;

        while (time < drinkTime) {
            time += Time.deltaTime;

            mat.SetFloat("_LiquidAmount", 1.0f - time / drinkTime);

            yield return null;
        }
    }



    public bool CanDrink() {
        return !drank;
    }
}
