using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndExperienceTrigger : MonoBehaviour
{
    [SerializeField]
    private List<TerrainCollider> componentsToUnable = new List<TerrainCollider>();

    [SerializeField]
    private ImageFader fader;

    private void OnTriggerEnter(Collider other) {
        if(other.CompareTag("Player")){
            DisableComponents();
            fader.FadeOut(8);
        }
    }

    private void DisableComponents() {
        for(int i = 0; i < componentsToUnable.Count; i++) {
            componentsToUnable[i].enabled = false;
        }
    }
}
