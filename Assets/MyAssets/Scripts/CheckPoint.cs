using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    private bool activated;

    private void OnTriggerEnter(Collider other) {
        if(other.CompareTag("Player") && !activated) {
            PlayerLifeController.lastCheckPointPosition = transform.position;
            activated = true;
        }
    }
}
