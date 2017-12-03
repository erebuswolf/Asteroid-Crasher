using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public AsteroidSpawner Spawner;

    public PlayerControlScript Player;

    int currentLevel = 1;
    bool started = false;

    bool failed = false;

    bool resetOnFail = false;

    public UIController uiController;

    public bool GameStarted() {
        return started;
    }

    public bool CheckWinConditionLevel1(PlayerControlScript player) {
        if (player.CheckVictory() && player.AsteroidCount() < 5) {
            FailedLevel();
        }
        return player.AsteroidCount() >= 5 && player.CheckVictory();
    }

	// Use this for initialization
	void Start () {
	}

    public void StartNextLevel() {

    }

    public void FailedLevel() {
        failed = true;
    }

    IEnumerator PlayLevel() {
        do {
            uiController.StartLevel(1);

            yield return new WaitForSeconds(3);
            uiController.HideGameUI();
            if (failed) {
                yield return new WaitForSeconds(2f);
            }

            failed = false;
            resetOnFail = false;

            Spawner.StartSpawning();
            int i = 0;
            while (i < 40) {
                yield return new WaitForSeconds(.5f);
                i++;
                if (failed) {
                    break;
                }
            }
            if (failed) {
                continue;
            }
            Spawner.StopAsteroids();

            i = 0;
            while (i < 10) {
                yield return new WaitForSeconds(.5f);
                i++;
                if (failed) {
                    break;
                }
            }
            if (failed) {
                continue;
            }
            Spawner.StopMines();
            i = 0;
            while (i < 6) {
                yield return new WaitForSeconds(.5f);
                if (failed) {
                    break;
                }
                i++;
            }
            if (failed) {
                continue;
            }

            Spawner.SpawnGate(1);

            i = 0;
            while (!CheckWinConditionLevel1(Player) && !failed) {
                yield return new WaitForSeconds(.1f);
                i++;
            }

        } while (failed);

        Spawner.TriggerVictoryOnGate();
        yield return new WaitForSeconds(3f);
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

        if (failed && !resetOnFail) {
            resetOnFail = true;
            StartCoroutine(resetGameOnFail());
        }
    }

    IEnumerator resetGameOnFail() {
        Spawner.TriggerVictoryOnGate();
        Spawner.StopMines();
        Spawner.StopAsteroids();
        yield return new WaitForSeconds(3); 
        Player.resetToNextLevel();
    }
}
