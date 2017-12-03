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

    // Use this for initialization
    void Start () {
		
	}

    float GetMass() {
        return 1;
    }
	
	// Update is called once per frame
	void Update () {
        handleMovement();
        handleRotation();
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

        if (rotateRight && !rotateLeft) {
            RigidBody.angularVelocity = -AngularVel;
        } else if (rotateLeft && !rotateRight) {
            RigidBody.angularVelocity = AngularVel;
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

        if (moveUp && !moveDown) {
            RigidBody.velocity = new Vector3(0, UpVel, 0);
        } else if (moveDown && !moveUp) {
            RigidBody.velocity = new Vector3(0, -UpVel);
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
