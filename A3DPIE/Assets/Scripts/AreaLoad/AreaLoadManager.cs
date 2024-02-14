using System.Collections;
using System.Collections.Generic;
using UnityEngine;



//manages loading areas of the level in and out by activating/deactivating their parent objects
//relies on PlayerMovement to tell it when we're entering a new area
//also relies on Door to tell it when to load in/out the area behind a door
public class AreaLoadManager : MonoBehaviour
{
    public static AreaLoadManager instance;
    public Area[] areas;    //all area data
    public ELoadArea currentArea = ELoadArea.F2;    //start on F2 as that's where we spawn

    //LEAVE THIS DISABLED!
    //auto-enables after the first few seconds so characters and other scripts have time to get set up before they get disabled
    public bool enableAreaLoader = false;



    void Start()
    {
        instance = this;

        //wait a couple seconds, then start the area loader.
        //this allows characters/animations to get set up properly before being disabled (if unloaded)
        StartCoroutine(DelayedInitialLoad());
    }



    IEnumerator DelayedInitialLoad()
    {
        //wait a few seconds
        yield return new WaitForSeconds(3.0f);

        //now we can enable the area loader
        enableAreaLoader = true;

        //start with just the F2 area active (since that's where the game starts)
        UnloadAll();
        LoadNewArea(ELoadArea.F2);

    }



    //unloads all areas
    //to be used as a clean reset from which to load an area in
    public void UnloadAll()
    {
        SetAreaLoaded(ELoadArea.F0, false);
        SetAreaLoaded(ELoadArea.F1, false);
        SetAreaLoaded(ELoadArea.F2, false);
        SetAreaLoaded(ELoadArea.F0F2STAIRWELL, false);
        SetAreaLoaded(ELoadArea.F0F1STAIRWELL, false);
    }



    //called by the player when overlapping an area's hitbox
    //hitboxes are spaced a bit to prevent spam
    public void EnteredAreaHitbox(GameObject newArea)
    {
        if (!enableAreaLoader)
        {
            return;
        }



        //find the corresponding parent object for the area and load it
        foreach (Area curr in areas)
        {
            if (curr.reference == newArea)
            {
                LoadNewArea(curr.areaName);
            }
        }
    }



    //loads a new area and adjacent areas ("coloaded" areas) that should load in with it
    //also unloads any old coloaded areas
    public void LoadNewArea(ELoadArea newArea)
    {
        if (!enableAreaLoader)
        {
            return;
        }



        //setup the correct fog data
        GraphicsManager.instance.ChangeFogAppearance(GetAreaByEnum(newArea).fogData);



        //areas to load in no matter what
        List<ELoadArea> loadAreas = new List<ELoadArea>(GetAreaByEnum(newArea).coloadedAreas);
        loadAreas.Add(newArea);
        
        //potential areas to unload. may contain areas we need to load in
        List<ELoadArea> newUnloadAreas = new List<ELoadArea>(GetAreaByEnum(currentArea).coloadedAreas);
        newUnloadAreas.Add(currentArea);

        //will contain all definitive areas to unload
        List<ELoadArea> finalUnloadAreas = new List<ELoadArea>();


        //create final list of areas to unload. this list excludes areas we intend to load in.
        foreach (ELoadArea currArea in newUnloadAreas)
        {
            //do not unload any areas we need to load in
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

        //set the current area to the new area for next time
        currentArea = newArea;
    }



    //directly load or unload an area
    //does NOT take coloaded areas into account. use with caution
    //called by Door to directly load/unload whatever's around the door
    public void SetAreaLoaded(ELoadArea area, bool isLoaded)
    {

        if (!enableAreaLoader)
        {
            return;
        }



        //never allowed to unload the area the player is currently in
        if (area == currentArea && !isLoaded)
        {
            return;
        }

        //grab the area data from its enum name
        Area loadArea = GetAreaByEnum(area);

        //only use SetActive if not already in the intended active/deactive state
        if (loadArea.reference.activeSelf != isLoaded)
        {
            loadArea.reference.SetActive(isLoaded);
        }
    }



    //gets the area data for an area's enum name
    Area GetAreaByEnum(ELoadArea area)
    {
        foreach (Area currArea in areas)
        {
            if (currArea.areaName == area)
            {
                return currArea;
            }
        }

        return null;
    }
}
