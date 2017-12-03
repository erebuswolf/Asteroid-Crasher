using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidPhysics : MonoBehaviour {
    public Asteroid asteroid;
    // Use this for initialization
    void Start() {
    }

    // Update is called once per frame
    void Update() {

    }

    private void OnTriggerEnter2D(Collider2D collision) {
        asteroid.PhysicsTrigger(collision);
    }

    public void BlowUp() {
        asteroid.BlowUp();
    }
}
