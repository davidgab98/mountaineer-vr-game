using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class LocomotionController : MonoBehaviour
{
    public XRController leftTeleportRay;
    public XRController rightTeleportRay;
    public InputHelpers.Button teleportActivationButton;
    public float activationThreshold = 0.1f;

    // ##NOTA:## Encapsulamos estas variables con un get; set; para poder modificarlas desde un evento en el inspector
    public bool enableLeftTeleport {get; set; } = true;
    public bool enableRightTeleport { get; set; } = true;

    public XRRayInteractor leftRayInteractor;
    public XRRayInteractor rightRayInteractor;

    void Update()
    {
        //TryGetHitInfo nos devuelve si el rayInteractor esta colisionando con algo válido o no
        if(leftTeleportRay) {
            //bool isLeftInteractorRayHovering = leftRayInteractor.TryGetHitInfo(out pos, out norm, out index, out validTarget);
            bool isLeftInteractorRayHovering = leftRayInteractor.TryGetHitInfo(out _, out _, out _, out _); //Usamos _ (valor de descarte) debido a que no nos interesa el valor de salida (asi, se lo indicamos al compilador y a un posible lector del codigo)
            leftTeleportRay.gameObject.SetActive(enableLeftTeleport && CheckIfActivated(leftTeleportRay) && !isLeftInteractorRayHovering);
        }

        if(rightTeleportRay) {
            //bool isRightInteractorRayHovering = rightRayInteractor.TryGetHitInfo(out pos, out norm, out index, out validTarget);
            bool isRightInteractorRayHovering = rightRayInteractor.TryGetHitInfo(out _, out _, out _, out _);
            rightTeleportRay.gameObject.SetActive(enableRightTeleport && CheckIfActivated(rightTeleportRay) && !isRightInteractorRayHovering);
        }
    }

    public bool CheckIfActivated(XRController controller) {
        //ATAJO PARA CHEQUEAR INPUTS.
        //InputHelpers.IsPressed:
            //-InputDevice
            //-Button to check
            //-Out bool
            //-Valor Minimo para activarse (isActivated: true)
        InputHelpers.IsPressed(controller.inputDevice, teleportActivationButton, out bool isActivated, activationThreshold);
        return isActivated;
    }
}
