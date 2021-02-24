using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class Teleport : TeleportationProvider {
    public enum TeleportTypes { BLINK, SHIFT, SCREENFADE };
    public TeleportTypes teleportType;
    public float shiftTeleportSpeed = 30;

    private CharacterController character;
    private Vector3 teleportDestination;
    private bool teleporting;

    protected override void Awake() {
        character = GetComponent<CharacterController>();
    }

    public override bool QueueTeleportRequest(TeleportRequest teleportRequest) {
        if(!teleporting) {
            teleportDestination = teleportRequest.destinationPosition;
            teleporting = true;
            return true;
        }

        return false;
    }

    protected override void Update() {
        if(teleporting) {
            if(teleportType == TeleportTypes.SHIFT) {
                ShiftTeleport();
                if(character.transform.position.Equals(teleportDestination)) {
                    teleporting = false;
                }
            } else if(teleportType == TeleportTypes.SCREENFADE) {

            } else { // BLINK
                character.transform.position = teleportDestination;
                teleporting = false;
            }
        }
    }

    void ShiftTeleport() {
        float step = shiftTeleportSpeed * Time.deltaTime;
        character.transform.position = Vector3.MoveTowards(character.transform.position, teleportDestination, step);
    }


}
