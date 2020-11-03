using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

// ESTE SCRIPTS ES UN AÑADIDO AL XRGRABINTERACTABLE QUE PERMITE QUE LOS OBJETOS QUE SE COGEN NO SE SITUEN 
// EN EL CENTRO DEL INTERACTOR (CONTROLADOR) SINO QUE SE SITE SU ATTACHPOINT EN EL PUNTO EN EL QUE HA ENTRADO
// EN CONTACTO EL CONTROLADOR CON EL OBJETO GRABBABLE

public class XROffsetGrabInteractable : XRGrabInteractable
{
    private Vector3    initialAttachLocalPos;
    private Quaternion initialAttachLocalRot;

    // Start is called before the first frame update
    void Start()
    {
        if(!attachTransform) {
            GameObject grab = new GameObject("Grav Pivot");
            grab.transform.SetParent(transform, false);
            attachTransform = grab.transform;
        }

        initialAttachLocalPos = attachTransform.localPosition;
        initialAttachLocalRot = attachTransform.localRotation;
    }

    protected override void OnSelectEnter(XRBaseInteractor interactor) {

        if(interactor is XRDirectInteractor) {
            attachTransform.position = interactor.transform.position;
            attachTransform.rotation = interactor.transform.rotation;
        } else {
            attachTransform.localPosition = initialAttachLocalPos;
            attachTransform.localRotation = initialAttachLocalRot;
        }

        base.OnSelectEnter(interactor);
    }
}
