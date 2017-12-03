using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidSpawner : MonoBehaviour {

    public GameManager manager;

    public int StartingAsteroidCount;
    List<GameObject> asteroids;
    public GameObject AsteroidPrefab;
    int createdAsteroidCount;
    public int MaxAsteroidsSpawned;
    public float AsteroidSpawnRate = .3f;
    
    public int StartingMineCount;
    List<GameObject> mines;
    public GameObject Mine;
    int createdMineCount;
    public int MaxMinesSpawned;
    public float MineSpawnRate = 1f;
    
    bool stopAsteroids;
    bool stopMines;

    public GameObject GatePrefab;


    public GameObject Gamegate;

    public void StopMines() {
        stopMines = true;
    }

    public void StopAsteroids() {
        stopAsteroids = true;
    }

    public void SpawnGate(float scale) {
        Gamegate.GetComponent<Gate>().CreateAsNew();
        Gamegate.SetActive(true);
        Gamegate.GetComponent<Gate>().StartEnd();
        Vector3 oldScale = Gamegate.transform.localScale;
        Gamegate.transform.localScale = scale * oldScale;
    }

    public void TriggerVictoryOnGate() {
        if (Gamegate.activeInHierarchy) {
            Gamegate.GetComponent<Gate>().RunVictory();
        }
    }

    // Use this for initialization
    void Start () {
        Gamegate = Instantiate(GatePrefab);
        Gamegate.SetActive(false);

        asteroids = new List<GameObject>(StartingAsteroidCount);
        for (int i = 0; i < StartingAsteroidCount; i++) {
            GameObject asteroid = Instantiate(AsteroidPrefab);
            asteroid.name = "asteroid " + i;
            asteroid.GetComponent<Asteroid>().SetSpawner(this);
            asteroid.SetActive(false);
            asteroids.Add(asteroid);
            asteroid.transform.parent = this.transform;
        }
        
        mines = new List<GameObject>(StartingMineCount);
        for (int i = 0; i < StartingMineCount; i++) {
            GameObject mine = Instantiate(Mine);
            mine.name = "mine " + i;
            mine.GetComponent<Mine>().SetSpawner(this);
            mine.SetActive(false);
            mines.Add(mine);
            mine.transform.parent = this.transform;
        }
    }

    public void StartSpawning() {
        stopMines = false;
        stopAsteroids = false;
        StartCoroutine(AsteroidSpawn());
        StartCoroutine(MineSpawn());
    }

    IEnumerator MineSpawn() {
        while (!stopMines) {
            if (createdMineCount < MaxMinesSpawned) {
                spawnMine();
            }
            yield return new WaitForSeconds(MineSpawnRate);
        }
    }

    IEnumerator AsteroidSpawn() {
        while(!stopAsteroids) {
            if (createdAsteroidCount < MaxAsteroidsSpawned) {
                spawnAsteroid();
            }
            yield return new WaitForSeconds(AsteroidSpawnRate);
        }
    }

    Asteroid.TYPE getType() {
        int level = manager.GetLevel();
        if (level < 2) {
            return Asteroid.TYPE.NORMAL;
        } else if (level == 2) {
            if (Random.Range(0f, 1f) < .2) {
                return Asteroid.TYPE.IRON;
            }
        } else if (level == 3) {
            if (Random.Range(0f, 1f) < .1) {
                return Asteroid.TYPE.GOLD;
            }
        } else if (level == 4) {
            float roll = Random.Range(0f, 1f);
            if (roll < .2) {
                return Asteroid.TYPE.IRON;
            }else if (roll< .5) {
                return Asteroid.TYPE.ICE;
            }
        } else if (level == 5) {
            float roll = Random.Range(0f, 1f);
            if (roll < .1) {
                return Asteroid.TYPE.IRON;
            } else if (roll < .2) {
                return Asteroid.TYPE.GOLD;
            } else if (roll < .4) {
                return Asteroid.TYPE.ICE;
            }
        } else if (level >= 6) {
            float roll = Random.Range(0f, 1f);
            if (roll < .25) {
                return Asteroid.TYPE.IRON;
            } else if (roll < .5) {
                return Asteroid.TYPE.GOLD;
            } else if (roll < .75) {
                return Asteroid.TYPE.ICE;
            }
        }
        return Asteroid.TYPE.NORMAL;
    }

    void spawnMine() {
        if (mines.Count > 0) {
            createdMineCount++;
            GameObject mineToSpawn = mines[0];
            mines.RemoveAt(0);
            mineToSpawn.SetActive(true);
            mineToSpawn.GetComponent<Mine>().CreateAsNew();
        }
    }

    void spawnAsteroid() {
        if (asteroids.Count > 0) {
            createdAsteroidCount++;
            GameObject asteroidToSpawn = asteroids[0];
            asteroids.RemoveAt(0);
            asteroidToSpawn.SetActive(true);
            asteroidToSpawn.GetComponent<Asteroid>().CreateAsNew();
            asteroidToSpawn.GetComponent<Asteroid>().SetType(getType());
        }
    }

    public void Reclaim(Asteroid asteroid) {
        asteroid.CreateAsNew();
        asteroid.transform.parent = null;
        asteroid.gameObject.SetActive(false);
        asteroids.Add(asteroid.gameObject);
        createdAsteroidCount--;
    }

    public void Reclaim(Mine mine) {
        mine.CreateAsNew();
        mine.gameObject.SetActive(false);
        mines.Add(mine.gameObject);
        createdMineCount--;
    }

    // Update is called once per frame
    void Update () {
		
	}
}
