using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Inventory : MonoBehaviour
{
    public static Inventory instance;
    public TMP_Text invUI;

    public int ielsek = 30; //resource you're selling
    public int kartet = 1000;  //currency you'll gain by selling ielsek



    void Start()
    {
        instance = this;
        UpdateInvUI();
    }



    public void SellIelsek(int amountSold, int ratePerIelsek)
    {
        ielsek -= amountSold;
        kartet += amountSold * ratePerIelsek;

        UpdateInvUI();
    }



    public void PayKartet(int amount)
    {

    }



    void UpdateInvUI()
    {
        invUI.text =
            "<color=#B7F126>" +
            ielsek +
            " Ielsek\r\n<color=#FFDD00>" +
            kartet +
            " <s>KT</s>";
    }
}