using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine : MonoBehaviour {

    bool blown = false;
    public Rigidbody2D rigidBody;

    public GameObject Explosion;

    public float startVel;

    public AsteroidSpawner spawner;
    
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

    // Update is called once per frame
    void Update () {
        if (this.transform.position.x < -20) {
            this.spawner.Reclaim(this);
        }
    }

    public void BlowUp() {
        if (!blown) {
            blown = true;
            Explosion.SetActive(true);
            ClearVel();
        }
    }

    public void CreateAsNew() {
        float yPos = Random.Range(-10f, 10f);
        Vector3 pos = new Vector3(20, yPos, 0);
        this.transform.position = pos;
        Explosion.SetActive(false);
        rigidBody.velocity = getStartVel();
        rigidBody.angularVelocity = getStartAngularVel();
        blown = false;
    }

    public void ReturnToSpawner() {
        spawner.Reclaim(this);
    }

    public void SetSpawner(AsteroidSpawner spawner) {
        this.spawner = spawner;
    }

    void ClearVel() {
        rigidBody.velocity = Vector3.zero;
        rigidBody.angularVelocity = 0;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.layer == 8 && !blown) {
            BlowUp();
        }
    }
}
