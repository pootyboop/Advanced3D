using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;



//manages the player's ielsek (sellable resource) and kartet (currency)
public class Inventory : MonoBehaviour
{
    public static Inventory instance;
    public TMP_Text invUI;  //the UI text element representing the inventory

    public int ielsek = 30; //quantity of resource you're selling
    public int kartet = 1000;  //quantity of currency you'll gain by selling ielsek and use to buy things

    public CanvasGroupFader invUpdateUI;    //displays changes to inventory quantities
    private TMPro.TextMeshProUGUI invUpdateText;   //actual text to update

    //run this dialogue when the player has sold all of their ielsek to prompt them to leave
    public DialogueCharacter soldAllIelsekDialogue;
    private bool soldAllIelsek = false;



    void Start()
    {
        instance = this;

        invUpdateText = invUpdateUI.GetComponent<TMPro.TextMeshProUGUI>();

        UpdateInvUI();  //update the UI to match whatever the player's starting ielsek and kartet are
    }



    //removes an amount of ielsek from the inventory and pays the player the character's buying rate for each ielsek sold
    public void SellIelsek(int amountSold, int ratePerIelsek)
    {
        //kartet payout
        int totalPay = amountSold * ratePerIelsek;

        //remove the player's ielsek
        ielsek -= amountSold;
        //pay the player for each ielsek capsule sold at the rate agreed in dialogue
        kartet += totalPay;

        //update the UI to match the new quantities
        UpdateInvUI();
        InvUIChange(-1 * amountSold, totalPay);

        if (amountSold > 0) {
        AudioManager.instance.PlayAudioByTag("sell");
        }

        //if sold all ielsek, prompt player to return to ship and end game
        if (ielsek == 0 && !soldAllIelsek)
        {
            soldAllIelsek = true;   //make sure this only runs once
            soldAllIelsekDialogue.Interact();
        }
    }



    //deduct currency from the player
    //used for purchasing things
    public void PayKartet(int amount)
    {
        kartet -= amount;

        //update the UI to match
        UpdateInvUI();
        InvUIChange(0, -1 * amount);

        if (amount > 0)
            AudioManager.instance.PlayAudioByTag("buy");
    }



    //update the inventory UI to match the current inventory quantities
    void UpdateInvUI()
    {
        invUI.text =
            "<color=#B7F126>" + //ielsek color (lime green)
            ielsek +
            "\r\n<color=#FFDD00>" +  //kartet color (yellow/gold)
            kartet +
            " <s>KT</s>";
    }



    //sets text to represent a change in resource quantities
    void InvUIChange(int ielsekChange, int kartetChange)
    {
        //ielsek change
        string ielsekString =
            "<color=#D9FF00>" + //ielsek color
            GetSignFromInt(ielsekChange);   //add a + if positive change

        //display amount if not 0
        if (ielsekChange != 0)
        {
            ielsekString += ielsekChange;
        }

        //kartet change
        string kartetString = "\n" +    //line break so kartet is always lower down
            "<color=#FFDD00>" + //kartet color
            GetSignFromInt(kartetChange);   //add a + if positive change

        //display amount if not 0
        if (kartetChange != 0)
        {
            kartetString += kartetChange;
        }

        //update the UI with final string
        invUpdateText.text = ielsekString + kartetString;

        //pop in UI and fade it out
        invUpdateUI.SetAlpha(1.0f);
        invUpdateUI.FadeAlpha(false);
    }



    //returns a + if positive
    string GetSignFromInt(int val)
    {
        if (val > 0)
        {
            return "+";
        }

        //else if (val < 0)
        //{
        //    return "-";
        //}

        //return nothing for 0 as i dont want to display no change
        return "";
    }



    //set the visibility of the inventory UI
    public void SetVisibility(bool visible)
    {
        invUI.gameObject.SetActive(visible);
    }
}