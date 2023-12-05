using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance;

    public DialogueBox dialogueBox;

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
        character = trigger;
        name = character.name;
        language = character.spokenLanguage;
        conversation = currentConversation;

        previousPlayerState = PlayerMovement.instance.state;
        PlayerMovement.instance.SetPlayerState(EPlayerState.DIALOGUE);
        dialogueIndex = 0;

        dialogueBox.gameObject.SetActive(true);

        print("Started dialogue with " + name + ", who speaks " + language.ToString() + ".");

        NextDialogue(false);
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

        string speaker = "";
        switch (dialogue.speaker)
        {
            case ESpeaker.CHARACTER:
                speaker = name;
                break;
            case ESpeaker.PLAYER:
                speaker = "Me";
                break;
        }

        dialogueBox.InitializeDialogueBox(dialogue, speaker, language);



        //Phrase[] phrases = dialogue.phrases;
        //print(speaker + " says:");

        //for (int i = 0; i < phrases.Length; i++)
        //{
        //    print(phrases[i].phrase + " (" + phrases[i].translation + ")");
        //}
    }



    public void EndConversation()
    {
        PlayerMovement.instance.SetPlayerState(previousPlayerState);
        character.OnConversationEnded();

        print("Ended dialogue with " + name);

        dialogueBox.EndDialogue();
        dialogueBox.gameObject.SetActive(false);

        CameraController.instance.StopTargetInteractable();
    }
}
