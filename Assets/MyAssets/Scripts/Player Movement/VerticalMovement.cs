using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerticalMovement : MonoBehaviour {

    public float gravity = -9.81f;
    public LayerMask groundLayer;

    public float fallingSpeed;
    public bool blockedFall;
    public bool isGrounded;

    private CharacterController character;
    private PlayerLifeController lifeController;


    void Awake() {
        character = GetComponent<CharacterController>();
        lifeController = GetComponent<PlayerLifeController>();
    }

    private void FixedUpdate() {
        // Easy Way: character.Move(Vector3.up * Physics.gravity.y * Time.fixedDeltaTime);

        // Move vertically
        // De esta forma solo aplicamos fuerza negativa hacia abajo cuando CheckIfGrounded = false (no estemos tocando el suelo: gameobjects con layer=Ground)
        // Puede dar problemas derivados de la longitud del rayCast que usamos para calcular si estamos tocando el suelo o no, pero da mucho más juego
        isGrounded = CheckIfGrounded();
        if(isGrounded) {
            if(Mathf.Abs(fallingSpeed) > 0) // Si la velocidad de caida es mayor que 0, hemos golpeado contra el suelo
                HitTheGround();

            fallingSpeed = 0; // Si estamos tocando el suelo, ponemos la velocidad de caida a 0, no caemos
        } else if(!blockedFall) { //Si la caída no esta bloqueada (podria estar bloqueada por ejemplo por Climber)
            fallingSpeed += gravity * Time.fixedDeltaTime; // Si no estamos tocando el suelo aumentamos la velocidad de caida con la gravedad
        }

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

    private void HitTheGround() {
        if(Mathf.Abs(fallingSpeed) > 10) {
            lifeController.SubtractLife(Mathf.Abs(fallingSpeed));
        }
    }
}
