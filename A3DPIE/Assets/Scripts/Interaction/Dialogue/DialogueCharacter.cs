using System.Collections;
using System.Collections.Generic;
using UnityEngine;



//a character, readable text, etc. that the player can engage in dialogue with
//this is where conversation data is ultimately stored
//this script can also be independent of any visible, in-game character/readable and Interact() can be called from other scripts...
//...which allows for tutorials and conversations even when the player didn't intentionally initiate them
public class DialogueCharacter : MonoBehaviour, IInteractable
{

    public string interactionName
    {
        get
        {
            return name;
        }
    }

    public EInteractionType interactionType
    {
        get
        {
            return type;
        }
    }

    //no cost to talk to someone or read something
    public int kartetCost
    {
        get
        {
            return 0;
        }
    }

    //can only be targeted if there's a conversation to be had
    public bool targetable
    {
        get
        {
            return (conversations.Length > 0);
        }
    }

    public EInteractionType type = EInteractionType.DIALOGUE;   //interaction type. can be set to READABLE for written text

    public string name;
    public ELanguage spokenLanguage = ELanguage.HIESCA; //native language that phrases will default to using
    public Conversation[] conversations = new Conversation[1];  //conversations this character/readable holds
    private int conversationIndex = 0;  //which conversation the player is on

    //these are used for updating character animations if necessary
    private Character character;
    private bool isCharacter = false;



    void Start()
    {
        character = GetComponent<Character>();
        if (character != null)
        {
            isCharacter = true;
        }
    }



    //most characters will look at the player when targeted for dialogue
    public void OnTargetedChanged(bool isTargeting)
    {
        if (isCharacter)
        {
            if (character.looksAtPlayerBeforeInteracting)
            {
                character.LookAtPlayer(isTargeting);
            }
        }
    }



    //when interacted with, start a conversation
    public void Interact()
    {
        DialogueManager.instance.StartConversation(this, conversations[conversationIndex]);
    }



    //set the character's animation state based on who's speaking
    public void SetDialogueState(ESpeaker speaker)
    {
        if (isCharacter)
        {
            character.SetDialogueState(speaker);
            character.SetInDialogue(true);
        }
    }



    //finish the conversation
    public void OnConversationEnded()
    {
        //increment the conversation index if this isn't the final conversation with this character
        //otherwise, leave it alone. the player will always be able to repeat the final conversation they had.
        //this allows for a quip from the character after the main conversation is over to indicate they don't have anything else interesting to say
        if (conversationIndex < conversations.Length - 1)
        {
            conversationIndex++;
        }

        //stop character dialogue animations
        if (isCharacter)
        {
            character.SetInDialogue(false);

            if (!character.looksAtPlayerBeforeInteracting)
            {
                character.LookAtPlayer(false);
            }
        }
    }
}
