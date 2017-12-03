using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UIController : MonoBehaviour {
    public GameObject StartUI;
    public GameObject Level1Instructions;

    public void HideStartUI() {
        StartUI.SetActive(false);
    }
    
    public void StartLevel (int level) {
        Level1Instructions.SetActive(true);
    }

    public void HideGameUI () {
        Level1Instructions.SetActive(false);

    }

    // Use this for initialization
    void Start () {
        StartUI.SetActive(true);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
