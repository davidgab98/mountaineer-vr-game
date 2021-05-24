using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class Climber : MonoBehaviour
{
    public static XRController climbingLeftHand, climbingRightHand; 
    private CharacterController character; 
    private ContinuousMovement continuousMovement; 
    private ArmSwinger armSwinger;
    private VerticalMovement verticalMovement; 

    void Start() {
        character = GetComponent<CharacterController>(); 
        continuousMovement = GetComponent<ContinuousMovement>(); 
        armSwinger = GetComponent<ArmSwinger>(); 
        verticalMovement = GetComponent<VerticalMovement>(); 
    }

    void FixedUpdate() {
        if(climbingLeftHand || climbingRightHand) {
            continuousMovement.enabled = false; 
            armSwinger.enabled = false; 
            verticalMovement.blockedFall = true; 
            Climb();
        } else {
            continuousMovement.enabled = true; 
            armSwinger.enabled = true; 
            verticalMovement.blockedFall = false; 
        }
    }

    //Climbing Computations
    void Climb() {
        Vector3 impulseVelocity = Vector3.zero;

        if(climbingLeftHand && InputDevices.GetDeviceAtXRNode(climbingLeftHand.controllerNode).TryGetFeatureValue(CommonUsages.deviceVelocity, out Vector3 leftHandVelocity)) {
            impulseVelocity += leftHandVelocity;
        }
        if(climbingRightHand && InputDevices.GetDeviceAtXRNode(climbingRightHand.controllerNode).TryGetFeatureValue(CommonUsages.deviceVelocity, out Vector3 rightHandVelocity)) {
            impulseVelocity += rightHandVelocity;
        }

        character.Move(transform.rotation * -impulseVelocity * Time.fixedDeltaTime);
    }
}
