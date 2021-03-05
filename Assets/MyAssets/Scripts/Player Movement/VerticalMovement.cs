using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerticalMovement : MonoBehaviour {

    public float gravity = -9.81f;
    public LayerMask groundLayer;
    public GameObject velocityParticles;
    public float minFallSpeedForEffects = 20;

    private float fallingSpeed;
    private CharacterController character;


    void Start() {
        character = GetComponent<CharacterController>();
    }

    private void Update() {
        if(fallingSpeed <= minFallSpeedForEffects) {
            velocityParticles.SetActive(true);
        } else {
            velocityParticles.SetActive(false);
        }
    }

    private void FixedUpdate() {
        // Easy Way: character.Move(Vector3.up * Physics.gravity.y * Time.fixedDeltaTime);

        // Move vertically
        // De esta forma solo aplicamos fuerza negativa hacia abajo cuando CheckIfGrounded = false (no estemos tocando el suelo: gameobjects con layer=Ground)
        // Puede dar problemas derivados de la longitud del rayCast que usamos para calcular si estamos tocando el suelo o no, pero da mucho más juego
        bool isGrounded = CheckIfGrounded();
        if(isGrounded)
            fallingSpeed = 0; // Si estamos tocando el suelo, ponemos la velocidad de caida a 0, no caemos
        else
            fallingSpeed += gravity * Time.fixedDeltaTime; // Si no estamos tocando el suelo aumentamos la velocidad de caida con la gravedad

        Debug.Log(fallingSpeed);

        character.Move(Vector3.up * fallingSpeed * Time.fixedDeltaTime); // Esto podria hacerse solo cuando isGrounded es false y nos ahorrariamos hacerlo cada vezs
    }

    private bool CheckIfGrounded() {
        // Para comprobar si tocamos el suelo usamos un sphereCast, es como un rayCast pero más ancho. Valem explica muy bien el por qué usar
        // un sphereRaycast en este caso en su video de ContinuousMovement (Ver para explicar)

        // Cogemos el centro del player (global) y la longitud que queramos que tenga el SphereCast
        Vector3 rayStart = transform.TransformPoint(character.center);
        float rayLenght = character.center.y + 0.1f;

        // Creamos el sphereCast y guardamos en hasHit si colisiona o no con groundLayer
        bool hasHit = Physics.SphereCast(rayStart, character.radius, Vector3.down, out RaycastHit hitInfo, rayLenght, groundLayer);
        return hasHit;
    }
}
