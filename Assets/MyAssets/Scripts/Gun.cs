using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public GameObject bullet;
    public Transform barrel;
    public int bulletAmount;

    float bulletSpeed = 18f;

    public AudioSource audioSource;
    public AudioClip audioClip;

    public void Fire() {
        StartCoroutine(FiringSequence());
    }

    private IEnumerator FiringSequence() {
        for(int i = 0; i < bulletAmount; i++) {
            GameObject spawnedBullet = Instantiate(bullet, barrel.position, Quaternion.identity);
            spawnedBullet.transform.rotation = Quaternion.LookRotation(barrel.forward);
            spawnedBullet.GetComponent<Rigidbody>().AddForce(barrel.forward * bulletSpeed, ForceMode.Impulse);
            Destroy(spawnedBullet, 3);
            audioSource.PlayOneShot(audioSource.clip);

            yield return new WaitForSeconds(0.1f);
        }
    }
}
