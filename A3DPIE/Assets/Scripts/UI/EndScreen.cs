using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



//simple actions for the buttons on the end screen (in the EndScreen scene opened after outro cutscene)
public class EndScreen : MonoBehaviour
{
    public GameObject[] disableOnPlay;
    public AudioSource music;
    public float fadeThenPlayTime = 2f;
    public float fadeToBlackSpeed;

    void Start()
    {
        //show the cursor so we can use the end screen
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        UI.instance.FadeToBlack(true);
    }



    public void PlayAgain()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        foreach (GameObject obj in disableOnPlay) {
            obj.SetActive(false);
        }

        UI.instance.fadeToBlackSpeed = fadeToBlackSpeed;
        UI.instance.StopFadeToBlack();
        StartCoroutine(FadeAndStart());
    }



    IEnumerator FadeAndStart() {

        float time = 0.0f;

        while (time < fadeThenPlayTime) {
            time += Time.deltaTime;
            float alpha = time / fadeThenPlayTime;

            UI.instance.SetFadeToBlack(alpha);
            music.volume = 1.0f - alpha;

            yield return null;
        }
        SceneManager.LoadSceneAsync("Main");
    }



    public void QuitToDesktop()
    {
        Application.Quit();
    }

    
    public void GoToItch() {
        Application.OpenURL("https://elliotgmann.itch.io/");
    }
}
