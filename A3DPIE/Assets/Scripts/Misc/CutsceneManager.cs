using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;



//the game's current state in relation to cutscenes
public enum ECutsceneState
{
    GAME,
    INTRO,
    OUTRO
}

/*
    Start(), Director_Played(), and Director_Stopped() are from Unity Learn.
    Author: Unity Technologies
    https://learn.unity.com/tutorial/starting-timeline-through-a-c-script-2019-3#5ff8d183edbc2a0020996601
    Accessed: 18/12/2023
*/



//manages in-game cutscenes made with the timeline
public class CutsceneManager : MonoBehaviour
{
    public static CutsceneManager instance;

    public PlayableDirector introCutscene, outroCutscene;  //cutscene references
    public Camera cutsceneCam;  //cinematic camera used in cutscenes instead of the player camera

    public CanvasGroupFader skipText;

    //easy toggles to turn off cutscenes during development
    public bool playIntroCutscene = true;
    public bool playOutroCutscene = true;

    public ECutsceneState currCutscene; //current cutscene


    void Start()
    {
        instance = this;

        cutsceneCam.gameObject.SetActive(false);

        //events for when the director starts and stops (when cutscenes begin and end)
        //Unity's code begins here.
        introCutscene.played += Director_Played;
        introCutscene.stopped += Director_Stopped;
        outroCutscene.played += Director_Played;
        outroCutscene.stopped += Director_Stopped;
        //Unity's code ends here.

        //automatically plays the intro cutscene on start unless turned off (bool playIntroCutscene)
        PlayIntroCutscene();
    }



    //called when a cutscene begins
    private void Director_Played(PlayableDirector obj)
    {
        PlayerMovement.instance.SetPlayerState(EPlayerState.CUTSCENE);
        cutsceneCam.gameObject.SetActive(true);
        skipText.FadeAlpha(true);
    }



    //called when a cutscene ends
    private void Director_Stopped(PlayableDirector obj)
    {
        skipText.SetAlpha(0.0f);
        currCutscene = ECutsceneState.GAME;
        cutsceneCam.gameObject.SetActive(false);
        PlayerMovement.instance.SetPlayerState(EPlayerState.MOVABLE);
    }



    //plays the intro cutscene if allowed to
    public void PlayIntroCutscene()
    {
        if (playIntroCutscene)
        {
            currCutscene = ECutsceneState.INTRO;
            introCutscene.Play();
        }
    }



    //plays the outro cutscene if allowed to
    public void PlayOutroCutscene()
    {
        if (playOutroCutscene)
        {
            currCutscene = ECutsceneState.OUTRO;
            outroCutscene.Play();
        }
    }



    //skips the cutscene
    public void SkipCutscene()
    {
        PlayableDirector currentCutscene;

        //get the playabledirector from currCutscene
        switch (currCutscene)
        {
            default:
                return;
            case ECutsceneState.INTRO:
                currentCutscene = introCutscene;
                break;
            case ECutsceneState.OUTRO:
                currentCutscene = outroCutscene;
                break;
        }

        currentCutscene.time = currentCutscene.duration;
        currentCutscene.Evaluate();
    }
}
