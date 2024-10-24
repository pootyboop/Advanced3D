using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharRefsHelper : MonoBehaviour
{
    public static CharRefsHelper instance;



    public GameObject charDrink;


    
    void Start()
    {
        instance = this;
    }
}
