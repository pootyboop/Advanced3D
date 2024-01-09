using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;



//manages spawning/adding/removing objects to/from the F1 island bar conveyor belt
public class BarConveyorBelt : MonoBehaviour
{
    //conveyor belt spline
    SplineContainer spline;
    float splineLength;

    public DialogueCharacter bartenderDialogueOnBuy;    //conversation to play when the player takes a drink off the conveyor belt

    public Beverage glassOfDrinkPrefab;     //the drink prefab to spawn along the conveyor belt
    public int numberOfDrinks = 14; //how many total drinks to spawn

    public GameObject[] conveyorObjects;    //all objects moving along the conveyor belt



    void Start()
    {
        spline = gameObject.GetComponent<SplineContainer>();
        splineLength = spline.CalculateLength();

        //spawn the drinks
        InstantiateDrinks(numberOfDrinks);
    }



    //spawn drinks
    void InstantiateDrinks(int drinkCount)
    {
        for (int i = 0; i < drinkCount; i++)
        {
            GameObject obj = Instantiate(glassOfDrinkPrefab).gameObject;
            obj.GetComponent<GrabbableObject>().dialogueOnFirstGrab = bartenderDialogueOnBuy;   //tell the drink what dialogue to use when grabbed
            JoinConveyorBelt(obj);
        }
    }



    //make an object move along the conveyor belt
    public void JoinConveyorBelt(GameObject obj)
    {
        MoveAlongSpline newObj = obj.AddComponent<MoveAlongSpline>();   //add the spline movement component
        newObj.barConveyorBelt = this;
        newObj.transform.SetParent(transform, true);  //make the drink a child of this object so it loads in/out properly
        newObj.spline = spline;
        newObj.splineLength = splineLength;

        //start at a random point along the spline
        //this may result in some drinks overlapping each other but i don't think it's a big issue
        newObj.distancePercentage = Random.Range(0f, 1f);

        //no physics, just perfectly follow the spline
        obj.GetComponent<Rigidbody>().isKinematic = true;
    }



    //remove an object from the conveyor belt
    //it will no longer follow the spline and its physics will be turned back on
    public void LeaveConveyorBelt(GameObject obj)
    {
        //enable physics
        obj.GetComponent<Rigidbody>().isKinematic = false;
        //destroy the spline follow script
        Destroy(obj.GetComponent<MoveAlongSpline>());
    }
}
