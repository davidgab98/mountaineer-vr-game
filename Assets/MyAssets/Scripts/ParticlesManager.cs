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

    bool hasScreamed;

    void Update() {
        if(Mathf.Abs(verticalMovement.fallingSpeed) >= minSpeedForVelocityParticles) {
            if(!hasScreamed) {
                FindObjectOfType<AudioManager>().PlaySound("FallScream");
                hasScreamed = true;
            }
            FindObjectOfType<AudioManager>().PlaySound("WindFalling");
            velocityParticles.SetActive(true);
        } else {
            FindObjectOfType<AudioManager>().StopSound("WindFalling");
            hasScreamed = false;
            FindObjectOfType<AudioManager>().StopSound("FallScream");
            velocityParticles.SetActive(false);
        }
    }
}
