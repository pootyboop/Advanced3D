using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



//functionality for opening the end screen from the outro cutscene
public class EndScreenOpener : MonoBehaviour
{
    public void OpenEndScreen()
    {
        //show the cursor so we can use the end screen
        Cursor.lockState = CursorLockMode.None;
        SceneManager.LoadScene("EndScreen");
    }
}
