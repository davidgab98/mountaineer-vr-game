using UnityEngine.Audio;
using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class Sound
{
    public string name;
    public List<AudioClip> clips;
    [HideInInspector]
    public int currentClip;

    public float minVolume;
    public float maxVolume;
    public float minPitch;
    public float maxPitch;

    public bool loop;


    [HideInInspector]
    public AudioSource source;
}
