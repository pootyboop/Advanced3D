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



    public void InitializeDialogueBox(Dialogue dialogue, string speaker, ELanguage language)
    {

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

        InitializePhrases(dialogue.phrases, usePlainText, language);
    }



    private void InitializePhrases(Phrase[] phrases, bool usePlainText, ELanguage language)
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
            currentPhraseText.transform.parent = transform;
            currentPhraseText.UpdateText(phrases[i], language, usePlainText);
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
