using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class XRBrokenClimbInteractable : XRBaseInteractable {

    public float lifeDuration = 5;
    float timeGrabbed = 0;
    bool hasBeenGrabbed;

    XRDirectInteractor selectingInteractor;

    protected override void OnSelectEntered(XRBaseInteractor interactor) {
        base.OnSelectEntered(interactor);

        if(interactor is XRDirectInteractor directInteractor) {
            hasBeenGrabbed = true;
            selectingInteractor = directInteractor;
            if(interactor.CompareTag("LeftHand"))
                Climber.climbingLeftHand = interactor.GetComponent<XRController>();
            else if(interactor.CompareTag("RightHand"))
                Climber.climbingRightHand = interactor.GetComponent<XRController>();
            
        }
    }

    private void Update() {
        if(hasBeenGrabbed) {
            timeGrabbed += Time.deltaTime;
            if(timeGrabbed >= lifeDuration) {
                Break();
            }
        }
    }

    void Break() {
        if(selectingInteractor) {
            DeselectClimbingInteractor(selectingInteractor);
        }

        //Dejamos que el objeto caiga
        transform.parent = null; //Eliminamos al padre para evitar deformaciones cuando se vea afectado por la gravedad
        GetComponent<Rigidbody>().isKinematic = false;

        Destroy(this); //Destruimos este componente
    }


    protected override void OnSelectExited(XRBaseInteractor interactor) {
        base.OnSelectExited(interactor);

        if(interactor is XRDirectInteractor directInteractor) {
            DeselectClimbingInteractor(directInteractor);
        }
    }

    void DeselectClimbingInteractor(XRDirectInteractor interactor) {
        if(Climber.climbingLeftHand && interactor.CompareTag("LeftHand"))
            Climber.climbingLeftHand = null;
        else if(Climber.climbingRightHand && interactor.CompareTag("RightHand"))
            Climber.climbingRightHand = null;

        selectingInteractor = null;
    }



}
