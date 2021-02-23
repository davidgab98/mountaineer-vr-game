using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class Teleport : TeleportationProvider
{
    private CharacterController character;
    private Vector3 nextDestination;
    private bool teleporting;

    private void Start() {
        character = GetComponent<CharacterController>();
    }

    public override bool QueueTeleportRequest(TeleportRequest teleportRequest) {
        if(!teleporting) {
            nextDestination = teleportRequest.destinationPosition;
            teleporting = true;
        }

        return true;
        //return base.QueueTeleportRequest(teleportRequest);
    }

    private void FixedUpdate() {
        if(teleporting) {
            float step = 30 * Time.fixedDeltaTime;
            character.transform.position = Vector3.MoveTowards(character.transform.position, nextDestination, step);

            if(character.transform.position.Equals(nextDestination)) {
                teleporting = false;
            }
        }
    }
}
