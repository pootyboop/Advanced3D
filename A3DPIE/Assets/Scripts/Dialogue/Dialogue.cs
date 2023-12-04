using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ESpeaker
{
    CHARACTER,
    PLAYER,
    PLAYERTHINKTOSELF
}

[System.Serializable]
public class Dialogue
{
    public ESpeaker speaker;

    public Phrase[] phrases = new Phrase[1];
}
