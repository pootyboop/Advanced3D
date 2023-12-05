using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using static System.Net.Mime.MediaTypeNames;

public class DialogueBox : MonoBehaviour
{
    public PhraseText phraseTextPrefab;
    public TMP_Text speakerName;
    private PhraseText[] spawnedPhraseTexts;

    private Dialogue currentDialogue;
    private string currentSpeaker;
    private ELanguage currentLanguage;



    public void InitializeDialogueBox(Dialogue dialogue, string speaker, ELanguage language, ELanguage translatedLanguage)
    {
        currentDialogue = dialogue;
        currentSpeaker = speaker;
        currentLanguage = language;

        bool usePlainText;
        if (speaker == "")
        {
            usePlainText = true;
            speakerName.gameObject.SetActive(false);
        }
        else
        {
            usePlainText = false;
            speakerName.gameObject.SetActive(true);
            speakerName.text = "- " + speaker + " -";
        }

        InitializePhrases(currentDialogue.phrases, usePlainText, currentLanguage, translatedLanguage);
    }



    public void InitializeDialogueBox(ELanguage translatedLanguage)
    {
        InitializeDialogueBox(currentDialogue, currentSpeaker, currentLanguage, translatedLanguage);
    }



    private void InitializePhrases(Phrase[] phrases, bool usePlainText, ELanguage language, ELanguage translatedLanguage)
    {
        CleanupPhrases();
        spawnedPhraseTexts = new PhraseText[phrases.Length];



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



    public void CleanupPhrases()
    {
        if (spawnedPhraseTexts == null)
        {
            return;
        }

        for (int i = 0; i < spawnedPhraseTexts.Length; i++)
        {
            Destroy(spawnedPhraseTexts[i].gameObject);
        }

        spawnedPhraseTexts = null;
    }



    public void EndDialogue()
    {
        speakerName.gameObject.SetActive(false);
    }
}
