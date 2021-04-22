using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PlayerLifeController : MonoBehaviour
{
    public Volume grayscaleVolume;

    private ColorCurves grayscaleEffect;

    [SerializeField]
    private float currentLife = 100;
    [SerializeField]
    private float maxLife = 100;
    [SerializeField]
    private float lifeIncreaseFactor = 4;

    // Start is called before the first frame update
    void Start()
    {
        if(grayscaleVolume.profile.TryGet<ColorCurves>(out ColorCurves cc)) {
            grayscaleEffect = cc;
        }
    }

    public void SubtractLife(float subtractedLife) {
        currentLife -= subtractedLife;
    }

    public void UpdateMaxLife(float newMaxLife) {
        maxLife = newMaxLife;
        currentLife = maxLife;
    }

    // Update is called once per frame
    void Update()
    {
        if(currentLife < maxLife) {
            currentLife += lifeIncreaseFactor * Time.deltaTime;
        }else if(currentLife > maxLife) {
            currentLife = maxLife;
        }else if(currentLife <= 0) {
            // Player dead
        }

        grayscaleEffect.hueVsSat.value.MoveKey(0, new Keyframe(0, (currentLife / maxLife) / 2));
    }
}
