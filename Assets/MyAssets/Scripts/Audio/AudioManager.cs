using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;

    private void Awake() {
        foreach(Sound s in sounds) {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.minVolume;
            s.source.pitch = s.minPitch;
            s.source.loop = s.loop;
            s.source.playOnAwake = false;
        }
    }

    public void PlayVariableSound(string name) {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        Debug.Log(s == null);
        if(s != null && !s.source.isPlaying) {
            s.source.volume = UnityEngine.Random.Range(s.minVolume, s.maxVolume);
            s.source.pitch = UnityEngine.Random.Range(s.minPitch, s.maxPitch);
            s.source.Play();
        } else {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
    }

    public void PlaySound(string name) {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if(s != null && !s.source.isPlaying){
            s.source.Play();
        } else {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
    }

    public void StopSound(string name) {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if(s != null && s.source.isPlaying) {
            s.source.Stop();
        } 
    }
}
