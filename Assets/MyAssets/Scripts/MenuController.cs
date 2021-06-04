using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class MenuController : MonoBehaviour
{
    public Canvas menu;
    public GameObject openingMenuImages;

    [SerializeField]
    private XRController leftController;
    private InputDevice leftControllerDevice;

    private float pressingActivateMenuButtonDuration = 0.75f;
    private float pressingActivateMenuButtonTime = 0;

    [SerializeField]
    private GameObject leftTeleportRay, rightTeleportRay;

    private bool buttonToOpenMenuUp;

    private void Start() {
        TryInitializeDevice(ref leftControllerDevice, leftController);
    }

    
    void Update() {
        if(!leftControllerDevice.isValid) {
            TryInitializeDevice(ref leftControllerDevice, leftController);
        } else {
            if(leftControllerDevice.TryGetFeatureValue(CommonUsages.secondaryButton, out bool secondaryButton) && secondaryButton) {
                if(buttonToOpenMenuUp) {
                    openingMenuImages.SetActive(true);
                    openingMenuImages.transform.GetChild(0).GetComponent<Image>().fillAmount += 1.5f * Time.deltaTime;

                    pressingActivateMenuButtonTime += Time.deltaTime;
                    if(pressingActivateMenuButtonTime >= pressingActivateMenuButtonDuration) {
                        openingMenuImages.SetActive(false);
                        openingMenuImages.transform.GetChild(0).GetComponent<Image>().fillAmount = 0;
                        OpenMenu();
                        pressingActivateMenuButtonTime = 0;
                        buttonToOpenMenuUp = false;
                    }
                }
            } else {
                openingMenuImages.transform.GetChild(0).GetComponent<Image>().fillAmount = 0;
                openingMenuImages.SetActive(false);
                pressingActivateMenuButtonTime = 0;
                buttonToOpenMenuUp = true;
            }
        }
    }


    void OpenMenu() {
        menu.gameObject.SetActive(true);

        FindObjectOfType<AudioManager>().PlaySound("OpenMenu");

        menu.gameObject.transform.position = new Vector3(transform.position.x, GetComponent<XRRig>().cameraGameObject.transform.position.y - 0.1f, transform.position.z) + (transform.forward * 1.5f);
        menu.gameObject.transform.rotation = transform.rotation;
    }

    public void CloseMenu() {
        menu.gameObject.SetActive(false);
    }

    public void SetPhysicalResistance(Slider slider) {
        GetComponent<ClimbingExhaustionController>().minTimeToHiperventilate = slider.value;
    }

    public void ToggleTurnType() {
        GetComponent<Turning>().snapTurn = !GetComponent<Turning>().snapTurn;
    }

    public void BackToLastCheckPoint() {
        GetComponent<PlayerLifeController>().SubtractLife(150);

        CloseMenu();
    }

    public void ActivateContinuousLocomotion() {
        GetComponent<ArmSwinger>().blockedMovement = true;
        GetComponent<LocomotionController>().blockedTeleport = true;
        GetComponent<ContinuousMovement>().blockedMovement = false;
    }
    public void ActivateArmSwingerLocomotion() {
        GetComponent<ContinuousMovement>().blockedMovement = true;
        GetComponent<LocomotionController>().blockedTeleport = true;
        GetComponent<ArmSwinger>().blockedMovement = false;
    }
    public void ActivateTeleportLocomotion() {
        GetComponent<ContinuousMovement>().blockedMovement = true;
        GetComponent<ArmSwinger>().blockedMovement = true;
        GetComponent<LocomotionController>().blockedTeleport = false;
    }
    public void ActivateMixedLocomotion() {
        GetComponent<ContinuousMovement>().blockedMovement = false;
        GetComponent<ArmSwinger>().blockedMovement = false;
        GetComponent<LocomotionController>().blockedTeleport = false;
    }

    public void ChangeArmSwingerTrackingType(string type) {
        GetComponent<ArmSwinger>().movementDirectionType = (ArmSwinger.MovementDirectionTypes)Enum.Parse(typeof(ArmSwinger.MovementDirectionTypes), type);
    }

    public void ChangeTeleportType(string type) {
        GetComponent<Teleport>().teleportType = (Teleport.TeleportTypes)Enum.Parse(typeof(Teleport.TeleportTypes), type);
    }

    void TryInitializeDevice(ref InputDevice device, XRController controller) {
        device = InputDevices.GetDeviceAtXRNode(controller.controllerNode);
    }
}
