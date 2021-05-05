using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyboxController : MonoBehaviour
{
    public float rotationSkyboxSpeed = 1f;
    private Material skybox;
    void Start()
    {
        skybox = RenderSettings.skybox;
    }

    void Update()
    {
        skybox.SetFloat("_Rotation", skybox.GetFloat("_Rotation") + Time.deltaTime * rotationSkyboxSpeed);

    }
}
