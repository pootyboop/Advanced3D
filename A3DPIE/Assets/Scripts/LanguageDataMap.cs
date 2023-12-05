using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//struct LangData
//{
//    Color color;
//    Texture2D icon;

//    LangData(Color color_, Texture2D icon_)
//    {
//        color = color_;
//        icon = icon_;
//    }
//}

[System.Serializable]
public class LanguageDataMap : MonoBehaviour
{
    //can't serialize dictionaries. thanks unity.
    //public Dictionary<ELanguage, LanguageData> languageDataMap = new Dictionary<ELanguage, LanguageData>()
    //{
    //    {ELanguage.HIESCA, new LangData()}
    //};

    public static LanguageDataMap instance;

    public LanguageData[] languageDataList = new LanguageData[1];

    void Start()
    {
        instance = this;
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

        return "000000";
    }
}
