using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ESpeaker
{
    CHARACTER = 0,
    PLAYER = 1,
    PLAYERTHINKTOSELF = 2
}

[System.Serializable]
public class Dialogue
{
    public ESpeaker speaker;

    public Phrase[] phrases = new Phrase[1];
}
