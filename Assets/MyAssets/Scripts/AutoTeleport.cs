using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoTeleport : MonoBehaviour
{
    [SerializeField]
    private Vector3 destinationTeleportPosition;
    [SerializeField]
    private Quaternion destinationTeleportRotation;

    private void OnTriggerEnter(Collider other) {
        if(other.CompareTag("Player")) {
            FindObjectOfType<AudioManager>().PlaySound("AutoTeleport");
            other.GetComponent<CharacterController>().transform.position = destinationTeleportPosition;
            other.GetComponent<CharacterController>().transform.rotation = destinationTeleportRotation;
        }
    }
}
