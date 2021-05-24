using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClimbingExhaustionController : MonoBehaviour {
    private VerticalMovement verticalMovement;

    [SerializeField]
    private float maxClimbingExhaustionTime = 65;
    [SerializeField]
    private float minTimeToHiperventilate = 15;
    [SerializeField]
    private float timeOffTheGround;

    private void Awake() {
        verticalMovement = GetComponent<VerticalMovement>();
    }

    void Update() {
        if(Climber.climbingLeftHand != null || Climber.climbingRightHand != null) {
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
