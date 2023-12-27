using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

/*
    Move() is based on a GitHub repository.
    Author: AnanD3V
    https://github.com/AnanD3V/Splines-2.0/blob/main/Scripts/MoveAlongSpline.cs
    Accessed: 6/12/2023
*/



//moves objects along a spline
//used in-game for the F1 bar conveyor belt
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



    //move every frame
    //this script is removed from objects that leave the conveyor belt...
    //...so Update is only called when necessary
    void Update()
    {
        Move();
    }



    //moves the object along the spline
    //the unity splines package includes a built-in function similar to this...
    //...but i can control the rotation properly with this one
    //AnanD3V wrote all of Move(). i only added comments to explain what's happening
    void Move()
    {
        //use speed to figure out how far along the spline the object should be
        distancePercentage += speed * Time.deltaTime / splineLength;

        //move to the new position
        Vector3 currentPosition = spline.EvaluatePosition(distancePercentage);
        transform.position = currentPosition;

        //if at/past the end of the spline, restart the loop
        if (distancePercentage > 1f)
        {
            distancePercentage = 0f;
        }

        //orient the object along the spline
        //0.05 is just how far along the spline to aim the object at.accuracy will not really matter too much
        Vector3 nextPosition = spline.EvaluatePosition(distancePercentage + 0.05f);
        Vector3 direction = nextPosition - currentPosition;
        transform.rotation = Quaternion.LookRotation(direction, transform.up);
    }
}