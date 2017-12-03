using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public AsteroidSpawner Spawner;

    public List<float> GateScale;

    public PlayerControlScript Player;

    int currentLevel = 0;
    bool started = false;

    bool failed = false;

    bool resetOnFail = false;

    public UIController uiController;

    public bool GameStarted() {
        return started;
    }

    public int GetLevel() {
        return currentLevel;
    }

    public bool CheckWinCondition(int level, PlayerControlScript player) {
        switch(level) {
            case 0:
                return CheckWinConditionLevel1(player);
            case 1:
                return CheckWinConditionLevel2(player);
            case 2:
                return CheckWinConditionLevel3(player);
            case 3:
                return CheckWinConditionLevel4(player);
            case 4:
                return CheckWinConditionLevel5(player);
            default:
                return false;
        }
    }

    public bool CheckWinConditionLevel2(PlayerControlScript player) {
        if (player.CheckVictory() && player.AsteroidCount() < 10) {
            FailedLevel();
        }
        return player.AsteroidCount() >= 5 && player.CheckVictory();
    }
    public bool CheckWinConditionLevel1(PlayerControlScript player) {
        if (player.CheckVictory() && player.AsteroidCount() < 5) {
            FailedLevel();
        }
        return player.AsteroidCount() >= 5 && player.CheckVictory();
    }

    public bool CheckWinConditionLevel3(PlayerControlScript player) {
        if (player.CheckVictory() && player.AsteroidCount(Asteroid.TYPE.IRON) < 5) {
            FailedLevel();
        }
        return player.AsteroidCount(Asteroid.TYPE.IRON) >= 5 && player.CheckVictory();
    }

    public bool CheckWinConditionLevel4(PlayerControlScript player) {
        if (player.CheckVictory() && player.AsteroidCount(Asteroid.TYPE.GOLD) < 2) {
            FailedLevel();
        }
        return player.AsteroidCount(Asteroid.TYPE.GOLD) >= 2 && player.CheckVictory();
    }
    public bool CheckWinConditionLevel5(PlayerControlScript player) {
        if (player.CheckVictory() && (player.AsteroidCount(Asteroid.TYPE.ICE) < 5
            || player.AsteroidCount(Asteroid.TYPE.IRON) < 3)) {
            FailedLevel();
        }
        return player.AsteroidCount(Asteroid.TYPE.ICE) >= 5 && 
            player.AsteroidCount(Asteroid.TYPE.IRON) >= 3 
            && player.CheckVictory();
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
            if (failed) {
                uiController.FailedLevel();
                yield return new WaitForSeconds(3);
                uiController.ClearFail();
            }
            uiController.StartLevel(currentLevel);
            yield return new WaitForSeconds(3);
            uiController.HideGameUI(currentLevel);
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

            Spawner.SpawnGate(GateScale[currentLevel]);

            i = 0;
            while (!CheckWinCondition(currentLevel, Player) && !failed) {
                yield return new WaitForSeconds(.1f);
                i++;
            }

        } while (failed);

        Spawner.TriggerVictoryOnGate();
        uiController.Success();
        yield return new WaitForSeconds(3f);
        Player.resetToNextLevel();
        // Level Victory
        currentLevel++;
        uiController.HideSuccess();

        if (currentLevel< GateScale.Count) {
            StartCoroutine(PlayLevel());
        } else {
            // roll credits
        }
    }

    public void SetLevel(int i) {
        currentLevel = i;
        started = true;
        uiController.HideStartUI();
        StartCoroutine(PlayLevel());
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
