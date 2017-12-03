using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public AsteroidSpawner spawner;
    int currentLevel = 1;
    bool started = false;

    public UIController uiController;

    public bool GameStarted() {
        return started;
    }

	// Use this for initialization
	void Start () {
	}

    IEnumerator PlayLevel() {
        uiController.StartLevel(1);

        yield return new WaitForSeconds(3);
        uiController.HideGameUI();

        spawner.StartSpawning();

        yield return new WaitForSeconds(20);
        spawner.StopAsteroids();

        yield return new WaitForSeconds(5);
        spawner.StopMines();

        yield return new WaitForSeconds(3);
        spawner.SpawnGate(1);
    }
	
	// Update is called once per frame
	void Update () {
        if (!started && Input.GetKey(KeyCode.Space)) {
            started = true;
            uiController.HideStartUI();
            StartCoroutine(PlayLevel());
        }
    }
}
