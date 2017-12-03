using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateCollisionCheck : MonoBehaviour {

    public ParticleSystem explosion;

    bool exploding;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    IEnumerator explode() {
        exploding = true;
        explosion.Play();
        yield return new WaitForSeconds(5f);
        exploding = false;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.gameObject.layer == 8) {
            FindObjectOfType<GameManager>().FailedLevel();
            if (!exploding) {
                StartCoroutine(explode());
            }
        }
    }

}
