using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ELanguage
{
    UNSET,
    HIESCA,
    HIESORSCA,
    HIEKIESCA,
    VOENNSCA,
    RONESCA,
    YUESCA,
    IRSCA,
    LUNSCA,
    PRIEDESCA,
    SORPRI,
    GURESSCA
}

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