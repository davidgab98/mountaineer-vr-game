using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoPlaceStone : MonoBehaviour
{
    private Rigidbody rb;
    private MeshCollider mesh;

    bool notCollidingWithTerrain;

    private void Awake() {
        rb = GetComponent<Rigidbody>();
        mesh = GetComponent<MeshCollider>();
    }

    private void Update() {
        if(!notCollidingWithTerrain) {
            transform.position = new Vector3(transform.position.x - 0.04f, transform.position.y, transform.position.z);
        }
    }

    private void OnTriggerEnter(Collider other) {
        if(other.CompareTag("Terrain")) {
            mesh.isTrigger = false;
            //mesh.convex = false;
            rb.constraints = RigidbodyConstraints.None;
            rb.isKinematic = true;
            notCollidingWithTerrain = true;
            Destroy(this);
        }
    }
}
