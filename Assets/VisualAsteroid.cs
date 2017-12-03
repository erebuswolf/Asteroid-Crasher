using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualAsteroid : MonoBehaviour {
    public Asteroid asteroid;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void BlowUp() {
        asteroid.BlowUp();
    }
}
