using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour {
    public Rigidbody2D rigidBody;
    bool runningVictory;

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
        this.transform.localScale = new Vector3(4, 4, 4);
        this.transform.localRotation = Quaternion.identity;
        rigidBody.velocity = Vector3.zero;
        rigidBody.angularVelocity = 0;
    }

    public void StartEnd() {
        StartCoroutine(endSequence());
    }

    IEnumerator endSequence() {
        rigidBody.velocity = new Vector3(-7, 0, 0);
        int i = 0;
        while (i < 6) {
            i++;
            yield return new WaitForSeconds(.5f);
            if (runningVictory) {
                FindObjectOfType<PlayerControlScript>().Gatebreak();
                break;
            }
        }

        if (!runningVictory) {
            FindObjectOfType<PlayerControlScript>().EndGame();
            rigidBody.velocity = new Vector3(0, 0, 0);
        }
    }
    
    public void RunVictory() {
        StartCoroutine(victorySequence());
    }

    IEnumerator victorySequence() {
        yield return new WaitForSeconds(3f);
        runningVictory = true;
        rigidBody.velocity = new Vector3(-15, 0, 0);
        yield return new WaitForSeconds(20f);
        rigidBody.velocity = new Vector3(0, 0, 0);
        this.gameObject.SetActive(false);
        runningVictory = false;
    }
}
