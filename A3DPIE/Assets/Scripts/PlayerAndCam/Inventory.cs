using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;



//manages the player's ielsek (sellable resource) and kartet (currency)
public class Inventory : MonoBehaviour
{
    public static Inventory instance;
    public TMP_Text invUI;  //the UI text element representing the inventory

    public int ielsek = 30; //resource you're selling
    public int kartet = 1000;  //currency you'll gain by selling ielsek



    void Start()
    {
        instance = this;
        UpdateInvUI();  //update the UI to match whatever the player's starting ielsek and kartet are
    }



    //removes an amount of ielsek from the inventory and pays the player the character's buying rate for each ielsek sold
    public void SellIelsek(int amountSold, int ratePerIelsek)
    {
        //remove the player's ielsek
        ielsek -= amountSold;
        //pay the player for each ielsek capsule sold at the rate agreed in dialogue
        kartet += amountSold * ratePerIelsek;

        //update the UI to match the new quantities
        UpdateInvUI();
    }



    //deduct currency from the player
    //used for purchasing things
    public void PayKartet(int amount)
    {
        kartet -= amount;
        //update the UI to match
        UpdateInvUI();
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



    //set the visibility of the inventory UI
    public void SetVisibility(bool visible)
    {
        invUI.gameObject.SetActive(visible);
    }
}