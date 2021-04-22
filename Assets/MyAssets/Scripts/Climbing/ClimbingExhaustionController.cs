using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClimbingExhaustionController : MonoBehaviour {
    private VerticalMovement verticalMovement;

    [SerializeField]
    private float maxClimbingExhaustionTime = 10;
    [SerializeField]
    private float timeOffTheGround;

    private void Awake() {
        verticalMovement = GetComponent<VerticalMovement>();
    }

    // Update is called once per frame
    void Update() {
        if(!verticalMovement.isGrounded) {
            timeOffTheGround += Time.deltaTime;
            if(timeOffTheGround >= maxClimbingExhaustionTime) {
                Climber.climbingLeftHand = null;
                Climber.climbingRightHand = null;
            }
        } else {
            timeOffTheGround = 0;
        }
    }
}
