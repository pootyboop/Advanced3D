using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;



//manages the translator used for translating PhraseTexts in conversations
public class Translator : MonoBehaviour
{
    public static Translator instance;

    public TMP_Text languageText;   //text UI for the currently translated language
    public Image sliderHandle;      //the slider handle
    public Slider languageSlider;   //the translator UI slider
    public Image languageIcon;      //language icon on the slider handle
    public ELanguage language = ELanguage.HIESCA;   //currently translated language



    void Start()
    {
        instance = this;
        SetTranslatorLanguage(ELanguage.HIESCA);    //set to Hiesca, the common language, by default
        gameObject.SetActive(false);    //disable on Start so it can set its default language and then go away until the player enters a conversation
    }



    //set the language by the slider value
    public void OnSliderChanged(float value)
    {
        SetTranslatorLanguageByID((int)value);
    }



    //set language by clicking PhraseText
    //also updates slider
    public void SetTranslatorLanguage(ELanguage newLanguage)
    {
        language = newLanguage;

        //turn the value from the slider into a language ID and then set that language as the new translated language
        languageSlider.value = LanguageDataMap.instance.GetIDByLanguage(language);

        TranslatorLanguageUpdated();
    }



    //set language from slider value
    public void SetTranslatorLanguageByID(int ID)
    {
        language = LanguageDataMap.instance.GetLanguageByID(ID);
        TranslatorLanguageUpdated();
    }



    //called when the translated language has changed
    private void TranslatorLanguageUpdated()
    {
        languageText.text =
            "<color=#" +
            LanguageDataMap.instance.GetLanguageHexCode(language) + //color the language its respective color using in-text formatting
            ">" +
            language.ToString();

        sliderHandle.color = LanguageDataMap.instance.GetLanguageColor(language);   //also set the slider handle to that color

        languageIcon.sprite = LanguageDataMap.instance.GetLanguageIcon(language);   //set the slider handle to display the language icon

        //and finally, update the dialogue box UI if in a conversation
        if (DialogueManager.instance != null)
        {
            DialogueManager.instance.OnTranslatorLanguageChanged(language);
        }
    }
}
