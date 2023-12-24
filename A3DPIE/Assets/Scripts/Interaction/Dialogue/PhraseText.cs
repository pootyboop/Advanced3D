using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PhraseText : MonoBehaviour
{
    private TMP_Text text;
    private ELanguage trueLanguage;



    public void UpdateText(Phrase phrase, ELanguage speakerLanguage, ELanguage translatedLanguage, bool usePlainText)
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
            text.text = CreateTMProText(phrase, trueLanguage, translatedLanguage);
        }
    }



    private string CreateTMProText(Phrase phrase, ELanguage language, ELanguage translatedLanguage)
    {
        //phrase text and translation
        string TMProText =
            "<color=#" +
            LanguageDataMap.instance.GetLanguageHexCode(language) +
            ">" +
            phrase.phrase +
            "\n<size=50%><color=#AAAAAA>|<size=100%>\n";

        if (language == translatedLanguage)
        {
            TMProText +=
                "<color=#FFFFFF>" +
                phrase.translation;
        }

        else
        {
            TMProText += "<color=#666666>?";
        }


        TMProText += "\n<size=40%>\n<color=#AAAAAA>";

        //context
        if (language == translatedLanguage && phrase.context != null && phrase.context != "")
        {
            TMProText +=
                "(" +
                phrase.context +
                ")";
        }

        else
        {
            //for formatting so that phrases with actual context fields aren't offset/smaller than the rest
            TMProText += "-";
        }

        return TMProText;
    }



    public void SetTranslatorLanguageToMatch()
    {
        Translator.instance.SetTranslatorLanguage(trueLanguage);
    }
}
