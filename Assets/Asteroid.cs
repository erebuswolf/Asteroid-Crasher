using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour {
    public enum TYPE {NORMAL, IRON, CRYSTAL, ICE, PLATINUM, GOLD }
    public TYPE type;

    public Rigidbody2D rigidBody;
    public AsteroidSpawner spawner;

    public GameObject PhysicsObject;
    public GameObject VisObject;

    public Color DisabledColor;

    public float startVel;

    bool attached = false;

    bool blownUp;

    public void SetType(TYPE newType) {
        type = newType;
        Sprite sprite = Resources.Load<Sprite>("asteroid");
        switch (type) {
            case TYPE.IRON:
                sprite = Resources.Load<Sprite>("asteroidIron");
                break;
            case TYPE.CRYSTAL:
                break;
            case TYPE.ICE:
                sprite = Resources.Load<Sprite>("asteroidIce");
                break;
            case TYPE.PLATINUM:
                break;
            case TYPE.GOLD:
                sprite = Resources.Load<Sprite>("asteroidGold");
                break;
            case TYPE.NORMAL:
                break;
        }

        PhysicsObject.GetComponent<SpriteRenderer>().sprite = sprite;
        VisObject.GetComponent<SpriteRenderer>().sprite = sprite;
    }


    public void SetSpawner(AsteroidSpawner spawner) {
        this.spawner = spawner;
    }

	// Use this for initialization
	void Start () {
        rigidBody.velocity = getStartVel();
        rigidBody.angularVelocity = getStartAngularVel();
    }
	
    public Vector3 getStartVel() {
        return new Vector3(-startVel, 0, 0);
    }

    float getStartAngularVel() {
        return Random.Range(-100f,100f);
    }

    public void PhysicsTrigger(Collider2D collision) {
        OnTriggerEnter2D(collision);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (blownUp) {
            return;
        }
        if (collision.gameObject.layer == 8 && !attached) {
            PhysicsObject.SetActive(false);

            VisObject.SetActive(true);
            SetVisToPhysPos();

            VisObject.layer = 8;
            attached = true;
            ClearVel();
            this.transform.parent = collision.gameObject.transform;
        }
    }

    public void PromoteChildAsteroid() {
        foreach(Transform child in PhysicsObject.transform) {
            Asteroid asteroid = child.gameObject.GetComponent<Asteroid>();
            asteroid.SwapVisForPhys();
            asteroid.transform.parent = null;
        }

        foreach (Transform child in VisObject.transform) {
            Asteroid asteroid = child.gameObject.GetComponent<Asteroid>();
            asteroid.SwapVisForPhys();
            asteroid.transform.parent = null;
        }
    }

    public void SwapVisForPhys() {
        PhysicsObject.transform.position = VisObject.transform.position;
        PhysicsObject.transform.rotation = VisObject.transform.rotation;
        foreach (Transform child in VisObject.transform) {
            child.gameObject.transform.parent = PhysicsObject.transform;
        }
        PhysicsObject.SetActive(true);
        VisObject.SetActive(false);
        rigidBody.velocity = getStartVel();
    }

    public void CreateAsNew() {
        PromoteChildAsteroid();
        float yPos = Random.Range(-10f, 10f);
        Vector3 pos = new Vector3(20, yPos, 0);
        this.transform.parent = null;
        this.transform.position = pos;
        PhysicsObject.SetActive(true);
        PhysicsObject.transform.localPosition = Vector3.zero;
        PhysicsObject.transform.localRotation = Quaternion.identity;

        VisObject.transform.localPosition = Vector3.zero;
        VisObject.transform.localRotation = Quaternion.identity;

        PhysicsObject.layer = 9;
        VisObject.layer = 9;
        this.gameObject.layer = 9;

        VisObject.SetActive(false);
        rigidBody.velocity = getStartVel();
        rigidBody.angularVelocity = getStartAngularVel();
        PhysicsObject.GetComponent<SpriteRenderer>().color = Color.white;
        VisObject.GetComponent<SpriteRenderer>().color = Color.white;
        VisObject.GetComponent<SpriteRenderer>().sortingOrder = 0;
        PhysicsObject.GetComponent<SpriteRenderer>().sortingOrder = 0;
        PhysicsObject.GetComponent<AsteroidPhysics>().Destroyed = false;
        blownUp = false;
    }

    void ClearVel() {
        rigidBody.velocity = Vector3.zero;
        rigidBody.angularVelocity = 0;
    }
    
    // Update is called once per frame
    void Update () {
        if (PhysicsObject.activeSelf && !VisObject.activeSelf) {
            VisObject.transform.position = PhysicsObject.transform.position;
            VisObject.transform.rotation = PhysicsObject.transform.rotation;
        } else if (!PhysicsObject.activeSelf && VisObject.activeSelf) {
            PhysicsObject.transform.position = VisObject.transform.position;
            PhysicsObject.transform.rotation = VisObject.transform.rotation;
        } else if (PhysicsObject.activeSelf && VisObject.activeSelf) {
            Debug.LogWarning("WHY THE FUCK ARE YOU BOTH AWAKE??");
        }
		if ( PhysicsObject.transform.position.x < -40 && !attached) {
            this.spawner.Reclaim(this);
        }
    }

    public void BlownRoot() {
        PhysicsObject.GetComponent<SpriteRenderer>().color = DisabledColor;
        VisObject.GetComponent<SpriteRenderer>().sortingOrder = -1;
        VisObject.GetComponent<SpriteRenderer>().color = DisabledColor;
        PhysicsObject.GetComponent<SpriteRenderer>().sortingOrder = -1;
        PhysicsObject.transform.position = VisObject.transform.position;
        PhysicsObject.transform.rotation = VisObject.transform.rotation;
        PhysicsObject.SetActive(true);
        PhysicsObject.GetComponent<AsteroidPhysics>().Destroyed = true;
        VisObject.SetActive(false);

        foreach(Transform child in VisObject.transform) {
            child.gameObject.transform.parent = PhysicsObject.transform;
        }

        rigidBody.velocity = getStartVel();
        blownUp = true;
    }

    public static void blownParent(Asteroid asteroid) {
        asteroid.VisObject.layer = 9;
        asteroid.VisObject.GetComponent<SpriteRenderer>().color = asteroid.DisabledColor;
        asteroid.VisObject.GetComponent<SpriteRenderer>().sortingOrder = -1;
        asteroid.PhysicsObject.GetComponent<SpriteRenderer>().color = asteroid.DisabledColor;
        asteroid.PhysicsObject.GetComponent<SpriteRenderer>().sortingOrder = -1;
        asteroid.blownUp = true;
    }

    static void RecursiveSearch(GameObject obj, bool found) {
        Asteroid asteroid = obj.GetComponent<Asteroid>();
        if (asteroid != null) {
            asteroid.attached = false;
            asteroid.blownUp = true;
            // only set parent false on first found and only turn on physics for first found.!!!!!
            if (!found) {
                asteroid.transform.SetParent(null);
                asteroid.attached = false;
                asteroid.BlownRoot();
            } else {
                blownParent(asteroid);
            }
            found = true;
        }

        foreach (Transform child in obj.transform) {
            RecursiveSearch(child.gameObject, found);
        }
    }

    public void SetVisToPhysPos() {
        VisObject.transform.position = PhysicsObject.transform.position;
        VisObject.transform.rotation = PhysicsObject.transform.rotation;
    }

    public void BlowUp() {
        /*foreach (Transform child in transform) {
            if (transform.gameObject == PhysicsObject || transform.gameObject == VisObject) {
                continue;
            }
            Asteroid asteroid = child.GetComponent<Asteroid>();
            if (asteroid != null) {
                Debug.LogWarning("found asteroid");
                asteroid.BlownParent();
                child.SetParent(null);
                asteroid.attached = false;
            }
        }*/
        RecursiveSearch(this.gameObject, false);
        attached = false;
        //this.gameObject.SetActive(false);
    }
}
