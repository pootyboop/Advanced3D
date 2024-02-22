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

    //public Slider musicSlider, sfxSlider;
    //public Toggle fullscreenToggle;


    private void Start() {
        instance = this;
    }


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

                switch (tabs[i].name) {
                    case "Settings":
                        AudioManager.instance.UpdateSliders();
                        mouseSensitivity.value = CameraController.instance.GetCorrectMouseSensivity();
                        break;
                }
            }
        }
    }


    //set fullscreen from slider
    public void SetFullscreen(bool fullscreen)
    {
        if (fullscreen)
        {
            Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
        }

        else
        {
            Screen.fullScreenMode = FullScreenMode.Windowed;
        }
    }



    //quit for quit button
    public void Quit()
    {
        Application.Quit();
    }
}
