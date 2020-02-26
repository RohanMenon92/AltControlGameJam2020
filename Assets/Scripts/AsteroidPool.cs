using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidPool : MonoBehaviour
{
    // Start is called before the first frame update
    public List<GameObject> asteroidPrefabs;
    public int numOfAsteroids;
    public float spawnTime;

    public List<Asteroid> asteroidPool;
    void Start()
    {
        for(int i = 0; i < numOfAsteroids; i++)
        {
            GameObject newAsteroid = Instantiate(asteroidPrefabs[Random.Range(0,6)], transform);
            asteroidPool.Add(newAsteroid.GetComponent<Asteroid>());
            newAsteroid.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
