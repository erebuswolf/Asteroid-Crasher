﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour {
    public Rigidbody2D rigidBody;
    AsteroidSpawner spawner;

    public GameObject PhysicsObject;
    public GameObject VisObject;

    public float startVel;

    bool attached = false;

    public void SetSpawner(AsteroidSpawner spawner) {
        this.spawner = spawner;
    }

	// Use this for initialization
	void Start () {
        rigidBody.velocity = getStartVel();
        rigidBody.angularVelocity = getStartAngularVel();
    }
	
    Vector3 getStartVel() {
        return new Vector3(-startVel, 0, 0);
    }

    float getStartAngularVel() {
        return 100;
    }

    public void PhysicsTrigger(Collider2D collision) {
        OnTriggerEnter2D(collision);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.layer == 8 && !attached) {
            PhysicsObject.SetActive(false);

            VisObject.SetActive(true);
            VisObject.transform.position = PhysicsObject.transform.position;
            VisObject.transform.rotation = PhysicsObject.transform.rotation;

            gameObject.layer = 8;
            attached = true;
            ClearVel();
            this.transform.parent = collision.gameObject.transform;
        }
    }

    public void CreateAsNew() {
        float yPos = Random.Range(-10f, 10f);
        Vector3 pos = new Vector3(20, yPos, 0);
        this.transform.parent = null;
        this.transform.position = pos;
        PhysicsObject.SetActive(true);
        PhysicsObject.transform.localPosition = Vector3.zero;
        PhysicsObject.transform.localRotation = Quaternion.identity;
        VisObject.SetActive(false);
        rigidBody.velocity = getStartVel();
        rigidBody.angularVelocity = getStartAngularVel();
        this.GetComponentInChildren<SpriteRenderer>().color = Color.white;
    }

    void ClearVel() {
        rigidBody.velocity = Vector3.zero;
        rigidBody.angularVelocity = 0;
    }
    
    // Update is called once per frame
    void Update () {
		if ( PhysicsObject.transform.position.x < -20 && !attached) {
            this.spawner.Reclaim(this);
        }
    }

    public void BlownParent() {
        this.VisObject.layer = 9;
        this.GetComponentInChildren<SpriteRenderer>().color = Color.black;
    }

    public void BlowUp() {

        foreach (Transform child in transform) {
            if (transform.gameObject == PhysicsObject || transform.gameObject == VisObject) {
                continue;
            }
            Asteroid asteroid = child.GetComponent<Asteroid>();
            if (asteroid != null) {
                Debug.LogWarning("found asteroid");
                asteroid.BlownParent();
                child.SetParent(null);
            }
        }
        attached = false;
        this.gameObject.SetActive(false);
    }
}