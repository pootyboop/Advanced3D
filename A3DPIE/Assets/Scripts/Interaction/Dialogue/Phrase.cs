using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Phrase
{
    public ELanguage overrideLanguage;

    [TextArea(2,5)]
    public string phrase;

    [TextArea(2, 5)]
    public string translation;

    public string context;
}