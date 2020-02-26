using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidPool : MonoBehaviour
{
    // Start is called before the first frame update
    public List<GameObject> asteroidPrefabs;
    public int numOfAsteroids;
    public float spawnTime;

    public float maxX;
    public float maxZ;
    public float maxInitialForce;

    public List<Asteroid> asteroidPool;
    public List<Asteroid> unusedAsteroid;
    void Start()
    {
        for(int i = 0; i < numOfAsteroids; i++)
        {
            GameObject newAsteroid = Instantiate(asteroidPrefabs[Random.Range(0, asteroidPrefabs.Count)], transform);
            asteroidPool.Add(newAsteroid.GetComponent<Asteroid>());

            // random spawn in map

            Vector3 newPosition = new Vector3(Random.Range(-maxX, maxX), 0, Random.Range(-maxZ, maxZ));
            newAsteroid.transform.position = newPosition;
            newAsteroid.GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(-maxInitialForce, maxInitialForce), 0, Random.Range(-maxInitialForce, maxInitialForce)));
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void spawnNewAsteroid()
    {

    }
}
