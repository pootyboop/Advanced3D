using System.Collections;
using System.Collections.Generic;
using UnityEngine;



//stores information about languages to be referenced by translation UI
//i also reference this during development for other stuff like what colors/icons banners should use
[System.Serializable]
public class LanguageDataMap : MonoBehaviour
{
    public static LanguageDataMap instance;

    public LanguageData[] languageDataList = new LanguageData[1];   //languages tied to their icons and colors. managed in inspector



    void Start()
    {
        instance = this;
    }



    //returns the Color of a language
    public Color GetLanguageColor(ELanguage language)
    {
        foreach (LanguageData currLang in languageDataList)
        {
            if (language == currLang.language)
            {
                return currLang.color;
            }
        }

        //worst case, just return white
        return new Color(1, 1, 1);
    }



    //returns the hexcode color of a language
    //used for in-text formatting where unity's Color doesn't work
    //return does not include the # at the start
    public string GetLanguageHexCode(ELanguage language)
    {
        foreach (LanguageData currLang in languageDataList)
        {
            if (language == currLang.language)
            {
                return ColorUtility.ToHtmlStringRGB(currLang.color);
            }
        }

        //worst case, just return white
        return "FFFFFF";
    }



    //returns the icon of a language
    public Sprite GetLanguageIcon(ELanguage language)
    {
        foreach (LanguageData currLang in languageDataList)
        {
            if (language == currLang.language)
            {
                return currLang.icon;
            }
        }

        //no icon for the given language
        return null;
    }


    
    //returns the language with the requested ID
    //used by the translator slider to convert slider value into language
    public ELanguage GetLanguageByID(int ID)
    {
        for (int i = 0; i < languageDataList.Length; i++)
        {
            if (ID == i)
            {
                return languageDataList[i].language;
            }
        }

        //no language at that ID
        return ELanguage.UNSET;
    }


    //returns the language ID of a language
    //used to set the translator slider value to the currently translated language
    public int GetIDByLanguage(ELanguage language)
    {
        for (int i = 0; i < languageDataList.Length; i++)
        {
            if (language == languageDataList[i].language)
            {
                return i;
            }
        }

        //no ID for that language
        return -1;
    }
}
