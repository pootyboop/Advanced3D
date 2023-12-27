using System.Collections;
using System.Collections.Generic;
using UnityEngine;



//all languages
public enum ELanguage
{
    UNSET = 0,      //Used by Phrase to indicate the phrase does not differ from the character's native language
    HIESCA = 1,     //Common language, use this when in doubt
    HIESORSCA = 2,  //Variant of Hiesca sometimes used by southern planets
    HIEKIESCA = 3,  //Variant of Hiesca sometimes used by northern planets
    VOENNSCA = 4,   //Voenn (planet the game takes place on) planetary language
    RONESCA = 5,    //Ronetto planetary language
    YUESCA = 6,     //Yuetta planetary language
    IRSCA = 7,      //Irsen planetary language
    LUNSCA = 8,     //Lunscu planetary language
    PRIEDESCA = 9,  //Priedenne planetary language (used mostly in north)
    SORPRI = 10,    //Southern Priedenne dialect, used by enough people to justify being in translator
    GURESSCA = 11   //Guressen (protagonist's home planet) planetary language
}



//container for associating a language with its color and icon (usually just the planet icon)
[System.Serializable]
public class LanguageData
{
    public ELanguage language;

    public Color color; //color to be used with this language in text, on banners, etc.
    public Sprite icon; //icon to represent this language
}