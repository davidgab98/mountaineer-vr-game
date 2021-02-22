using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

// ***** WAYS TO GET AN INPUT ***** //

public class InputNotes : MonoBehaviour
{
    private InputDevice targetDevice;
    private Vector2 inputAxis;

    private void Update() {
        // Si no encontramos un Device intentamos buscarlo hasta que lo encontremos
        if(!targetDevice.isValid)  // Usamos isValid en lugar de == null
        {
            TryGetDevice();
        } 
        else 
        {
            GetInputFromDevice();
        }
    }


    // *** FORMAS DE OBTENER UN DEVICE *** //
    private void TryGetDevice() {
        List<InputDevice> devices = new List<InputDevice>();

    // Obtener todos los devices conectados (gafas y controladores)
        InputDevices.GetDevices(devices);


    // Obtener todos los devices conectados con unas caracteristicas concretas
        InputDeviceCharacteristics characteristics = InputDeviceCharacteristics.Right | InputDeviceCharacteristics.Controller; //en este caso solo entraria a devices el controlador derecho
        InputDevices.GetDevicesWithCharacteristics(characteristics, devices);
        targetDevice = devices[0];


    // Obtener un device a partir de un XRNode
        XRNode inputSource = XRNode.LeftHand;
        targetDevice = InputDevices.GetDeviceAtXRNode(inputSource);


    //Obtener un device a partir de un XRController obteniendo su XRNode asociado
        XRController controller = null;
        targetDevice = InputDevices.GetDeviceAtXRNode(controller.controllerNode);
    }


    // *** FORMAS DE OBTENER UN INPUT A PARTIR DE UN DEVICE *** //
    private void GetInputFromDevice() {

    // Device.TryGetFeatureValue
        targetDevice.TryGetFeatureValue(CommonUsages.primary2DAxis, out inputAxis); //Obtenemos en inputAxis el valor recogido del primary2DAxis (joystick principal) del targetDevice

        // Podemos meter la llamada en un if de forma que si no encuentra el CommonUsages.x en el device, no entrará al device. Esto puede
        // pasar si usamos un controlador que no tiene algun boton al que estemos intentando acceder: ENTENDER BIEN ESTO: No da error si el boton no existe
        if(targetDevice.TryGetFeatureValue(CommonUsages.primary2DAxis, out inputAxis)) {

        }

        // Si queremos comprobar además que el valor obtenido cumpla cierta condicion (superar un umbral, por ejemplo) hacemos:
        if(targetDevice.TryGetFeatureValue(CommonUsages.primary2DAxis, out inputAxis) && inputAxis.x > 0.2) {
            // entrará aquí si target device posee un boton primary2DAxis y además la x de la salida obtenida supera 0.2
        }


    // InputHelpers.IsPressed : Atajo para chequear si se ha pulsado un boton (Valor true o false)
        InputHelpers.Button buttonToChek = InputHelpers.Button.Primary2DAxisClick;
        bool isActivated;
        float activationThreshold = 0.1f; // valor minimo para activarse (isActivated: true, si el valor minimo es superado)
        InputHelpers.IsPressed(targetDevice, buttonToChek, out isActivated, activationThreshold);
    }
}
