using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaMusicManager : MonoBehaviour
{
    public static AreaMusicManager instance;

    private AudioSource audioSource, currAreaMusic;

    public float areaMusicFadeInTime = 8.0f;
    public float areaMusicFadeOutTime = 5.0f;
    public float areaMusicMaxVolume = 0.6f;

    private IEnumerator musicCoroutine;



    void Start() {
        instance = this;
        audioSource = GetComponent<AudioSource>();
    }



    public void SetAreaMusicPlaying(AudioSource areaMusic, bool newPlaying) {

        if (areaMusic == currAreaMusic && newPlaying) {
            return;
        }

        if (musicCoroutine != null) {
            StopCoroutine(musicCoroutine);
        }

        musicCoroutine = FadeMusic(areaMusic, newPlaying);
        StartCoroutine(musicCoroutine);
    }



    IEnumerator FadeMusic(AudioSource newAreaMusic, bool fadeIn) {
        
        if (newAreaMusic.isPlaying) {

            float time = 0.0f;
            while (time < areaMusicFadeOutTime)
            {
                    time += Time.deltaTime;
                    currAreaMusic.volume = (1.0f - time / areaMusicFadeOutTime) * areaMusicMaxVolume;
                    yield return null;
            }

            currAreaMusic.Stop();
            //audioSource.clip = null;
        }

        currAreaMusic = newAreaMusic;

        if (fadeIn && newAreaMusic != null) {
            //audioSource.clip = music;
            currAreaMusic.Play();

            float time = 0.0f;
            while (time < areaMusicFadeInTime)
            {
                    time += Time.deltaTime;
                    currAreaMusic.volume = time / areaMusicFadeInTime * areaMusicMaxVolume;
                    yield return null;
            }
        }
    }


}
