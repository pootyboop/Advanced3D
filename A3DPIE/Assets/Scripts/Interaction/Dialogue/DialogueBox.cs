using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using static System.Net.Mime.MediaTypeNames;



//displays a dialogue on a UI element
//this script is persistent and is not recreating for each new dialogue
//instead, new data is passed to it and it modifies the existing UI element
public class DialogueBox : MonoBehaviour
{
    public PhraseText phraseTextPrefab; //prefab to generate phrases from
    public TMP_Text speakerName;        //text displaying the speaker
    private PhraseText[] spawnedPhraseTexts;    //all PhraseTexts being used to display the main body text

    private Dialogue currentDialogue;   //the dialogue being used
    private string currentSpeaker;      //used to format dialogue
    private ELanguage currentLanguage;  //default language for when phrases do not have an override language



    //set up the dialogue box with a new dialogue
    public void InitializeDialogueBox(Dialogue dialogue, string speaker, ELanguage language, ELanguage translatedLanguage)
    {
        currentDialogue = dialogue;
        currentSpeaker = speaker;
        currentLanguage = language;

        bool usePlainText;

        //player is thinking to themself, so use plain text with no translation necessary
        if (speaker == "")
        {
            usePlainText = true;
            speakerName.gameObject.SetActive(false);    //hide the speaker name since this is just the player's thought to themself

            Translator.instance.gameObject.SetActive(false);    //no translator necessary
        }

        //dialogue is publicly spoken/read and must be translated
        else
        {
            usePlainText = false;
            speakerName.gameObject.SetActive(true);
            speakerName.text = "- " + speaker + " -";   //add detail around speaker name

            Translator.instance.gameObject.SetActive(true); //display translator for... translating
        }

        InitializePhrases(currentDialogue.phrases, usePlainText, currentLanguage, translatedLanguage);
    }



    //set up the dialogue box with a new dialogue
    //this override is used when the player changes their translated language and no other new dialogue information is necessary
    public void InitializeDialogueBox(ELanguage translatedLanguage)
    {
        InitializeDialogueBox(currentDialogue, currentSpeaker, currentLanguage, translatedLanguage);
    }



    //populate the dialogue box with the phrases that constitute the dialogue
    private void InitializePhrases(Phrase[] phrases, bool usePlainText, ELanguage language, ELanguage translatedLanguage)
    {
        //clear old phrases
        CleanupPhrases();
        spawnedPhraseTexts = new PhraseText[phrases.Length];


        //add the new phrases
        for (int i = 0; i < phrases.Length; i++)
        {
            //spawn the phrase
            PhraseText currentPhraseText = Instantiate(phraseTextPrefab);
            //save a ref to delete it later
            spawnedPhraseTexts[i] = currentPhraseText;
            //make it a child of this dialogue box
            currentPhraseText.transform.SetParent(transform, false);
            //set up its text
            currentPhraseText.UpdateText(phrases[i], language, translatedLanguage, usePlainText);
        }
    }



    //delete any old leftover phrases
    public void CleanupPhrases()
    {
        if (spawnedPhraseTexts == null)
        {
            return;
        }

        foreach (PhraseText currPhrase in spawnedPhraseTexts)
        {
            Destroy(currPhrase.gameObject);
        }

        spawnedPhraseTexts = null;
    }



    public void EndDialogue()
    {
        speakerName.gameObject.SetActive(false);
    }
}
