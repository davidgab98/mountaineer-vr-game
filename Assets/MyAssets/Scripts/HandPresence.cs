using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class HandPresence : MonoBehaviour {
    public bool showController = false;
    public InputDeviceCharacteristics controllerCharacteristics;
    public List<GameObject> controllerPrefabs;
    public GameObject handModelPrefab;

    private InputDevice targetDevice;
    private GameObject spawnedController;
    private GameObject spawnedHandModel;
    private Animator handAnimator;

    void Start() {
        TryInitialize();
    }

    void TryInitialize() {
        List<InputDevice> devices = new List<InputDevice>();

        /*De esta forma agregamos a <devices> todos los devices conectados (gafas y controladores)
        InputDevices.GetDevices(devices);

        foreach(var item in devices) {
            Debug.Log("Name: "+ item.name + " /// Characteristics: "+item.characteristics);
        }*/

        //De esta forma agregamos a <devices> todos los devices con caracteristicas concretas 
        /*InputDeviceCharacteristics rightControllerCharacteristics = InputDeviceCharacteristics.Right | InputDeviceCharacteristics.Controller; (en este caso Right y Controller: solo entraria a devices el controlador derecho)
        InputDevices.GetDevicesWithCharacteristics(rightControllerCharacteristics, devices);*/
        InputDevices.GetDevicesWithCharacteristics(controllerCharacteristics, devices);

        foreach(var item in devices) {
            Debug.Log("Name: " + item.name + " /// Characteristics: " + item.characteristics);
        }

        if(devices.Count > 0) {
            targetDevice = devices[0];
            GameObject prefab = controllerPrefabs.Find(controller => controller.name == targetDevice.name);

            if(prefab) {
                spawnedController = Instantiate(prefab, transform);
            } else {
                Debug.Log("Did not find corresponding controller model");
                spawnedController = Instantiate(controllerPrefabs[0], transform);
            }

            spawnedHandModel = Instantiate(handModelPrefab, transform);
            handAnimator = spawnedHandModel.GetComponent<Animator>(); //Cogemos asi el Animator Component del modelo de la mano
        }
    }

    // Update is called once per frame
    void Update() {

        //Si no encontramos un targetDevice intentamos buscarlo hasta que lo encontremos
        if(!targetDevice.isValid)  { //Para esto, usamos .isValid en lugar de == null
            TryInitialize();
        } else {
            /*Con esto descomentado, cuando pulsemos el triggerButton se mostrara el modelo del controlador en lugar de las manos: 
            targetDevice.TryGetFeatureValue(CommonUsages.trigger, out float triggerValue);
            if(triggerValue > 0.5) {
                showController = true;
            } else {
                showController = false;
            }*/
            if(!showController) {
                spawnedController.SetActive(false);
                spawnedHandModel.SetActive(true);
                UpdateHandAnimation();
            } else {
                spawnedController.SetActive(true);
                spawnedHandModel.SetActive(false);
            }
        }

        


        //#### EJEMPLO: LLAMADAS A LOS BOTONES (INPUTS) DEL CONTROLADOR ####/
        //targetDevice.TryGetFeatureValue(CommonUsages.primaryButton, out bool primaryButtonValue) 
        //if(primaryButtonValue){}
        //En el codigo sin comentar, metemos la llamada en la condicion del if porque si no encuentra el CommonUsages.x en el device, no entrará al device, esto puede
        //pasar si usamos un controlador que no tiene algun boton al que nosotros estemos intentando acceder

        /*Chequeando que se pulsa el primaryButton       
        if(targetDevice.TryGetFeatureValue(CommonUsages.primaryButton, out bool primaryButtonValue) && primaryButtonValue) {
            Debug.Log("Pressing primary button");
        }*/

        /*Chequeando que se pulsa el trigger      
        if(targetDevice.TryGetFeatureValue(CommonUsages.trigger, out float triggerValue) && triggerValue > 0.1) {
            Debug.Log("Pressing trigger with value: " + triggerValue);
        }*/

        /*Chequeando que se pmueve el joystick
        if(targetDevice.TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 primary2DAxisValue)  && primary2DAxisValue != Vector2.zero) {
            Debug.Log("Moving 2d axis - x: " + primary2DAxisValue.x + " / y: " + primary2DAxisValue.y);
        }*/
    }

    void UpdateHandAnimation() {
        if(targetDevice.TryGetFeatureValue(CommonUsages.trigger, out float triggerValue)) {
            handAnimator.SetFloat("Trigger", triggerValue);
        } else {
            handAnimator.SetFloat("Trigger", 0);
        }

        if(targetDevice.TryGetFeatureValue(CommonUsages.grip, out float gripValue)) {
            handAnimator.SetFloat("Grip", gripValue);
        } else {
            handAnimator.SetFloat("Grip", 0);
        }
    }
}
