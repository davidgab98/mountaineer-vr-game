using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class ArmSwinger : MonoBehaviour
{
    public XRController leftController, rightController;
    public enum MovementDirectionTypes { HEAD, CONTROLLERS};
    public MovementDirectionTypes movementDirectionType;

    public float additionalHeight = 0.2f;

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
        CapsuleFollowHeadset();

        Vector3 direction = Vector3.zero;

        if(movementDirectionType == MovementDirectionTypes.HEAD) {
            // Move horizontally by head direction
            direction = rig.cameraGameObject.transform.forward.normalized;
        }else if(movementDirectionType == MovementDirectionTypes.CONTROLLERS) {
            // Move horizontally by controllers direction
            direction = (leftController.transform.forward.normalized + rightController.transform.forward.normalized).normalized;
        }

        character.Move(direction * Time.fixedDeltaTime * speed);

        // Move vertically
        character.Move(Vector3.up * Physics.gravity.y * Time.fixedDeltaTime);
    }

    // Para rotar el CharacterCollider(capsule) segun la camara (a donde mire el player) 
    void CapsuleFollowHeadset() {
        character.height = rig.cameraInRigSpaceHeight + additionalHeight;
        // InverseTransformPoint: da la posicion local que tendria el objeto si fuera un hijo de la camara
        Vector3 capsuleCenter = transform.InverseTransformPoint(rig.cameraGameObject.transform.position);
        character.center = new Vector3(capsuleCenter.x, character.height / 2 + character.skinWidth, capsuleCenter.z);
    }

    void TryInitializeDevice(ref InputDevice device, XRController controller) {
        device = InputDevices.GetDeviceAtXRNode(controller.controllerNode);
    }
}
