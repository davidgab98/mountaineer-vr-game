using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

// ESTE SCRIPTS ES UN AÑADIDO AL XRGRABINTERACTABLE QUE PERMITE QUE LOS OBJETOS QUE SE COGEN NO SE SITUEN  
// EN EL CENTRO DEL INTERACTOR (CONTROLADOR) SINO QUE SE SITE SU ATTACHPOINT EN EL PUNTO EN EL QUE HA ENTRADO  
// EN CONTACTO EL CONTROLADOR CON EL OBJETO GRABBABLE: HACEMOS ESTO IGUALANDO LA POSICION DEL INTERACTOR A LA DEL INTERACTABLE (asi resumidamente)

public class XROffsetGrabInteractable : XRGrabInteractable {
    private Vector3 initialInteractorPosition = Vector3.zero;
    private Quaternion initialInteractorRotation = Quaternion.identity;

    protected override void OnSelectEntered(XRBaseInteractor interactor) {
        base.OnSelectEntered(interactor);
        if(interactor is XRDirectInteractor) {
            SaveInitialInteractorLocation(interactor);
            UpdateInteractorTransform(interactor);
        }
    }

    void SaveInitialInteractorLocation(XRBaseInteractor interactor) {
        initialInteractorPosition = interactor.attachTransform.localPosition;
        initialInteractorRotation = interactor.attachTransform.localRotation;
    }

    void UpdateInteractorTransform(XRBaseInteractor interactor) {
        if(attachTransform != null) {
            interactor.attachTransform.position = attachTransform.position;
            interactor.attachTransform.rotation = attachTransform.rotation;
        } else {
            interactor.attachTransform.position = transform.position;
            interactor.attachTransform.rotation = transform.rotation;
        }

        /* ANDREW LO HACE ASI DE GUAPO
        bool hasAttach = attachTransform != null;
        interactor.attachTransform.position = hasAttach ? attachTransform.position : transform.position;
        interactor.attachTransform.rotation = hasAttach ? attachTransform.rotation : transform.rotation;
        */
    }

    protected override void OnSelectExited(XRBaseInteractor interactor) {
        base.OnSelectExited(interactor);
        if(interactor is XRDirectInteractor) {
            ResetInteractorLocation(interactor);
        }
    }

    void ResetInteractorLocation(XRBaseInteractor interactor) {
        interactor.attachTransform.localPosition = initialInteractorPosition;
        interactor.attachTransform.localRotation = initialInteractorRotation;

        initialInteractorPosition = Vector3.zero;
        initialInteractorRotation = Quaternion.identity;
    }
}
