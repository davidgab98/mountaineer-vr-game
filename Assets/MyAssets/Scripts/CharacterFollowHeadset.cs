using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

// Mediante este componente rotamos el CharacterCollider segun la camara (a donde mire el player)
public class CharacterFollowHeadset : MonoBehaviour
{
    public float additionalHeight = 0.2f;

    private XRRig rig; // Get the XRRig to acces to the head (camera) 
    private CharacterController character;

    private void Start() {
        character = GetComponent<CharacterController>();
        rig = GetComponent<XRRig>();
    }

    private void FixedUpdate() {
        character.height = rig.cameraInRigSpaceHeight + additionalHeight;
        Vector3 capsuleCenter = transform.InverseTransformPoint(rig.cameraGameObject.transform.position);  // InverseTransformPoint: da la posicion local que tendria el objeto si fuera un hijo de la camara
        character.center = new Vector3(capsuleCenter.x, character.height / 2 + character.skinWidth, capsuleCenter.z);
    }
}
