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

    bool endGame;

    bool endRoutine;

    bool passedGate;
    
    public GameManager manager;

    public int AsteroidCount() {
        return GetAsteroidCount(this.gameObject);
    }

    public void PassedGate() {
        passedGate = true;
    }

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

    // Function to remove all asteroids from the player.
    void ShedAsteroids() {
        foreach(Transform child in gameObject.transform) {
            Asteroid asteroid = child.gameObject.GetComponent<Asteroid>();
            if (asteroid != null) {
                asteroid.transform.position = new Vector3(-50, 0, 0);
                asteroid.BlowUp();
            }
        }
    }

    public void resetToNextLevel() {
        endGame = false;
        ShedAsteroids();
        StartCoroutine(resetLevelRoutine());
    } 

    IEnumerator resetLevelRoutine() {
        endRoutine = true;
        Vector3 curPos = this.transform.position;
        curPos.x = 25;
        this.transform.position = curPos;

        RigidBody.velocity = new Vector2(-10, 0);
        while(this.transform.position.x > -8.3f) {
            yield return new WaitForSeconds(0.01f);
        }
        Vector3 pos = this.transform.position;
        pos.x = -8.3f;
        this.transform.position = pos;
        
        manager.StartNextLevel();

        endRoutine = false;
    }

	// Update is called once per frame
	void Update () {
        if (!manager.GameStarted() || endRoutine) {
            return;
        }
        asteroidCount = GetAsteroidCount(this.gameObject);
        handleMovement();
        handleRotation();
    }

    public bool CheckVictory() {
        return (transform.position.x > 21 && passedGate);
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

        float moveVel = AngularVel * (1f - Mathf.Sqrt( Mathf.Lerp(0, .9f, Mathf.Clamp01(asteroidCount / 20f))));

        if (rotateRight && !rotateLeft) {
            RigidBody.angularVelocity = -moveVel;
        } else if (rotateLeft && !rotateRight) {
            RigidBody.angularVelocity = moveVel;
        } else {
            RigidBody.angularVelocity = 0;
        }

    }

    public void EndGame() {
        endGame = true;
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

        float xVel = endGame? 7 : 0;

        if (moveUp && !moveDown) {
            RigidBody.velocity = new Vector3(xVel, moveVel, 0);
        } else if (moveDown && !moveUp) {
            RigidBody.velocity = new Vector3(xVel, -moveVel);
        } else {
            RigidBody.velocity = new Vector3(xVel, 0);
        }

        if (Mathf.Abs(this.transform.position.y) > Bound) {
            RigidBody.velocity = new Vector3(xVel, 0);
            Vector3 pos = this.transform.position;
            pos.y = Mathf.Sign(pos.y) * Bound;
            this.transform.position = pos;
        }
    }

    public void BlowUp() {
        Debug.LogWarningFormat("game lost!");
    }
}
