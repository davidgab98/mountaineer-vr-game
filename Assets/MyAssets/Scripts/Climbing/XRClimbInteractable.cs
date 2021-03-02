using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class XRClimbInteractable : XRBaseInteractable
{
    protected override void OnSelectEntered(XRBaseInteractor interactor) {
        base.OnSelectEntered(interactor);

        if(interactor is XRDirectInteractor) {
            if(interactor.CompareTag("LeftHand")) 
                Climber.climbingLeftHand = interactor.GetComponent<XRController>();
            else if(interactor.CompareTag("RightHand"))
                Climber.climbingRightHand = interactor.GetComponent<XRController>();
        }
    }

    protected override void OnSelectExited(XRBaseInteractor interactor) {
        base.OnSelectExited(interactor);

        if(interactor is XRDirectInteractor) {
            if(interactor.CompareTag("LeftHand") && Climber.climbingLeftHand) {
                Climber.climbingLeftHand = null;
            } else if(interactor.CompareTag("RightHand") && Climber.climbingRightHand) {
                Climber.climbingRightHand = null;
            }
        }
    }

}
