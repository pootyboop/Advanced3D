using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Conversation
{
    public string conversationID;   //used by complex characters to know what to say
                                    //bought me a drink? "drink"
                                    //you didn't? "nodrink"
    public Dialogue[] dialogues = new Dialogue[1];
}
