using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour {
    public Rigidbody2D rigidBody;
    
	// Use this for initialization
	void Start () {
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    
    private void OnTriggerEnter2D(Collider2D collision) {
        PlayerControlScript player = collision.GetComponent<PlayerControlScript>();
        if (player != null) {
            player.PassedGate();
        }
    }

    public void CreateAsNew() {
        this.transform.position  = new Vector3(25, 0, 0);
    }

    public void StartEnd() {
        StartCoroutine(endSequence());
    }

    IEnumerator endSequence() {
        rigidBody.velocity = new Vector3(-7, 0, 0);
        yield return new WaitForSeconds(3f);
        rigidBody.velocity = new Vector3(0, 0, 0);
        FindObjectOfType<PlayerControlScript>().EndGame();
    }
    
    public void RunVictory() {
        StartCoroutine(victorySequence());
    }

    IEnumerator victorySequence() {
        rigidBody.velocity = new Vector3(-15, 0, 0);
        yield return new WaitForSeconds(20f);
        rigidBody.velocity = new Vector3(0, 0, 0);
        this.gameObject.SetActive(false);
    }
}
