using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlesManager : MonoBehaviour {

    [SerializeField]
    public GameObject velocityParticles;

    [SerializeField]
    public VerticalMovement verticalMovement;
    [SerializeField]
    private float minSpeedForVelocityParticles = 20;

    void Update() {
        if(Mathf.Abs(verticalMovement.fallingSpeed) >= minSpeedForVelocityParticles) {
            velocityParticles.SetActive(true);
        } else {
            velocityParticles.SetActive(false);
        }
    }
}
