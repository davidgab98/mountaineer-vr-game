using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class XRSocketInteractorHelmet : XRSocketInteractor {
    PlayerLifeController lifeController;

    protected override void Awake() {
        base.Awake();

        lifeController = GetComponentInParent<PlayerLifeController>();
    }

    protected override void OnHoverEntering(XRBaseInteractable interactable) {
        if(interactable.CompareTag("ClimbingHelmet")) {
            Debug.Log("Hovering hat");
            allowSelect = true;
            base.OnHoverEntering(interactable);
        } else {
            Debug.Log("Hovering another object");
            allowSelect = false;
        }
    }

    /*
    protected override void OnSelectEntering(XRBaseInteractable interactable) {
        if(interactable.CompareTag("ClimbingHelmet")) {
            allowSelect = true;
            base.OnSelectEntering(interactable);
        } else {
            allowSelect = false;
        }
       
    }*/

    protected override void OnSelectEntered(XRBaseInteractable interactable) {
        if(interactable.CompareTag("ClimbingHelmet")) {
            lifeController.UpdateMaxLife(150);
            base.OnSelectEntered(interactable);
        }
    }

    protected override void OnSelectExited(XRBaseInteractable interactable) {
        if(interactable && interactable.CompareTag("ClimbingHelmet")) {
            lifeController.UpdateMaxLife(100);
        }

        base.OnSelectExited(interactable);
    }
}
