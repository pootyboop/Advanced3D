using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;



//manages the flow of dialogue
//gets data between the player, the DialogueCharacter, the UI, and the translator
public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance;

    public DialogueBox dialogueBox; //dialogue UI box

    private bool inDialogue = false;

    DialogueCharacter character;    //the character/readable/etc the player is in dialogue with

    //data from the DialogueCharacter, just made more easily accessible
    //this is not cleaned out between conversations, just overwritten
    string name;
    ELanguage language;
    Conversation conversation;
    int dialogueIndex;

    EPlayerState previousPlayerState;   //state to return the player to once finished with dialogue



    void Start()
    {
        instance = this;
    }



    //initiate a conversation
    //called from DialogueCharacter when interacted with
    public void StartConversation(DialogueCharacter trigger, Conversation currentConversation)
    {
        inDialogue = true;

        //save the conversation info globally for easy access
        character = trigger;
        name = character.name;
        language = character.spokenLanguage;
        conversation = currentConversation;

        //the player does NOT enter the DIALOGUE state when in a cutscene.
        //this means dialogue will just appear on screen and the player does not have control over it
        if (PlayerMovement.instance.state != EPlayerState.CUTSCENE)
        {
            previousPlayerState = PlayerMovement.instance.state;
            PlayerMovement.instance.SetPlayerState(EPlayerState.DIALOGUE);
        }

        //start at the first dialogue in the conversation
        dialogueIndex = 0;

        //start UI and the translator
        dialogueBox.gameObject.SetActive(true);
        Translator.instance.gameObject.SetActive(true);

        //initiate the first dialogue
        NextDialogue(false);

        //default to Hiesca, the common language, for every new conversation
        Translator.instance.SetTranslatorLanguage(ELanguage.HIESCA);

        //play a sound for entering a conversation
        AudioManager.instance.PlayAudioByTag("dialogue");
    }



    //progress to the next dialogue in the array, or just display the first one if we just started the conversation
    public void NextDialogue(bool showNextDialogue)
    {
        //don't increment the index if just starting
        if (showNextDialogue)
        {
            dialogueIndex++;
        }

        //end the conversation when the player attempts to progress the conversation when no dialogues are left
        if (dialogueIndex >= conversation.dialogues.Length)
        {
            EndConversation();
            return;
        }

        //get the current dialogue
        Dialogue dialogue = conversation.dialogues[dialogueIndex];

        //update the dialogue state (only important if speaking to a character and not a readable or etc)
        character.SetDialogueState(dialogue.speaker);

        //convert the speaker enum to a string to display as the speaker's name
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
                speaker = "";   //used later on to indicate that the player is thinking to themself
                break;
        }

        //finally, set up the UI
        dialogueBox.InitializeDialogueBox(dialogue, speaker, language, Translator.instance.language);
    }



    //called when the conversation ends
    public void EndConversation()
    {
        inDialogue = false;

        //if not in a cutscene, put the player back in whatever state they were in before entering the conversation
        if (PlayerMovement.instance.state != EPlayerState.CUTSCENE)
        {
            PlayerMovement.instance.SetPlayerState(previousPlayerState);
            CameraController.instance.StopTargetInteractable();
        }

        //revert the character's animations to their default activity (if speaking to a character and not a readable or etc)
        character.OnConversationEnded();

        //cleanup dialogue box UI
        dialogueBox.EndDialogue();
        dialogueBox.gameObject.SetActive(false);

        //turn off translator
        Translator.instance.gameObject.SetActive(false);

        //finally, make any ielsek sales the player and character agreed to
        Inventory.instance.SellIelsek(conversation.ielsekBought, conversation.ielsekBuyRate);
    }



    //changes the translated language in the dialogue box
    public void OnTranslatorLanguageChanged(ELanguage translatedLanguage)
    {
        //make sure we're in dialogue before updating the dialogue box
        if (inDialogue)
        {
            //reinitialize the dialogue box UI with the new translated language
            dialogueBox.InitializeDialogueBox(translatedLanguage);
        }
    }
}
