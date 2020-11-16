using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public GameObject bullet;
    float bulletSpeed = 200f;

    public AudioSource audioSource;
    public AudioClip audioClip;

    // En el tuturial de Valem hay mas cosas ricas en este script, como lanzar una bala
    public void Fire() {
        GameObject spawnedBullet = Instantiate(bullet, transform.position, Quaternion.identity);
        spawnedBullet.GetComponent<Rigidbody>().AddForce(transform.forward * bulletSpeed);

        audioSource.PlayOneShot(audioSource.clip);
    }
}
