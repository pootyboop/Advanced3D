using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

public class BarConveyorBelt : MonoBehaviour
{
    SplineContainer spline;
    float splineLength;

    public Beverage glassOfDrinkPrefab;
    public GameObject[] conveyorObjects;

    public int numberOfDrinks = 14;


    void Start()
    {
        spline = gameObject.GetComponent<SplineContainer>();
        splineLength = spline.CalculateLength();

        InstantiateDrinks(numberOfDrinks);

        //for (int i = 0; i < conveyorObjects.Length; i++)
        //{
        //    JoinConveyorBelt(conveyorObjects[i]);
        //}
    }



    void InstantiateDrinks(int drinkCount)
    {
        for (int i = 0; i < drinkCount; i++)
        {
            GameObject obj = Instantiate(glassOfDrinkPrefab).gameObject;
            JoinConveyorBelt(obj);
        }
    }



    public void JoinConveyorBelt(GameObject obj)
    {
        MoveAlongSpline newObj = obj.AddComponent<MoveAlongSpline>();
        newObj.barConveyorBelt = this;
        newObj.spline = spline;
        newObj.splineLength = splineLength;
        newObj.distancePercentage = Random.Range(0f, 1f);
        obj.GetComponent<Rigidbody>().isKinematic = true;
    }



    public void LeaveConveyorBelt(GameObject obj)
    {
        MoveAlongSpline newObj = obj.GetComponent<MoveAlongSpline>();
        obj.GetComponent<Rigidbody>().isKinematic = false;

        Destroy(newObj);
    }
}
