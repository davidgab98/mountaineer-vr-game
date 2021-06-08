using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class ColdZone : MonoBehaviour {
    [SerializeField]
    private ParticleSystem steam;

    [SerializeField]
    private Volume globalVolume;
    private WhiteBalance whiteBalance;

    bool activated;
    bool couldEnough;

    private void Start() {
        if(globalVolume.profile.TryGet<WhiteBalance>(out WhiteBalance wb)) {
            whiteBalance = wb;
        }
    }

    private void OnTriggerEnter(Collider other) {
        if(other.CompareTag("Player")) {
            PlayColdBreath();
            PlayColdSteamParticles();
            activated = true;
        }
    }

    private void Update() {
        if(activated) {
            if(!couldEnough) {
                whiteBalance.temperature.value -= 0.15f;
                if(whiteBalance.temperature.value <= -22)
                    couldEnough = true;
            } else {
                whiteBalance.temperature.value += 0.1f;
                if(whiteBalance.temperature.value >= -5) {
                    whiteBalance.temperature.value = -5;
                    Destroy(gameObject);
                }
            }
            
        }
    }

    void PlayColdSteamParticles() {
        steam.Play();
    }

    void PlayColdBreath() {
        FindObjectOfType<AudioManager>().PlaySerialSound("ColdBreath");
    }


}
