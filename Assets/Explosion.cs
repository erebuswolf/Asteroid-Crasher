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
        Debug.LogWarningFormat("collided! {0} {1}", collision, collision.gameObject);
        AsteroidPhysics phys = collision.gameObject.GetComponent<AsteroidPhysics>();
        if (phys != null) {
            Debug.LogWarningFormat("bowing phys");
            phys.BlowUp();
            return;
        }
        VisualAsteroid vis = collision.gameObject.GetComponent<VisualAsteroid>();
        if (vis != null) {
            Debug.LogWarningFormat("bowing viz");
            vis.BlowUp();
            return;
        }
        Asteroid Asteroid = collision.gameObject.GetComponent<Asteroid>();
        if (Asteroid != null) {
            Debug.LogWarningFormat("bowing asteroid");
            Asteroid.BlowUp();
            return;
        }
        PlayerControlScript player = collision.gameObject.GetComponent<PlayerControlScript>();
        if (player != null) {
            Debug.LogWarningFormat("bowing player");
            player.BlowUp();
            return;
        }
        Mine mine = collision.gameObject.GetComponent<Mine>();
        if (mine != null) {
            Debug.LogWarningFormat("bowing mine");
            mine.BlowUp();
            return;
        }
    }

    IEnumerator BoomTimeout() {
        yield return new WaitForSeconds(1);
        this.gameObject.SetActive(false);
        this.GetComponentInParent<Mine>().ReturnToSpawner();
    }
}
