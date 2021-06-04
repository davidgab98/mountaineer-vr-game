using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    [SerializeField]
    private SkinnedMeshRenderer flagMesh;
    [SerializeField]
    private Material flagActivatedMaterial;

    private bool activated;

    private void OnTriggerEnter(Collider other) {
        if(other.CompareTag("Player") && !activated) {
            ActivateCheckPointParticles();
            StartCoroutine(ChangeFlagColor());

            FindObjectOfType<AudioManager>().PlaySound("CheckPoint");

            PlayerLifeController.lastCheckPointPosition = transform.position;
            activated = true;
        }
    }

    private void ActivateCheckPointParticles() {
        // set particles in the FlagPrefab position (child(0)) and Play that particles
        FindObjectOfType<ParticlesManager>().SetAndPlayCheckPointParticles(transform.GetChild(0).position);
    }

    IEnumerator ChangeFlagColor() {
        yield return new WaitForSeconds(1.5f);
        flagMesh.material = flagActivatedMaterial;
    }
}
