using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidPhysics : MonoBehaviour {
    public Asteroid asteroid;
    public bool Destroyed = false;
    Rigidbody2D rigidBody;    
    // Use this for initialization
    void Start() {
        rigidBody = this.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update() {
        rigidBody.velocity = asteroid.getStartVel();
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (Destroyed) {
            return;
        }
        asteroid.PhysicsTrigger(collision);
    }

    public void BlowUp() {
        if (Destroyed) {
            return;
        }
        asteroid.SetVisToPhysPos();
        asteroid.BlowUp();
    }
}
