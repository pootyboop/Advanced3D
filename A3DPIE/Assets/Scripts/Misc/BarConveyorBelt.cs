using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

public class BarConveyorBelt : MonoBehaviour
{
    SplineContainer spline;
    float splineLength;

    public GameObject[] conveyorObjects;


    void Start()
    {
        spline = gameObject.GetComponent<SplineContainer>();
        splineLength = spline.CalculateLength();

        for (int i = 0; i < conveyorObjects.Length; i++)
        {
            JoinConveyorBelt(conveyorObjects[i]);
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
