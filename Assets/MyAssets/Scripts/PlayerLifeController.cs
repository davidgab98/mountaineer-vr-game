using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PlayerLifeController : MonoBehaviour
{
    [SerializeField]
    private float currentLife = 100;
    [SerializeField]
    private float maxLife = 100;
    [SerializeField]
    private float lifeIncreaseFactor = 4;


    public static Vector3 lastCheckPointPosition;
    [SerializeField]
    private ImageFader fader;
    private float screenFadingTime;
    [SerializeField]
    private float screenFadingDuration = 1f;

    private CharacterController character;

    [SerializeField] 
    private Volume grayscaleVolume;
    private ColorCurves grayscaleEffect;

    private void Awake() {
        character = GetComponent<CharacterController>();
    }

    // Start is called before the first frame update
    void Start()
    {
        if(grayscaleVolume.profile.TryGet<ColorCurves>(out ColorCurves cc)) {
            grayscaleEffect = cc;
        }

        lastCheckPointPosition = character.transform.position;
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
        if(currentLife < maxLife && currentLife > 0) {
            currentLife += lifeIncreaseFactor * Time.deltaTime;
        }else if(currentLife > maxLife) {
            currentLife = maxLife;
        }else if(currentLife <= 0) {
            ResurrectAtLastCheckPoint(); 
        }

        grayscaleEffect.hueVsSat.value.MoveKey(0, new Keyframe(0, (currentLife / maxLife) / 2));
    }

    public void ResurrectAtLastCheckPoint() {
        fader.FadeOut(screenFadingDuration);

        character.transform.rotation = new Quaternion(character.transform.rotation.x, character.transform.rotation.y, character.transform.rotation.z + 0.8f * Time.deltaTime, character.transform.rotation.w);

        GetComponent<VerticalMovement>().enabled = false;

        screenFadingTime += Time.deltaTime;
        if(screenFadingTime >= screenFadingDuration) {
            character.transform.position = new Vector3(lastCheckPointPosition.x, lastCheckPointPosition.y, lastCheckPointPosition.z - 2f);
            character.transform.rotation = new Quaternion(character.transform.rotation.x, character.transform.rotation.y, 0, character.transform.rotation.w);

            fader.FadeIn(screenFadingDuration);

            GetComponent<VerticalMovement>().enabled = true;

            currentLife = maxLife;
            screenFadingTime = 0;
        }
    }
}
