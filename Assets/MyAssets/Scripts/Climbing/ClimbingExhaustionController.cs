using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class ClimbingExhaustionController : MonoBehaviour {

    [SerializeField]
    private float maxClimbingExhaustionTime = 65; 
    public float minTimeToHiperventilate = 15; 

    [SerializeField]
    private float timeOffTheGround;
    private VerticalMovement verticalMovement;

    [SerializeField]
    private Volume vignetteVolume;
    private Vignette vignetteEffect;

    private void Awake() {
        verticalMovement = GetComponent<VerticalMovement>();
    }

    private void Start() {
        if(vignetteVolume.profile.TryGet<Vignette>(out Vignette cc)) {
            vignetteEffect = cc;
        }
    }

    void Update() {
        if(Climber.climbingLeftHand != null || Climber.climbingRightHand != null) {
            timeOffTheGround += Time.deltaTime;
            if(timeOffTheGround >= minTimeToHiperventilate) {
                FindObjectOfType<AudioManager>().PlaySound("Hiperventilate");
                vignetteEffect.intensity.value += 0.01f * Time.deltaTime;
            }
            if(timeOffTheGround >= maxClimbingExhaustionTime) {
                Climber.climbingLeftHand = null;
                Climber.climbingRightHand = null;
            }
        } else {
            if(verticalMovement.isGrounded) {
                timeOffTheGround = 0;
                if(vignetteEffect.intensity.value > 0) {
                    vignetteEffect.intensity.value -= 0.4f * Time.deltaTime;
                }
            }
            FindObjectOfType<AudioManager>().StopSound("Hiperventilate");
        }
    }

    /*
        if(!verticalMovement.isGrounded) {
            timeOffTheGround += Time.deltaTime;
            if(timeOffTheGround >= minTimeToHiperventilate) {
                FindObjectOfType<AudioManager>().PlaySound("Hiperventilate");
            }
            if(timeOffTheGround >= maxClimbingExhaustionTime) {
                Climber.climbingLeftHand = null;
                Climber.climbingRightHand = null;
            }
        } else {
            timeOffTheGround = 0;
            FindObjectOfType<AudioManager>().StopSound("Hiperventilate");
        }
    }*/
}
