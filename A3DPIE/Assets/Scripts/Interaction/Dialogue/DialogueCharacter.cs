using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public int kartetCost
    {
        get
        {
            return 0;
        }
    }

    public bool targetable
    {
        get
        {
            return (conversations.Length > 0);
        }
    }

    public EInteractionType type = EInteractionType.DIALOGUE;

    public string name;
    public ELanguage spokenLanguage = ELanguage.HIESCA;
    public Conversation[] conversations = new Conversation[1];
    private int conversationIndex = 0;

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



    public void Interact()
    {
        //start dialogue
        DialogueManager.instance.StartConversation(this, conversations[conversationIndex]);
    }



    public void SetDialogueState(ESpeaker speaker)
    {
        if (isCharacter)
        {
            character.SetDialogueState(speaker);
            character.SetInDialogue(true);
        }
    }



    public void OnConversationEnded()
    {
        //increment the conversation index if this isn't the final conversation with this character
        if (conversationIndex < conversations.Length - 1)
        {
            conversationIndex++;
        }

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
