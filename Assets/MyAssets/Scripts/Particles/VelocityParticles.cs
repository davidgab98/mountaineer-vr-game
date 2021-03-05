using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VelocityParticles : MonoBehaviour
{
    private ParticleSystem.MainModule velocityParticlesMainModule;
    private float startSpeedOverTimeFactor = 5f;

    private void Awake() {
        velocityParticlesMainModule = GetComponent<ParticleSystem>().main;
    }

    private void Update() {
        float newStartSpeed = velocityParticlesMainModule.startSpeed.constant + (startSpeedOverTimeFactor * Time.deltaTime);
        velocityParticlesMainModule.startSpeed = newStartSpeed;
    }
}
