using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine : MonoBehaviour {
    public SpriteRenderer MineSprite;
    bool blown = false;
    public Rigidbody2D rigidBody;
    public ParticleSystem ps;

    public AudioSource boomSound;

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
        return Random.Range(-100f, 100f);
    }

    // Update is called once per frame
    void Update () {
        if (this.transform.position.x < -20) {
            this.spawner.Reclaim(this);
        }
    }

    public void BlowUp() {
        if (!blown) {
            boomSound.pitch = Random.Range(.9f, 1.1f);
            boomSound.Play();
            blown = true;
            Explosion.SetActive(true);
            ps.Play();
            ClearVel();
            MineSprite.gameObject.SetActive(false);
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
        MineSprite.gameObject.SetActive(true);
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
