using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public GameObject bullet;
    public Transform barrel;
    float bulletSpeed = 18f;

    public AudioSource audioSource;
    public AudioClip audioClip;

    // En el tuturial de Valem hay mas cosas ricas en este script, como lanzar una bala
    public void Fire() {
        GameObject spawnedBullet = Instantiate(bullet, barrel.position, Quaternion.identity);
        spawnedBullet.transform.rotation = Quaternion.LookRotation(barrel.forward);
        spawnedBullet.GetComponent<Rigidbody>().AddForce(barrel.forward * bulletSpeed, ForceMode.Impulse);

        Destroy(spawnedBullet, 5);

        audioSource.PlayOneShot(audioSource.clip);
    }
}
