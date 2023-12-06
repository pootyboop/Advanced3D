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
    }



    public void LeaveConveyorBelt(GameObject obj)
    {
        if (obj.GetComponent<MoveAlongSpline>() != null)
        {
            Destroy(obj.GetComponent<MoveAlongSpline>());
        }
    }
}
