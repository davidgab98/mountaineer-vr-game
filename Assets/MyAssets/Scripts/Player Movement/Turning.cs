using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class Turning : MonoBehaviour {
    public XRNode inputSource;
    public bool snapTurn; //True: Snap turn / False: Continuous Turn
    public int snapTurnAmount;

    private InputDevice targetDevice;
    private Vector2 inputAxis;

    private float timeSinceLastSnapTurn = 0;
    private float timeBetweenSnapTurns = 0.3f;


    private void Start() {
        TryInitializeDevice();
    }

    private void Update() {
        if(!targetDevice.isValid)
        {
            TryInitializeDevice();
        } 
        else 
        {
            targetDevice.TryGetFeatureValue(CommonUsages.primary2DAxis, out inputAxis);
            if(snapTurn) {
                timeSinceLastSnapTurn += Time.deltaTime;
                if(inputAxis.x > 0.3f || inputAxis.x < -0.3f)
                    SnapTurn();
            } else {
                if(inputAxis.x > 0.3f || inputAxis.x < -0.3f)
                    ContinuousTurn();
            }
        }
    }

    void ContinuousTurn() {
        // Calculo de SharkJets (video: Quest 2 Unity Gamedev - Part 7 - Movement and Turning)
        float rotationAmount = transform.eulerAngles.y + inputAxis.x;
        Vector3 direction = new Vector3(transform.eulerAngles.x, rotationAmount, transform.eulerAngles.z);
        transform.rotation = Quaternion.Euler(direction);
    }

    void SnapTurn() {
        if(timeSinceLastSnapTurn > timeBetweenSnapTurns) {
            int sideToTurn = inputAxis.x > 0 ? 1 : -1; // si (inputAxis.x > 0) -> sideToTurn = 1; else -> sideToTurn = -1
            float rotationAmount = transform.eulerAngles.y + (snapTurnAmount * sideToTurn);
            Vector3 direction = new Vector3(transform.eulerAngles.x, rotationAmount, transform.eulerAngles.z);
            transform.rotation = Quaternion.Euler(direction);

            timeSinceLastSnapTurn = 0;
        }
    }

    void TryInitializeDevice() {
        targetDevice = InputDevices.GetDeviceAtXRNode(inputSource);
    }

}
