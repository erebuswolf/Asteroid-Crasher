using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidSpawner : MonoBehaviour {

    public int StartingAsteroidCount;
    List<GameObject> asteroids;
    public GameObject Asteroid;
    int createdAsteroidCount;
    public bool Loss;
    public int MaxAsteroidsSpawned;
    public float AsteroidSpawnRate = .3f;


    public int StartingMineCount;
    List<GameObject> mines;
    public GameObject Mine;
    int createdMineCount;
    public int MaxMinesSpawned;
    public float MineSpawnRate = 1f;

    // Use this for initialization
    void Start () {
        asteroids = new List<GameObject>(StartingAsteroidCount);
        for (int i = 0; i < StartingAsteroidCount; i++) {
            GameObject asteroid = Instantiate(Asteroid);
            asteroid.name = "asteroid " + i;
            asteroid.GetComponent<Asteroid>().SetSpawner(this);
            asteroid.SetActive(false);
            asteroids.Add(asteroid);
            asteroid.transform.parent = this.transform;
        }
        StartCoroutine(AsteroidSpawn());
        
        mines = new List<GameObject>(StartingMineCount);
        for (int i = 0; i < StartingMineCount; i++) {
            GameObject mine = Instantiate(Mine);
            mine.name = "mine " + i;
            mine.GetComponent<Mine>().SetSpawner(this);
            mine.SetActive(false);
            mines.Add(mine);
            mine.transform.parent = this.transform;
        }
        StartCoroutine(MineSpawn());
    }

    IEnumerator MineSpawn() {
        while (!Loss) {
            if (createdMineCount < MaxMinesSpawned) {
                spawnMine();
            }
            yield return new WaitForSeconds(MineSpawnRate);
        }
    }

    IEnumerator AsteroidSpawn() {
        while(!Loss) {
            if (createdAsteroidCount < MaxAsteroidsSpawned) {
                spawnAsteroid();
            }
            yield return new WaitForSeconds(AsteroidSpawnRate);
        }
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
