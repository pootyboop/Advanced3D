using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PhraseText : MonoBehaviour
{
    private TMP_Text text;
    private ELanguage trueLanguage;



    public void UpdateText(Phrase phrase, ELanguage speakerLanguage, bool usePlainText)
    {
        text = gameObject.GetComponent<TMP_Text>();

        //determine what language this phrase is
        if (phrase.overrideLanguage == ELanguage.UNSET)
        {
            //language was not overriden, use speaker's base language
            trueLanguage = speakerLanguage;
        }
        else
        {
            //override language
            trueLanguage = phrase.overrideLanguage;
        }



        //use plain english text in dialogue where the player is thinking to themself
        if (usePlainText)
        {
            text.text = phrase.phrase;
        }

        //otherwise do the fancy translation stuff
        else
        {
            text.text = CreateTMProText(phrase, trueLanguage);
        }
    }



    private string CreateTMProText(Phrase phrase, ELanguage language)
    {
        //phrase text and translation
        string TMProText =
            "<color=#" +
            LanguageDataMap.instance.GetLanguageHexCode(language) +
            //GetLanguageHexColor(language) +
            ">" +
            phrase.phrase +
            "\n<size=50%><color=#AAAAAA>|<size=100%><color=#FFFFFF>\n" +
            phrase.translation +
            "\n<size=40%>";

        //context
        if (phrase.context != null && phrase.context != "")
        {
            TMProText +=
                "<color=#666666>(" +
                phrase.context +
                ")";
        }

        return TMProText;
    }



    private string GetLanguageHexColor(ELanguage language)
    {
        return "FF0000";
    }
}
