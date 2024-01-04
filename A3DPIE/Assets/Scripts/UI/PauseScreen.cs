using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



//manages opening tabs on the pause screen
public class PauseScreen : MonoBehaviour
{
    public Color selectedColor, unselectedColor;
    public GameObject[] tabs;
    public TMPro.TextMeshProUGUI[] buttonTexts;



    //auto-open How To Play (the first tab)
    void OnEnable()
    {
        OpenTab(tabs[0]);
    }



    //set a tab to active and deactivate the rest
    public void OpenTab(GameObject tab)
    {
        for (int i = 0; i < tabs.Length; i++)
        {
            if (tabs[i] != tab)
            {
                buttonTexts[i].color = unselectedColor;
                tabs[i].SetActive(false);
            }

            else
            {
                tabs[i].SetActive(true);
                buttonTexts[i].color = selectedColor;
            }
        }
    }



    //quit for quit button
    public void Quit()
    {
        Application.Quit();
    }
}
