using System.Collections;
using System.Collections.Generic;
using UnityEngine;



//manages volume settings and plays one shot UI sounds
public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public float musicVolume = 1.0f;
    public float sfxVolume = 1.0f;

    public AudioSource[] musics;

    private AudioSource audioSource;    //audioSource used to play one shot sounds

    //audioClips tagged with a string name so they're easy to call from other scripts
    //mess with these in the inspector
    public TaggedAudio[] taggedAudios;




    void Start()
    {
        instance = this;
        audioSource = GetComponent<AudioSource>();
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

        audioSource.PlayOneShot(clip, sfxVolume);
    }



    //set music volume from settings slider
    public void SetMusicVolume(float volume)
    {
        musicVolume = volume;

        //update all musics' volume
        foreach (AudioSource music in musics)
        {
            music.volume = musicVolume;
        }
    }



    //set sfx volume from settings slider
    public void SetSFXVolume(float volume)
    {
        sfxVolume = volume;
    }
}
