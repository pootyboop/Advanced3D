using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance;

    public DialogueBox dialogueBox;

    private bool inDialogue = false;

    DialogueCharacter character;
    string name;
    ELanguage language;
    Conversation conversation;
    int dialogueIndex;

    EPlayerState previousPlayerState;



    void Start()
    {
        instance = this;
    }



    public void StartConversation(DialogueCharacter trigger, Conversation currentConversation)
    {
        inDialogue = true;

        character = trigger;
        name = character.name;
        language = character.spokenLanguage;
        conversation = currentConversation;

        previousPlayerState = PlayerMovement.instance.state;
        PlayerMovement.instance.SetPlayerState(EPlayerState.DIALOGUE);
        dialogueIndex = 0;

        dialogueBox.gameObject.SetActive(true);

        Translator.instance.gameObject.SetActive(true);

        NextDialogue(false);

        //default to common language for every new conversation
        //KEEP THIS AFTER NextDialogue()
        Translator.instance.SetTranslatorLanguage(ELanguage.HIESCA);
    }



    public void NextDialogue(bool showNextDialogue)
    {

        if (showNextDialogue)
        {
            dialogueIndex++;
        }

        if (dialogueIndex >= conversation.dialogues.Length)
        {
            EndConversation();
            return;
        }

        Dialogue dialogue = conversation.dialogues[dialogueIndex];

        character.SetDialogueState(dialogue.speaker);

        string speaker;
        switch (dialogue.speaker)
        {
            case ESpeaker.CHARACTER:
                speaker = name;
                break;
            case ESpeaker.PLAYER:
                speaker = "You";
                break;
            default:
                speaker = "";
                break;
        }


        //print(Translator.instance.language.ToString());

        dialogueBox.InitializeDialogueBox(dialogue, speaker, language, Translator.instance.language);
    }



    public void EndConversation()
    {
        inDialogue = false;

        PlayerMovement.instance.SetPlayerState(previousPlayerState);
        character.OnConversationEnded();

        dialogueBox.EndDialogue();
        dialogueBox.gameObject.SetActive(false);

        Translator.instance.gameObject.SetActive(false);

        CameraController.instance.StopTargetInteractable();
    }



    public void OnTranslatorLanguageChanged(ELanguage translatedLanguage)
    {
        //make sure we're in dialogue before updating the dialogue box
        if (inDialogue)
        {
            dialogueBox.InitializeDialogueBox(translatedLanguage);
        }
    }
}
