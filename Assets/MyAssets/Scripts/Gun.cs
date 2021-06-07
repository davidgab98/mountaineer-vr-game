using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public GameObject bullet;
    public Transform barrel;

    [SerializeField]
    private int bulletAmount = 1;
    [SerializeField]
    private float bulletSpeed = 18f;

    [SerializeField]
    private GameObject smokeAfterShootParticles;
    private AudioSource audioSource;
    
    public void Fire() {
        audioSource = GetComponent<AudioSource>();
        StartCoroutine(FiringSequence());
    }

    private IEnumerator FiringSequence() {
        for(int i = 0; i < bulletAmount; i++) {
            GetComponent<Animation>().CrossFade("flareGunShot");
            audioSource.Play();

            GameObject spawnedBullet = Instantiate(bullet, barrel.position, Quaternion.identity);
            spawnedBullet.transform.rotation = Quaternion.LookRotation(barrel.forward);
            spawnedBullet.GetComponent<Rigidbody>().AddForce(barrel.forward * bulletSpeed, ForceMode.Impulse);

            GameObject spawnedSmoke = Instantiate(smokeAfterShootParticles, barrel.position, Quaternion.identity);

            yield return new WaitForSeconds(0.1f);
        }
    }
}
