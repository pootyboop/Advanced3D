using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

/*
    Awake(), Director_Played(), and Director_Stopped() are from Unity Learn.
    Author: Unity Technologies
    https://learn.unity.com/tutorial/starting-timeline-through-a-c-script-2019-3#5ff8d183edbc2a0020996601
    Accessed: 18/12/2023
*/



//manages in-game cutscenes made with the timeline
public class CutsceneManager : MonoBehaviour
{
    public static CutsceneManager instance;

    public PlayableDirector introCutscene;  //cutscene that plays when the game begins
    public Camera cutsceneCam;  //cinematic camera used in cutscenes instead of the player camera
    public bool playIntroCutscene = true;   //easy toggle to turn off the intro cutscene during development



    void Start()
    {
        instance = this;

        //automatically plays the intro cutscene on start unless turned off
        if (playIntroCutscene)
        {
            introCutscene.Play();
        }
    }



    private void Awake()
    {
        //events for when the director starts and stops (when cutscenes begin and end)
        introCutscene.played += Director_Played;
        introCutscene.stopped += Director_Stopped;
    }



    //called when a cutscene begins
    private void Director_Played(PlayableDirector obj)
    {
        cutsceneCam.gameObject.SetActive(true);
    }



    //called when a cutscene ends
    private void Director_Stopped(PlayableDirector obj)
    {
        cutsceneCam.gameObject.SetActive(false);
    }
}
