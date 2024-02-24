using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NavigationManager : MonoBehaviour
{
    public static NavigationManager instance;
    public AreaNavPoints areaNavPointsPrefab;
    public List<NavigablePoint> allNavPoints;

    public List<AreaNavPoints> sortedNavPoints;

    //private int maxChecksPerRandomPointFind = 10;



    void Start()
    {
        instance = this;

        allNavPoints = new List<NavigablePoint>();
        sortedNavPoints = new List<AreaNavPoints>();
        foreach(ELoadArea areaType in Enum.GetValues(typeof(ELoadArea))) {
            AreaNavPoints newAreaNavPoints = Instantiate(areaNavPointsPrefab);
            newAreaNavPoints.area = areaType;
            newAreaNavPoints.navPoints = new List<NavigablePoint>();
            sortedNavPoints.Add(newAreaNavPoints);
        }
    }



    public void AddNavigablePoint(NavigablePoint newNavPoint) {
        allNavPoints.Add(newNavPoint);

        foreach(ELoadArea area in newNavPoint.areas) {
            AddNavigablePointByArea(area, newNavPoint);
        }
    }



    void AddNavigablePointByArea(ELoadArea area, NavigablePoint newNavPoint) {
        foreach(AreaNavPoints areaNavPoints in sortedNavPoints) {
            if (areaNavPoints.area == area) {
                areaNavPoints.navPoints.Add(newNavPoint);
                return;
            }
        }
    }
    

    
    public Vector3 GetRandomNavigablePoint() {
        return allNavPoints[UnityEngine.Random.Range(0, allNavPoints.Count)].transform.position;
    }



    public Vector3 GetNavigablePointInArea(ELoadArea[] validAreas) {
        ELoadArea area = validAreas[UnityEngine.Random.Range(0, validAreas.Length)];

        foreach(AreaNavPoints areaNavPoints in sortedNavPoints) {
            if (areaNavPoints.area == area) {
                //print("SUCCESSFULLY FOUND POINT IN " + area);
                return areaNavPoints.GetRandomItem().transform.position;
            }
        }

        //print("FAILED TO FIND POINT IN AREA " + area);
        return Vector3.zero;

        /*
        for (int i = 0; i < maxChecksPerRandomPointFind; i++) {
            NavigablePoint currPoint = navPoints[Random.Range(0, navPoints.Count)];

            foreach (ELoadArea area in currPoint.areas) {
                if (validAreas.Contains(area)) {
                    return currPoint.transform.position;
                }
            }
        }
        */
    }
}