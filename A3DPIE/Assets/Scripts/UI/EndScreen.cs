using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



//simple actions for the buttons on the end screen (in the EndScreen scene opened after outro cutscene)
public class EndScreen : MonoBehaviour
{
    void Start()
    {
        //show the cursor so we can use the end screen
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }



    public void PlayAgain()
    {
        SceneManager.LoadScene("Main");
    }



    public void QuitToDesktop()
    {
        Application.Quit();
    }
}
