using UnityEngine.Audio;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;

    public float minVolume;
    public float maxVolume;
    public float minPitch;
    public float maxPitch;

    public bool loop;

    [HideInInspector]
    public AudioSource source;
}
