using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;



//handles UI for a single phrase in a DialogueBox
public class PhraseText : MonoBehaviour
{
    private TMP_Text text;
    private ELanguage trueLanguage; //language this phrase uses



    //set the text
    public void UpdateText(Phrase phrase, ELanguage speakerLanguage, ELanguage translatedLanguage, bool usePlainText)
    {
        text = gameObject.GetComponent<TMP_Text>();

        //determine what language this phrase is
        if (phrase.overrideLanguage == ELanguage.UNSET)
        {
            //if language was not overriden, use speaker's base language
            trueLanguage = speakerLanguage;
        }
        else
        {
            //otherwise, override the language
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



    //creates the string to be used by the TMPro text
    //all text (phrase, translation, and context) use the same TMPro text with in-line formatting
    private string CreateTMProText(Phrase phrase, ELanguage language, ELanguage translatedLanguage)
    {
        //phrase text and translation
        string TMProText =
            "<color=#" +
            LanguageDataMap.instance.GetLanguageHexCode(language) + //translatable text color
            ">" +
            phrase.phrase +
            "\n<size=50%><color=#AAAAAA>|<size=100%>\n";    //color/formatting for translation

        if (language == translatedLanguage)
        {
            TMProText +=
                "<color=#FFFFFF>" + //white, easy-to-read text when translated
                phrase.translation;
        }

        else
        {
            TMProText += "<color=#666666>?";    //grayed out when not translated
        }


        TMProText += "\n<size=40%>\n<color=#AAAAAA>";   //formatting for context

        //add the context if it exists and if the phrase is translated
        if (language == translatedLanguage && phrase.context != null && phrase.context != "")
        {
            TMProText +=
                "(" +
                phrase.context +
                ")";
        }

        else
        {
            //hyphen for formatting so that phrases with actual context fields aren't offset/smaller than the rest
            //admittedly not the cleanest solution
            TMProText += "-";
        }

        return TMProText;
    }



    //click phrase to auto-tune translator to the clicked phrase's language
    //this function is bound to the invisible button on the PhraseText prefab
    public void SetTranslatorLanguageToMatch()
    {
        if (trueLanguage != Translator.instance.language) {
            AudioManager.instance.PlayAudioByTag("button");
        Translator.instance.SetTranslatorLanguage(trueLanguage);
        }
    }
}
