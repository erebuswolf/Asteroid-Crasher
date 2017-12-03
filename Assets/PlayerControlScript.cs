using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControlScript : MonoBehaviour {

    public Rigidbody2D RigidBody;
    public List<KeyCode> UpCodes;
    public List<KeyCode> DownCodes;

    public List<KeyCode> RightCodes;
    public List<KeyCode> LeftCodes;

    public float UpVel;
    public float VelMassRatio;

    public float AngularVel;

    public float Bound;

    int asteroidCount;
    // Use this for initialization
    void Start () {
		
	}

    float GetMass() {
        return 1;
    }
	
    static int GetAsteroidCount(GameObject obj) {
        int count = 0;
        foreach (Transform child in obj.transform) {
            Asteroid asteroid = child.gameObject.GetComponent<Asteroid>();
            if (asteroid != null) {
                count += GetAsteroidCount(asteroid.VisObject);
                count++;
            }
        }
        return count;
    }

	// Update is called once per frame
	void Update () {
        asteroidCount = GetAsteroidCount(this.gameObject);
        handleMovement();
        handleRotation();
        //
    }

    void handleRotation() {
        bool rotateRight = false;
        bool rotateLeft = false;
        foreach (KeyCode k in RightCodes) {
            if (Input.GetKey(k)) {
                rotateRight = true;
            }
        }

        foreach (KeyCode k in LeftCodes) {
            if (Input.GetKey(k)) {
                rotateLeft = true;
            }
        }

        float moveVel = AngularVel * (1f - Mathf.Lerp(0, .9f, Mathf.Clamp01(asteroidCount / 20f)));

        if (rotateRight && !rotateLeft) {
            RigidBody.angularVelocity = -moveVel;
        } else if (rotateLeft && !rotateRight) {
            RigidBody.angularVelocity = moveVel;
        } else {
            RigidBody.angularVelocity = 0;
        }

    }

    void handleMovement() {
        bool moveUp = false;
        bool moveDown = false;
        foreach (KeyCode k in UpCodes) {
            if (Input.GetKey(k)) {
                moveUp = true;
            }
        }

        foreach (KeyCode k in DownCodes) {
            if (Input.GetKey(k)) {
                moveDown = true;
            }
        }

        float moveVel = UpVel * (1f - Mathf.Lerp(0,.8f, Mathf.Clamp01(asteroidCount/20f)));

        if (moveUp && !moveDown) {
            RigidBody.velocity = new Vector3(0, moveVel, 0);
        } else if (moveDown && !moveUp) {
            RigidBody.velocity = new Vector3(0, -moveVel);
        } else {
            RigidBody.velocity = new Vector3(0, 0);
        }

        if (Mathf.Abs(this.transform.position.y) > Bound) {
            RigidBody.velocity = new Vector3(0, 0);
            Vector3 pos = this.transform.position;
            pos.y = Mathf.Sign(pos.y) * Bound;
            this.transform.position = pos;
        }
    }

    public void BlowUp() {
        Debug.LogWarningFormat("game lost!");
    }
}
