using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

/*
 * La funcion que cumple este componente es permitir la escalada (Componente Climber del VRRig) evitando que el propio componente Interactable (this) pueda
 * ser intercambiado de mano cuando ya esta siendo agarrado con un interactor. Esto por si solo no tiene demasiadas aplicaciones practicas ( al menos que haya 
 * encontrado), pero si se combina con otro LockHandClimbInteractable situado en la misma posicion, conseguiremos como resultado dar la impresion de que el objeto 3D
 * sobre el que esten estos Interactables, puede ser agarrado por ambas manos, siendo en realidad una falsa ilusión, pues lo que estaremos haciendo es agarrar con cada
 * mano un LockHandClimbInteractable distinto.
 */

public class XROneHandClimbInteractable : XRBaseInteractable {

    XRBaseInteractor selectingInteractor;

    protected override void OnSelectEntered(XRBaseInteractor interactor) {
        base.OnSelectEntered(interactor);

        if(interactor is XRDirectInteractor directInteractor) {
            selectingInteractor = directInteractor;
            if(interactor.CompareTag("LeftHand"))
                Climber.climbingLeftHand = interactor.GetComponent<XRController>();
            else if(interactor.CompareTag("RightHand"))
                Climber.climbingRightHand = interactor.GetComponent<XRController>();
        }
    }

    protected override void OnSelectExited(XRBaseInteractor interactor) {
        base.OnSelectExited(interactor);

        if(interactor is XRDirectInteractor) {
            selectingInteractor = null;
            if(Climber.climbingLeftHand && interactor.CompareTag("LeftHand")) {
                Climber.climbingLeftHand = null;
            } else if(Climber.climbingRightHand && interactor.CompareTag("RightHand")) {
                Climber.climbingRightHand = null;
            }
        }
    }

    public override bool IsSelectableBy(XRBaseInteractor interactor) {
        // De esta forma comprobamos si el objeto ya esta agarrado, y si lo está, evitamos que se pueda agarrar con un interactor diferente (pasar de un interactor a otro)
        bool isAlreadyGrabbed = selectingInteractor && !interactor.Equals(selectingInteractor);

        return base.IsSelectableBy(interactor) && !isAlreadyGrabbed;
    }
}
