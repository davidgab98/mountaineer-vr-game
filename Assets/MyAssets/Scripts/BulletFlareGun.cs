using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletFlareGun : MonoBehaviour
{
    private Light bullet_flarelight;
    private AudioSource flaresound;
    private ParticleSystemRenderer flareParticleSystem;

    private bool myCoroutine;
    private float smoothOff = 2.4f;
    private float flareTimer = 10;

    void Start()
    {
        StartCoroutine("flareLight");

        GetComponent<AudioSource>().Play();

        bullet_flarelight = GetComponent<Light>();
        flaresound = GetComponent<AudioSource>();
        flareParticleSystem = GetComponent<ParticleSystemRenderer>();

        Destroy(gameObject, flareTimer + 1f);
    }

    // Update is called once per frame
    void Update()
    {
        if(myCoroutine == true) {
            bullet_flarelight.intensity = Random.Range(2f, 6.0f);
        }else {
            bullet_flarelight.intensity = Mathf.Lerp(bullet_flarelight.intensity, 0f, Time.deltaTime * smoothOff);
            bullet_flarelight.range = Mathf.Lerp(bullet_flarelight.range, 0f, Time.deltaTime * smoothOff);
            flaresound.volume = Mathf.Lerp(flaresound.volume, 0f, Time.deltaTime * smoothOff);
            flareParticleSystem.maxParticleSize = Mathf.Lerp(flareParticleSystem.maxParticleSize, 0f, Time.deltaTime * 5);
        }
    }

    IEnumerator flareLight() {
        myCoroutine = true;
        yield return new WaitForSeconds(flareTimer);
        myCoroutine = false;
    }
}
