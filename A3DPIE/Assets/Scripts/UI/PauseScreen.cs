using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;



//manages settings (other than audio, handled by AudioManager) and opens tabs on the pause screen
public class PauseScreen : MonoBehaviour
{
    public static PauseScreen instance;

    public Slider mouseSensitivity;

    public Color selectedColor, unselectedColor;
    public GameObject[] tabs;
    public TextMeshProUGUI[] buttonTexts;
    private bool playButtonSound = true;

    //public Slider musicSlider, sfxSlider;
    //public Toggle fullscreenToggle;


    private void Start() {
        instance = this;
    }


    //auto-open How To Play (the first tab)
    void OnEnable()
    {
        OpenTab(tabs[0]);
        //playButtonSound = true;
    }



    //set a tab to active and deactivate the rest
    public void OpenTab(GameObject tab)
    {
        if (playButtonSound) {
            AudioManager.instance.PlayAudioByTag("button");
        }

        for (int i = 0; i < tabs.Length; i++)
        {
            //hide non-selected tabs
            if (tabs[i] != tab)
            {
                buttonTexts[i].color = unselectedColor;
                // this weird operator just removes the underline fontstyle if it's currently applied
                buttonTexts[i].fontStyle &= ~FontStyles.Underline;
                tabs[i].SetActive(false);
            }

            //unhide selected tab
            else
            {
                tabs[i].SetActive(true);
                buttonTexts[i].color = selectedColor;
                buttonTexts[i].fontStyle = FontStyles.Underline;
            }
        }
    }



    //quit for quit button
    public void Quit()
    {
        Application.Quit();
    }
}
