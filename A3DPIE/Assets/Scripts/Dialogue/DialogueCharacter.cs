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



    public void Interact()
    {
        //start dialogue
        DialogueManager.instance.StartConversation(this, conversations[conversationIndex]);
    }



    public void OnConversationEnded()
    {
        //increment the conversation index if this isn't the final conversation with this character
        if (conversationIndex < conversations.Length - 1)
        {
            conversationIndex++;
        }
    }
}
