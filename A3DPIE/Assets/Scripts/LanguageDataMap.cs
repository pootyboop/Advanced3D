using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LanguageDataMap : MonoBehaviour
{
    public static LanguageDataMap instance;

    public LanguageData[] languageDataList = new LanguageData[1];



    void Start()
    {
        instance = this;
    }



    public Color GetLanguageColor(ELanguage language)
    {
        for (int i = 0; i < languageDataList.Length; i++)
        {
            if (language == languageDataList[i].language)
            {
                return languageDataList[i].color;
            }
        }

        //worst case, just return white
        return new Color(1, 1, 1);
    }



    public string GetLanguageHexCode(ELanguage language)
    {
        for (int i = 0; i < languageDataList.Length; i++)
        {
            if (language == languageDataList[i].language)
            {
                return ColorUtility.ToHtmlStringRGB(languageDataList[i].color);
            }
        }

        //worst case, just return white
        return "FFFFFF";
    }



    public Sprite GetLanguageIcon(ELanguage language)
    {
        for (int i = 0; i < languageDataList.Length; i++)
        {
            if (language == languageDataList[i].language)
            {
                return languageDataList[i].icon;
            }
        }

        return null;
    }



    public ELanguage GetLanguageByID(int ID)
    {
        for (int i = 0; i < languageDataList.Length; i++)
        {
            if (ID == i)
            {
                return languageDataList[i].language;
            }
        }

        return ELanguage.UNSET;
    }



    public int GetIDByLanguage(ELanguage language)
    {
        for (int i = 0; i < languageDataList.Length; i++)
        {
            if (language == languageDataList[i].language)
            {
                return i;
            }
        }

        return -1;
    }
}
