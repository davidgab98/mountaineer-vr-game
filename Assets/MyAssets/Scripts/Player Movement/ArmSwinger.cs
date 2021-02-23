using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class ArmSwinger : MonoBehaviour
{
    public XRController leftController, rightController;
    public enum MovementDirectionTypes { HEAD, CONTROLLERS};
    public MovementDirectionTypes movementDirectionTypes;

    private InputDevice leftControllerDevice, rightControllerDevice;
    private XRRig rig; // Get the XRRig to acces to the head (camera) 
    private CharacterController character;

    private float speed;

    private void Start() {
        character = GetComponent<CharacterController>();
        rig = GetComponent<XRRig>();

        TryInitializeDevice(ref leftControllerDevice, leftController);
        TryInitializeDevice(ref rightControllerDevice, rightController);
    }

    private void Update() {
        speed = 0;

        if(!leftControllerDevice.isValid) {
            TryInitializeDevice(ref leftControllerDevice, leftController);
        } else {
            if(CheckButtonToWalk(leftControllerDevice))
                speed += Mathf.Abs(GetDeviceVelocity(leftControllerDevice).y); 
        }

        if(!rightControllerDevice.isValid) {
            TryInitializeDevice(ref rightControllerDevice, rightController);
        } else {
            if(CheckButtonToWalk(rightControllerDevice))
                speed += Mathf.Abs(GetDeviceVelocity(rightControllerDevice).y);
        }
    }

    bool CheckButtonToWalk(InputDevice device) {
        device.TryGetFeatureValue(CommonUsages.primaryButton, out bool tryingWalk);

        return tryingWalk;
    }

    Vector3 GetDeviceVelocity(InputDevice device) {
        device.TryGetFeatureValue(CommonUsages.deviceVelocity, out Vector3 velocity);

        return velocity;
    }

    private void FixedUpdate() {
        Vector3 direction = Vector3.zero;

        if(movementDirectionTypes == MovementDirectionTypes.HEAD) {
            // Move horizontally by head direction
            direction = rig.cameraGameObject.transform.forward.normalized;
        }else if(movementDirectionTypes == MovementDirectionTypes.CONTROLLERS) {
            // Move horizontally by controllers direction
            direction = (leftController.transform.forward.normalized + rightController.transform.forward.normalized).normalized;
        }

        character.Move(direction * Time.fixedDeltaTime * speed);

        // Move vertically
        character.Move(Vector3.up * Physics.gravity.y * Time.fixedDeltaTime);
    }

    void TryInitializeDevice(ref InputDevice device, XRController controller) {
        device = InputDevices.GetDeviceAtXRNode(controller.controllerNode);
    }
}
