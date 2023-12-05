using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Translator : MonoBehaviour
{
    public static Translator instance;

    public TMP_Text languageText;
    public Image sliderHandle;
    public Slider languageSlider;
    public ELanguage language = ELanguage.HIESCA;



    void Start()
    {
        instance = this;
        gameObject.SetActive(false);
    }



    public void OnSliderChanged(float value)
    {
        SetTranslatorLanguageByID((int)value);
    }



    //set language by clicking PhraseText
    //also updates slider
    public void SetTranslatorLanguage(ELanguage newLanguage)
    {
        language = newLanguage;

        //update slider value
        languageSlider.value = LanguageDataMap.instance.GetIDByLanguage(language);

        TranslatorLanguageUpdated();
    }



    //set language from slider value
    public void SetTranslatorLanguageByID(int ID)
    {
        language = LanguageDataMap.instance.GetLanguageByID(ID);
        TranslatorLanguageUpdated();
    }



    private void TranslatorLanguageUpdated()
    {
        languageText.text =
            "<size=50%><color=#DDDDDD>Translator:\n<size=100%><color=#" +
            LanguageDataMap.instance.GetLanguageHexCode(language) +
            ">" +
            language.ToString();

        sliderHandle.color = LanguageDataMap.instance.GetLanguageColor(language);

        if (DialogueManager.instance == null)
        {
            return;
        }

        DialogueManager.instance.OnTranslatorLanguageChanged(language);
    }
}