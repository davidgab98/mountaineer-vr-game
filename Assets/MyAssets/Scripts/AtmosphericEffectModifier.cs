using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtmosphericEffectModifier : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem snow;
    [SerializeField]
    private int snowMaxParticles;
    [SerializeField]
    private int snowRateOverTime;

    [SerializeField]
    private GameObject wind;
    [SerializeField]
    private float windMultiplier;

    private void OnTriggerEnter(Collider other) {
        if(other.CompareTag("Player")) {
            ModifyAtmosphericEffects();
        }
    }

    private void ModifyAtmosphericEffects() {
        ParticleSystem.MainModule main = snow.main;
        main.maxParticles = snowMaxParticles;
        ParticleSystem.EmissionModule emission = snow.emission;
        emission.enabled = true;
        emission.rateOverTime = snowRateOverTime;

        wind.GetComponent<CTI.CTI_SRP_CustomWind>().WindMultiplier = windMultiplier;
    }
}
