using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

public class BarConveyorBelt : MonoBehaviour
{
    SplineContainer spline;
    float splineLength;


    void Start()
    {
        spline = gameObject.GetComponent<SplineContainer>();
        splineLength = spline.CalculateLength();
    }



    public void JoinConveyorBelt(GameObject object)
    {
        MoveAlongSpline newObj = object.AddComponent<MoveAlongSpline>();
        object.splineLength = splineLength;
    }



    public void LeaveConveyorBelt(GameObject object)
    {
        if (object.GetComponent<MoveAlongSpline>() != null)
        {
            Destroy(object.GetComponent<MoveAlongSpline>());
        }
    }
}
