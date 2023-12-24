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
    public Image languageIcon;
    public ELanguage language = ELanguage.HIESCA;



    void Start()
    {
        instance = this;
        SetTranslatorLanguage(ELanguage.HIESCA);
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
            "<color=#" +
            LanguageDataMap.instance.GetLanguageHexCode(language) +
            ">" +
            language.ToString();

        sliderHandle.color = LanguageDataMap.instance.GetLanguageColor(language);

        languageIcon.sprite = LanguageDataMap.instance.GetLanguageIcon(language);

        if (DialogueManager.instance != null)
        {
            DialogueManager.instance.OnTranslatorLanguageChanged(language);
        }
    }
}
