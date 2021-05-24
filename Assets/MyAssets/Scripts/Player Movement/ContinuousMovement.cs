using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class ContinuousMovement : MonoBehaviour {
    public float walkingSpeed = 1;
    public float runningSpeed = 2;
    public XRNode inputSource;
    public float additionalHeight = 0.2f;

    private XRRig rig; // Get the XRRig to acces to the head (camera)
    private Vector2 inputAxis; //Update by primary2dAxis
    private bool running; //true if Primary2DAxisClick is clicked
    private CharacterController character;
    private VerticalMovement vm;

    void Start() {
        character = GetComponent<CharacterController>();
        vm = GetComponent<VerticalMovement>();
        rig = GetComponent<XRRig>();
    }

    void Update() {
        // Otra forma de coger un device diferente a GetDevicesWithCharacteristics (mas sencilla). Se usa un XRNode.
        InputDevice device = InputDevices.GetDeviceAtXRNode(inputSource);

        device.TryGetFeatureValue(CommonUsages.primary2DAxis, out inputAxis);
        device.TryGetFeatureValue(CommonUsages.primary2DAxisClick, out running);
    }

    // FIXED UPDATE se ejecuta cada vez que Unity ejecuta las fisicas
    // PODEMOS AUMENTAR CADA CUANTO SE EJECUTAN LAS FISICAS EN ProjectSetting - Time - FixedTimeStep
    private void FixedUpdate() {
        CapsuleFollowHeadset();

        // Move horizontaly
        Quaternion headY = Quaternion.Euler(0, rig.cameraGameObject.transform.eulerAngles.y, 0); // Get the y rotation of the camera (head) of the xrrig
        Vector3 direction = headY * new Vector3(inputAxis.x, 0, inputAxis.y);  // Multiply the rotation of the head in y by the direction to move, then we move considering where the player is looking

        if(!running)
            character.Move(direction * Time.fixedDeltaTime * walkingSpeed);
        else
            character.Move(direction * Time.fixedDeltaTime * runningSpeed);

        if(character.velocity.magnitude > 2 && vm.isGrounded) {
            WalkWithSound();
        }
    }

    private void WalkWithSound() {
        if(!running) {
            if(vm.currentLayerHitting == LayerMask.NameToLayer("Ground")){
                FindObjectOfType<AudioManager>().PlayVariableSound("StepSnowWalk");
            } else if(vm.currentLayerHitting == LayerMask.NameToLayer("GroundWood")) {
                FindObjectOfType<AudioManager>().PlaySerialSound("StepWoodWalk");
            }
        } else {
            if(vm.currentLayerHitting == LayerMask.NameToLayer("Ground")) {
                FindObjectOfType<AudioManager>().PlayVariableSound("StepSnowRun");
            } else if(vm.currentLayerHitting == LayerMask.NameToLayer("GroundWood")) {
                FindObjectOfType<AudioManager>().PlaySerialSound("StepWoodWalk");
            }
        }
    }

    // Para rotar el CharacterCollider(capsule) segun la camara (a donde mire el player) 
    void CapsuleFollowHeadset() {
        character.height = rig.cameraInRigSpaceHeight + additionalHeight;
        // InverseTransformPoint: da la posicion local que tendria el objeto si fuera un hijo de la camara
        Vector3 capsuleCenter = transform.InverseTransformPoint(rig.cameraGameObject.transform.position);
        character.center = new Vector3(capsuleCenter.x, character.height / 2 + character.skinWidth, capsuleCenter.z);
    }
}
