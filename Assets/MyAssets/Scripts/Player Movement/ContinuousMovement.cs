using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class ContinuousMovement : MonoBehaviour {
    public float walkingSpeed = 1;
    public float runningSpeed = 2;
    public XRNode inputSource;
    public float gravity = -9.81f;
    public LayerMask groundLayer;
    public float additionalHeight = 0.2f;

    private float fallingSpeed;
    private XRRig rig; // Get the XRRig to acces to the head (camera)
    private Vector2 inputAxis; //Update by primary2dAxis
    private bool running; //true if Primary2DAxisClick is clicked
    private CharacterController character;

    void Start() {
        character = GetComponent<CharacterController>();
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

        // Move vertical
        character.Move(Vector3.up * gravity * Time.fixedDeltaTime);

        /* USING A RAYCAST TO CHECK IF GROUNDED
            // De esta forma solo aplicamos fuerza negativa hacia abajo cuando CheckIfGrounded = false (no estemos tocando el suelo: gameobjects con layer=Ground)
            // Puede dar problemas derivados de la longitud del rayCast que usamos para calcular si estamos tocando el suelo o no, pero da mucho más juego
        bool isGrounded = CheckIfGrounded();
        if(isGrounded)
            fallingSpeed = 0; // Si estamos tocando el suelo, ponemos la velocidad de caida a 0, no caemos
        else
            fallingSpeed += gravity * Time.fixedDeltaTime; // Si no estamos tocando el suelo aumentamos la velocidad de caida con la gravedad
        
        character.Move(Vector3.up * fallingSpeed); // Esto podria hacerse solo cuando isGrounded es false y nos ahorrariamos hacerlo cada vez
         */
    }

    // Para rotar el CharacterCollider(capsule) segun la camara (a donde mire el player) 
    void CapsuleFollowHeadset() {
        character.height = rig.cameraInRigSpaceHeight + additionalHeight;
        // InverseTransformPoint: da la posicion local que tendria el objeto si fuera un hijo de la camara
        Vector3 capsuleCenter = transform.InverseTransformPoint(rig.cameraGameObject.transform.position);
        character.center = new Vector3(capsuleCenter.x, character.height / 2 + character.skinWidth, capsuleCenter.z);
    }

    // Chequea si estamos tocando el suelo 
    private bool CheckIfGrounded() {
        // Para comprobar si tocamos el suelo usamos un sphereCast, es como un rayCast pero más ancho

        // Cogemos el centro del player (global) y la longitud que queramos que tenga el SphereCast
        Vector3 rayStart = transform.TransformPoint(character.center);
        float rayLenght = character.center.y + 0.5f;

        // Creamos el sphereCast y guardamos en hasHit si colisiona o no con groundLayer
        bool hasHit = Physics.SphereCast(rayStart, character.radius, Vector3.down, out RaycastHit hitInfo, rayLenght, groundLayer);
        return hasHit;
    }
}
