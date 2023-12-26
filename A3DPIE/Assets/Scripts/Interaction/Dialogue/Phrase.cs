using System.Collections;
using System.Collections.Generic;
using UnityEngine;



//contains data for a single translatable phrase in a single language
[System.Serializable]
public class Phrase
{
    public ELanguage overrideLanguage;  //if not UNSET, this will override the speaker's native language with the desired language

    [TextArea(2,5)]
    public string phrase;   //the text in non-translated language

    [TextArea(2, 5)]
    public string translation;  //the translation of the phrase text

    public string context;  //additional optional context. used to clarify confusing translations (e.g. sayings that don't translate word-for-word to english)
}