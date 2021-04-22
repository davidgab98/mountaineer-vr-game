using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class XRSocketInteractorHelmet : XRSocketInteractor
{
    PlayerLifeController lifeController;

    protected override void Awake() {
        base.Awake();

        lifeController = GetComponentInParent<PlayerLifeController>();
    }

    protected override void OnSelectEntered(XRBaseInteractable interactable) {
        base.OnSelectEntered(interactable);

        if(interactable.CompareTag("ClimbingHelmet")) {
            lifeController.UpdateMaxLife(150);
        }
    }

    protected override void OnSelectExited(XRBaseInteractable interactable) {
        base.OnSelectExited(interactable);

        if(interactable.CompareTag("ClimbingHelmet")) {
            lifeController.UpdateMaxLife(100);
        }
    }
}
