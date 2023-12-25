using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaLoadManager : MonoBehaviour
{
    public static AreaLoadManager instance;
    public Area[] areas;
    public ELoadArea currentArea = ELoadArea.F2;



    void Start()
    {
        instance = this;

        //wait a couple seconds, then set up the load.
        //this allows characters/animations to get set up properly before being disabled (if unloaded)
        StartCoroutine(DelayedInitialLoad());
    }



    IEnumerator DelayedInitialLoad()
    {
        yield return new WaitForSeconds(3.0f);

        //start with just the F2 area active (since that's where the game starts)
        UnloadAll();
        LoadNewArea(ELoadArea.F2);

    }



    //unloads all areas. to be used as a clean reset from which to load an area in
    public void UnloadAll()
    {
        SetAreaLoaded(ELoadArea.F0, false);
        SetAreaLoaded(ELoadArea.F1, false);
        SetAreaLoaded(ELoadArea.F2, false);
        SetAreaLoaded(ELoadArea.F0F2STAIRWELL, false);
        SetAreaLoaded(ELoadArea.F0F1STAIRWELL, false);
    }



    //called by the player when overlapping an area's hitbox
    public void EnteredAreaHitbox(GameObject newArea)
    {
        for (int i = 0; i < areas.Length; i++)
        {
            if (areas[i].reference == newArea)
            {
                LoadNewArea(areas[i].areaName);
            }
        }
    }



    //loads a new area and adjacent areas that should load in with it.
    //also unloads any old non-adjacent areas
    public void LoadNewArea(ELoadArea newArea)
    {
        //areas to load in
        List<ELoadArea> loadAreas = new List<ELoadArea>(GetAreaByEnum(newArea).coloadedAreas);
        loadAreas.Add(newArea);
        
        //all potential areas to load out
        List<ELoadArea> newUnloadAreas = new List<ELoadArea>(GetAreaByEnum(currentArea).coloadedAreas);
        newUnloadAreas.Add(currentArea);

        //will contain all definitive areas to load out
        List<ELoadArea> finalUnloadAreas = new List<ELoadArea>();


        //create final list of areas to unload. this list excludes areas we intend to load in.
        foreach (ELoadArea currArea in newUnloadAreas)
        {
            if (!loadAreas.Contains(currArea))
            {
                finalUnloadAreas.Add(currArea);
            }
        }



        //load areas
        foreach (ELoadArea currArea in loadAreas)
        {
            SetAreaLoaded(currArea, true);
        }

        //unload areas
        foreach (ELoadArea currArea in finalUnloadAreas)
        {
            SetAreaLoaded(currArea, false);
        }


        //SetColoadedAreasLoaded(currentArea, false);

        currentArea = newArea;

        //SetColoadedAreasLoaded(newArea, true);
    }



    void SetColoadedAreasLoaded(ELoadArea area, bool isLoaded)
    {
        ELoadArea[] coloadedAreas = GetAreaByEnum(area).coloadedAreas;
        for (int i = 0; i < coloadedAreas.Length; i++)
        {
            SetAreaLoaded(coloadedAreas[i], isLoaded);
        }
    }



    //called by doors to directly load/unload whatever's around the door
    public void SetAreaLoaded(ELoadArea area, bool isLoaded)
    {
        //never allowed to unload the area the player is currently in
        if (area == currentArea && !isLoaded)
        {
            return;
        }

        //Area currArea = GetAreaByEnum(currentArea);

        //for (int i = 0; i < currArea.coloadedAreas.Length; i++)
        //{
        //    if (currArea.coloadedAreas[i])
        //    {

        //    }
        //}



        Area loadArea = GetAreaByEnum(area);

        //only use SetActive if not already in the intended state
        if (loadArea.reference.activeSelf != isLoaded)
        {
            loadArea.reference.SetActive(isLoaded);
        }
    }



    Area GetAreaByEnum(ELoadArea area)
    {
        for (int i = 0; i < areas.Length; i++)
        {
            if (areas[i].areaName == area)
            {
                return areas[i];
            }
        }

        return null;
    }
}
