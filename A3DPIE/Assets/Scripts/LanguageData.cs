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
public class LanguageData
{
    public ELanguage language;

    public Color color;
    public Texture2D icon;
}