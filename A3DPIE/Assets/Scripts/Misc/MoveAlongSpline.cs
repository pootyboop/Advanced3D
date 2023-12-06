using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

/*

This script is based on a GitHub repository.
    Author: AnanD3V
    https://github.com/AnanD3V/Splines-2.0/blob/main/Scripts/MoveAlongSpline.cs
    Accessed: 6/12/2023
*/


[RequireComponent(typeof(Rigidbody))]
public class MoveAlongSpline : MonoBehaviour
{
    public BarConveyorBelt barConveyorBelt;
    public SplineContainer spline;
    public float speed = .5f;
    public float distancePercentage = 0f;

    public float splineLength;

    public Rigidbody rb;



    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }



    void Update()
    {
        Move();
    }



    void Move()
    {
        distancePercentage += speed * Time.deltaTime / splineLength;

        Vector3 currentPosition = spline.EvaluatePosition(distancePercentage);
        transform.position = currentPosition;

        if (distancePercentage > 1f)
        {
            distancePercentage = 0f;
        }

        Vector3 nextPosition = spline.EvaluatePosition(distancePercentage + 0.05f);
        Vector3 direction = nextPosition - currentPosition;
        transform.rotation = Quaternion.LookRotation(direction, transform.up);
    }
}