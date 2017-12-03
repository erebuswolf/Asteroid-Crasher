using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarSlide : MonoBehaviour {
    public float speed;
    public Rigidbody2D RigidBody;
    public float resetPoint;
	// Use this for initialization
	void Start () {
        RigidBody.velocity = new Vector2(-speed, 0);
    }
	
	// Update is called once per frame
	void Update () {
		if (this.transform.position.x < -resetPoint) {
            Vector3 pos = this.transform.position;
            pos.x += resetPoint * 2;
            this.transform.position = pos;
        }
	}
}
