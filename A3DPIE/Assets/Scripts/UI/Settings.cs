using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    public static Settings instance;

    public Slider sensitivitySlider, musicSlider, sfxSlider;
    public Toggle fullscreenButton;



    void Start()
    {
        instance = this;
    }



    private void OnEnable() {
        UpdateUI();
    }


    //set fullscreen from slider
    public void SetFullscreen(bool fullscreen)
    {
        if (fullscreen)
        {
            Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
            PlayerPrefs.SetInt("windowed", 0);
        }

        else
        {
            Screen.fullScreenMode = FullScreenMode.Windowed;
            PlayerPrefs.SetInt("windowed", 1);
        }
    }



    public void UpdateUI() {
        sensitivitySlider.value = GetCorrectMouseSensivity();

        musicSlider.value = AudioManager.instance.musicVolume;
        sfxSlider.value = AudioManager.instance.sfxVolume;
        
        fullscreenButton.isOn = (PlayerPrefs.GetInt("windowed") == 0);
    }



    public float GetCorrectMouseSensivity() {
        float sens = PlayerPrefs.GetFloat("sensitivity");

        if (sens == 0.0f) {
            sens = 300.0f;
        }

        return sens;
    }



    public void SetMusicVolume(float volume) {
        AudioManager.instance.SetMusicVolume(volume);
    }



    public void SetSFXVolume(float volume) {
        AudioManager.instance.SetSFXVolume(volume);
    }



    public void SetMouseSensitivity(float sens) {
        if (CameraController.instance != null) {
            CameraController.instance.SetMouseSensitivity(sens);
        }

        else {
            PlayerPrefs.SetFloat("sensitivity", sens);
        }
    }
}
