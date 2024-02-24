using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaNavPoints : MonoBehaviour
{
    public ELoadArea area;
    public List<NavigablePoint> navPoints;



    public NavigablePoint GetRandomItem() {
        return navPoints[Random.Range(0, navPoints.Count)];
    }
}
