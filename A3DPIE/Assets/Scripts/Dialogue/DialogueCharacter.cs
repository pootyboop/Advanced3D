using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueCharacter : MonoBehaviour, IInteractable
{
    public string name;
    public ELanguage spokenLanguage = ELanguage.HIESCA;
    public Conversation[] conversations = new Conversation[1];
    private int conversationIndex = 0;

    public void Interact()
    {
        //start dialogue
    }
}
