using System.Collections;
using System.Collections.Generic;
using UnityEngine;



//extra data for audioClips: a tag/name to call them and an in-editor volume multiplier in case they're too loud
[System.Serializable]
public class TaggedAudio
{
    public string name;
    public AudioClip clip;
    public float volumeMultiplier = 1.0f;
}