using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public float musicVolume = 1.0f;
    public float sfxVolume = 1.0f;

    private AudioSource audioSource;
    public TaggedAudio[] taggedAudios;




    void Start()
    {
        instance = this;
        audioSource = GetComponent<AudioSource>();
    }



    public void PlayAudioByTag(string tag)
    {
        for (int i = 0; i < taggedAudios.Length; i++)
        {
            if (taggedAudios[i].name == tag)
            {
                PlayAudioByClip(taggedAudios[i].clip);
            }
        }
    }



    public void PlayAudioByClip(AudioClip clip)
    {
        //audioSource.Stop();
        //audioSource.clip = clip;
        //audioSource.volume = sfxVolume;
        //audioSource.Play();

        audioSource.PlayOneShot(clip, sfxVolume);
    }
}
