using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuSettingsHelper : MonoBehaviour
{
    public GameObject[] group1, group2;



    private void Start() {
        SetActiveGroup(false);
    }



    public void SetActiveGroup(bool isGroup2) {
        foreach (GameObject obj in group1) {
            obj.SetActive(!isGroup2);
        }

        foreach (GameObject obj in group2) {
            obj.SetActive(isGroup2);
        }
    }
}
