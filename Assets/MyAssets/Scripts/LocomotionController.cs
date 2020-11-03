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

    void Update()
    {
        if(leftTeleportRay) {
            leftTeleportRay.gameObject.SetActive(enableLeftTeleport && CheckIfActivated(leftTeleportRay));
        }

        if(rightTeleportRay) {
            rightTeleportRay.gameObject.SetActive(enableRightTeleport && CheckIfActivated(rightTeleportRay));
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
