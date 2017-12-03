using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UIController : MonoBehaviour {
    public GameObject StartUI;
    public List<GameObject> LevelInstructions;
    public GameObject LevelFail;
    public GameObject Victory;

    public GameManager manager;

    public void HideStartUI() {
        StartUI.SetActive(false);
    }
    
    public void Success() {
        Victory.SetActive(true);

    }

    public void HideSuccess() {
        Victory.SetActive(false);

    }

    public void FailedLevel() {
        LevelFail.SetActive(true);
    }

    public void ClearFail() {
        LevelFail.SetActive(false);
    }
    
    public void SkipToLevel(int i) {
        manager.SetLevel(i);
    }

    public void StartLevel (int level) {
        LevelInstructions[level].SetActive(true);
    }

    public void HideGameUI (int level) {
        LevelInstructions[level].SetActive(false);
    }

    // Use this for initialization
    void Start () {
        StartUI.SetActive(true);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
