using System.Collections;
using System.Collections.Generic;
using UnityEngine;



//contains data for a full conversation (from when you first interact to when the last dialogue ends)
[System.Serializable]
public class Conversation
{
    //used by complex characters to know what to say
    //bought me a drink? ID "drink"
    //you didn't? ID "nodrink"
    public string conversationID;

    //these two ints are used by characters who purchase ielsek from the player
    public int ielsekBought = 0;    //how many ielsek capsules the character buys once this conversation concludes
    public int ielsekBuyRate = 2400;    //how much kartet (currency) the character pays per ielsek capsule

    //the dialogues that constitute this conversation
    public Dialogue[] dialogues = new Dialogue[1];
}
