using System.Collections;
using System.Collections.Generic;
using UnityEngine;



//the current speaker in the conversation OR where the dialogue is originating from
public enum ESpeaker
{
    CHARACTER = 0,  //the character is speaking translatable language, or the player is reading translatable text
    PLAYER = 1,     //the player is speaking aloud in translatable language
    PLAYERTHINKTOSELF = 2   //the player is thinking to themselves in english with no translation or character name. just raw text
}



//contains data for a single dialogue (one dialogue box on-screen)
[System.Serializable]
public class Dialogue
{
    public ESpeaker speaker;    //where the dialogue is originating from
    public Phrase[] phrases = new Phrase[1];    //the phrases that constitute this dialogue
}
