using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip audioClip;

    // En el tuturial de Valem hay mas cosas ricas en este script, como lanzar una bala
    public void Fire() {
        audioSource.PlayOneShot(audioSource.clip);
    }
}
