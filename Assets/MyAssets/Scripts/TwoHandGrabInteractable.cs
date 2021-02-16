using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

/**********
Existe un problema con este componente: 
Si agarramos el objeto con el RayInteractor, y el SecondGrabPoint esta cerca del GrabInteractable que hemos agarrado
con la primera mano (con el rayInteractor), este secondGrabPoint se agarra con el DirectInteractor de la primera mano, quedandose pues ambos puntos agarrados con la primera mano
y por tanto dando un fallo en los calculos de rotación del objeto con ambas manos. Se puede solucionar poniendo como capa GrabIgnoreRay al objeto, pero si queremos que se pueda
coger con el RayInteractor, debemos hacer algun cambio en el componente.
**********/

//Por como esta constuido, el componente base XRGrabInteractable solo puede registrar un interactor a la vez
//Para un agarre con dos manos, en lugar de intentar registrar dos interactors para el mismo interactable,
//lo que hacemos es habilitar puntos se segundo agarre (Second Grab Points) cuando el objeto interactable ya este siendo 
//agarrado por un primer interactor

public class TwoHandGrabInteractable : XRGrabInteractable {
    public List<XRSimpleInteractable> secondHandGrabPoints = new List<XRSimpleInteractable>();
    private XRBaseInteractor secondInteractor;

    // Guardamos aqui la rotacion local del interactor principal antes de que el objeto interactable sea agarrado con ambas manos (ya que cuando 
    // esto ocurre, rotamos el interactor principal segun donde mire el segundo interactor (second hang grab)), y de esta forma, cuando soltamos
    // el objeto, devolvemos al interactor principal su rotación original
    private Quaternion attachInitialLocalRotation;

    // Para rotar el objeto interactable segun la rotación (y no solo la posición) de nuestras manos, podemos hacerlo segun la rotación de la
    // primera mano (selectingInteractor), de la segunda mano (secondInteractor) o de ninguna (el objeto no rota segun la rotación de nuestas manos)
    public enum TwoHandRotationType { None, FirstHand, SecondHand };
    public TwoHandRotationType twoHandRotationType;

    // Cuando agarramos el objeto con la segunda mano, el objeto interactable hace un pequeño snap debido a que rotamos el objeto interactable en ProcessInteractable()
    // Podemos evitar ese snap o rotación extra de la siguiente manera:
    public bool snapToSecondHand = true; // true - evitamos snap, false - no lo evitamos
    // Esta variable guardará la rotacíón "extra" que se aplica al agarrar el objeto con la segunda mano y la usaremos para restaurarla cuando se produzca y así evitar este comportamiento
    private Quaternion initialRotationOffset;


    private void Start() {
        // De esta forma podemos llamar a las funciones de OnSecondHandGrab/Release cuando seleccionamos o deseleccionamos 
        // los simpleInteractable de los SecondHandGrabPoints
        foreach(var item in secondHandGrabPoints) {
            item.onSelectEnter.AddListener(OnSecondHandGrab);
            item.onSelectExit.AddListener(OnSecondHandRelease);
        }
    }

    // To override the object movement
    public override void ProcessInteractable(XRInteractionUpdateOrder.UpdatePhase updatePhase) {
        // Antes de mover el objeto en la funcion base (cuando lo tenemos agarrado con ambas manos), vamos a calcular la rotación que debe tener 
        // según la posición y la rotación de nuestras manos (GetTwoHandRotation). Para ello, en lugar de rotar todo el objeto, vamos a rotar el attachTransform del interactor principal
        // que esta siendo usado como pivote para mover el objeto interactable.
        
        if(secondInteractor && selectingInteractor) {

            if(snapToSecondHand)
                selectingInteractor.attachTransform.rotation = GetTwoHandRotation();
            else
                selectingInteractor.attachTransform.rotation = GetTwoHandRotation() * initialRotationOffset; //Para evitar el snap, multiplicamos la nueva rotación que resultaría de agarrar con ambas manos, por la diferencia de rotación calculada
        }

        base.ProcessInteractable(updatePhase);
    }

    // Calculamos la rotación que debe tener el objeto interactable según la posición y la rotación de nuestras manos
    private Quaternion GetTwoHandRotation() {
        Quaternion targetRotation;

        // Con Quaternion.LookRotation obtenemos una rotación según un vector (forward) y si queremos también según un segundo vector (upward) que le pasamos por parámetro 

        //La rotación del objeto según la posición de nuestras manos:
            // calculamos el vector entre la primera mano y la segunda (este sera el parametro que le pasamos a Quaternion.LookRotation): (secondInteractor.attachTransform.position - selectingInteractor.attachTransform.position)

        //Según la rotación: 
            // podemos hacerlo segun la rotación de la primera mano (selectingInteractor), de la segunda mano (secondInteractor) o de ninguna (el objeto no rota segun la rotación de nuestas manos)
            // para ello, le pasamos a Quaternion.LookRotation un segundo parametro que es el vector Up de la mano de la cual queremos que siga la rotación, y así, la rotación que nos calcule Quaternion.LookRotation también tendrá en cuenta este vector

        if(twoHandRotationType == TwoHandRotationType.FirstHand) {
            // En un comentario en el video de Valem: si usamos "First" y agarras el objeto con ambas manos y haces circulos en el aire con la primera mano, el objeto rotará de una forma inesperada: se soluciona cambiando el <attachTransform> por <transform>
                // targetRotation = Quaternion.LookRotation(secondInteractor.attachTransform.position - selectingInteractor.attachTransform.position, selectingInteractor.attachTransform.up); 
            targetRotation = Quaternion.LookRotation(secondInteractor.attachTransform.position - selectingInteractor.attachTransform.position, selectingInteractor.transform.up);
        } else if(twoHandRotationType == TwoHandRotationType.SecondHand) {
            targetRotation = Quaternion.LookRotation(secondInteractor.attachTransform.position - selectingInteractor.attachTransform.position, secondInteractor.attachTransform.up);
        } else { //None
            targetRotation = Quaternion.LookRotation(secondInteractor.attachTransform.position - selectingInteractor.attachTransform.position);
        }

        return targetRotation;
    }

    // When we grab with the second hand
    public void OnSecondHandGrab(XRBaseInteractor interactor) {
        if(selectingInteractor && !selectingInteractor.Equals(interactor)) {
            Debug.Log("Second Hand Grab");
            secondInteractor = interactor;

            // Calculamos la diferencia de rotacion entre la rotación inicial (cuando el objeto esta agarrado solo con la primera mano) y
            // la nueva rotación que se produce al agarrarlo con la segunda mano (que obtenemos de GetTwoHandRotation()) 
            // Para calcular esta diferencia, multiplicamos la Inversa de la nueva rotación x la rotación actual
            // Más sobre este calculo aquí: https://forum.unity.com/threads/get-the-difference-between-two-quaternions-and-add-it-to-another-quaternion.513187/
            initialRotationOffset = Quaternion.Inverse(GetTwoHandRotation()) * selectingInteractor.attachTransform.rotation;
        }
    }

    // When we release with the second hand
    public void OnSecondHandRelease(XRBaseInteractor interactor) {
        Debug.Log("Second Hand Release");

        secondInteractor = null;
    }

    // When we grab with the first hand
    protected override void OnSelectEnter(XRBaseInteractor interactor) {
        Debug.Log("First Hand Enter");
        base.OnSelectEnter(interactor);

        attachInitialLocalRotation = interactor.attachTransform.localRotation;
    }

    // When we release with the first hand
    protected override void OnSelectExit(XRBaseInteractor interactor) {
        Debug.Log("First Hand Exit");
        base.OnSelectExit(interactor);

        // De esta forma forzamos a soltar el objeto con ambas manos cuando lo soltamos con el interactor principal (agarre principal)
        secondInteractor = null;

        interactor.attachTransform.localRotation = attachInitialLocalRotation;
    }

    public override bool IsSelectableBy(XRBaseInteractor interactor) {
        // De esta forma comprobamos si el objeto ya esta agarrado, y si lo está, evitamos que se pueda agarrar con un interactor diferente (pasar de un interactor a otro)
        bool isAlreadyGrabbed = selectingInteractor && !interactor.Equals(selectingInteractor);

        return base.IsSelectableBy(interactor) && !isAlreadyGrabbed;
    }
}
