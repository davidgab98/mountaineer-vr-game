using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoTeleportGrab : MonoBehaviour {
    [SerializeField]
    private Vector3 destinationTeleportPosition;

    public void StartTeleportPlayer(CharacterController character) {
        FindObjectOfType<VerticalMovement>().enabled = false;
        Climber.climbingLeftHand = null;
        Climber.climbingRightHand = null;
        character.transform.position = destinationTeleportPosition;
    }

    public void EndTeleportPlayer() {
        FindObjectOfType<VerticalMovement>().enabled = true;
    }
}
