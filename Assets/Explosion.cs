using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnEnable() {
        StartCoroutine(BoomTimeout());
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        AsteroidPhysics phys = collision.gameObject.GetComponent<AsteroidPhysics>();
        if (phys != null) {
            phys.BlowUp();
            return;
        }
        VisualAsteroid vis = collision.gameObject.GetComponent<VisualAsteroid>();
        if (vis != null) {
            vis.BlowUp();
            return;
        }
        Asteroid Asteroid = collision.gameObject.GetComponent<Asteroid>();
        if (Asteroid != null) {
            Asteroid.BlowUp();
            return;
        }
        PlayerControlScript player = collision.gameObject.GetComponent<PlayerControlScript>();
        if (player != null) {
            player.BlowUp();
            return;
        }
        Mine mine = collision.gameObject.GetComponent<Mine>();
        if (mine != null) {
            mine.BlowUp();
            return;
        }
    }

    IEnumerator BoomTimeout() {
        yield return new WaitForSeconds(.2f);
        this.gameObject.SetActive(false);
        this.GetComponentInParent<Mine>().ReturnToSpawner();
    }
}
