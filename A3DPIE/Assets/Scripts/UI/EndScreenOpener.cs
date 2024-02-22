using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



//functionality for opening the end screen from the outro cutscene
public class EndScreenOpener : MonoBehaviour
{
    public void OpenEndScreen()
    {
        if (PlayerMovement.instance.state == EPlayerState.PAUSED) {
            UI.instance.SetPaused(false);
        }

        SceneManager.LoadScene("MainMenu");
    }
}
