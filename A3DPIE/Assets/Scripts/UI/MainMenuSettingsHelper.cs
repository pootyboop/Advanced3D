using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuSettingsHelper : MonoBehaviour
{
    public GameObject[] group1, group2;
    bool playAudio = false;


    private void Start() {
        SetActiveGroup(false);
        playAudio = true;
    }



    public void SetActiveGroup(bool isGroup2) {
        AudioManager.instance.PlayAudioByTag("button");
        foreach (GameObject obj in group1) {
            obj.SetActive(!isGroup2);
        }

        foreach (GameObject obj in group2) {
            obj.SetActive(isGroup2);
        }
    }
}
