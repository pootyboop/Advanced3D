using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class CutsceneManager : MonoBehaviour
{
    public static CutsceneManager instance;

    public PlayableDirector introCutscene;
    public Camera cutsceneCam;
    public bool playIntroCutscene = true;

    void Start()
    {
        instance = this;

        if (playIntroCutscene)
        {
            introCutscene.Play();
            cutsceneCam.gameObject.SetActive(true);
        }
    }


    //Awake, Director_Played, and Director_Stopped from:
    //https://learn.unity.com/tutorial/starting-timeline-through-a-c-script-2019-3#5ff8d183edbc2a0020996601
    private void Awake()
    {
        introCutscene.played += Director_Played;
        introCutscene.stopped += Director_Stopped;
    }



    private void Director_Played(PlayableDirector obj)
    {

    }



    private void Director_Stopped(PlayableDirector obj)
    {
        cutsceneCam.gameObject.SetActive(false);
    }
}
