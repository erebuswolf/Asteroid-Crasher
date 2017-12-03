using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public AsteroidSpawner Spawner;

    public PlayerControlScript Player;

    int currentLevel = 1;
    bool started = false;

    public UIController uiController;

    public bool GameStarted() {
        return started;
    }

    public bool CheckWinConditionLevel1(PlayerControlScript player) {
        return player.AsteroidCount() > 5 && player.CheckVictory();
    }

	// Use this for initialization
	void Start () {
	}

    public void StartNextLevel() {

    }

    IEnumerator PlayLevel() {
        uiController.StartLevel(1);

        yield return new WaitForSeconds(3);
        uiController.HideGameUI();

        Spawner.StartSpawning();

        yield return new WaitForSeconds(20);
        Spawner.StopAsteroids();

        yield return new WaitForSeconds(5);
        Spawner.StopMines();

        yield return new WaitForSeconds(3);
        Spawner.SpawnGate(1);

        while(!CheckWinConditionLevel1(Player)) {
            yield return new WaitForSeconds(.1f);
        }

        Spawner.TriggerVictoryOnGate();
        yield return new WaitForSeconds(1f);
        Player.resetToNextLevel();
        // Level Victory

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
