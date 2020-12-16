using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class Climber : MonoBehaviour
{
    private CharacterController character;
    public  static XRController climbingHand;
    private ContinuousMovement  continuousMovement;

    void Start() {
        character          = GetComponent<CharacterController>();
        continuousMovement = GetComponent<ContinuousMovement>();
    }

    void FixedUpdate()
    {
        if(climbingHand) {
            continuousMovement.enabled = false;
            Climb();
        } else {
            continuousMovement.enabled = true;
        }
    }

    //Climbing Computations
    void Climb() 
    {
        //Cogemos el Device a partir del XRController y leemos su velocidad (la del device) escribiendola en la salida: out vector3 velocity
        InputDevices.GetDeviceAtXRNode(climbingHand.controllerNode).TryGetFeatureValue(CommonUsages.deviceVelocity, out Vector3 velocity);

        //Movemos al player con la velocidad contraria a la de la mano (importante multiplicar por la rotación del player)
        character.Move(transform.rotation * -velocity * Time.fixedDeltaTime);
    }
}
