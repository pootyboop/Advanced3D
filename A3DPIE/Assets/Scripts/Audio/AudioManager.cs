using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;



//manages volume settings and plays one shot UI sounds
public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public float musicVolume = 1.0f;
    public float sfxVolume = 1.0f;

    public AudioMixer audioMixer;

    private AudioSource audioSource;    //audioSource used to play one shot sounds

    //audioClips tagged with a string name so they're easy to call from other scripts
    //mess with these in the inspector
    public TaggedAudio[] taggedAudios;




    void Start()
    {
        instance = this;
        audioSource = GetComponent<AudioSource>();

        SetMusicVolume(PlayerPrefs.GetFloat("musicVolume"));
        SetSFXVolume(PlayerPrefs.GetFloat("sfxVolume"));
    }



    //takes a pre-assigned audio clip name and plays a one shot sound
    public void PlayAudioByTag(string tag)
    {
        foreach (TaggedAudio taggedAudio in taggedAudios)
        {
            if (taggedAudio.name == tag)
            {
                PlayAudioByClip(taggedAudio.clip);
            }
        }
    }



    //plays a one shot sound
    public void PlayAudioByClip(AudioClip clip)
    {
        //audioSource.Stop();
        //audioSource.clip = clip;
        //audioSource.volume = sfxVolume;
        //audioSource.Play();

        audioSource.PlayOneShot(clip);
    }



    //set music volume from settings slider
    public void SetMusicVolume(float volume)
    {
        musicVolume = volume;

        if (volume == 0.0f) {
            musicVolume = 1.0f;
        }

        PlayerPrefs.SetFloat("musicVolume", musicVolume);
        audioMixer.SetFloat("musicVol", Mathf.Log10(musicVolume) * 20f);
    }



    //set sfx volume from settings slider
    public void SetSFXVolume(float volume)
    {
        sfxVolume = volume;

        if (volume == 0.0f) {
            sfxVolume = 1.0f;
        }

        PlayerPrefs.SetFloat("sfxVolume", sfxVolume);
        audioMixer.SetFloat("sfxVol", Mathf.Log10(sfxVolume) * 20f);
    }
}
