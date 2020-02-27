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

    public int unusedAsteroidThreshold;

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
        if (unusedAsteroid.Count >= unusedAsteroidThreshold)
        {
            for (int i = 0; i < unusedAsteroid.Count; i++)
            {
                reuseAsteroid(unusedAsteroid[i]);
            }
            unusedAsteroid.Clear();
        }

        foreach(Asteroid asteroid in asteroidPool)
        {
            if(asteroid.gameObject.activeSelf)
            {
                if (asteroid.transform.position.x < -maxX || asteroid.transform.position.x > maxX || asteroid.transform.position.z < -maxZ || asteroid.transform.position.z > maxZ)
                {
                    asteroid.OnHit();
                }
            }
        }
    }

    void reuseAsteroid(Asteroid asteroid)
    {
        asteroid.gameObject.SetActive(true);
        Vector3 newPosition = new Vector3(Random.Range(-maxX, maxX), 0, Random.Range(-maxZ, maxZ));
        asteroid.transform.position = newPosition;
        asteroid.GetComponent<ParticleSystem>().Stop();
        asteroid.GetComponent<MeshRenderer>().enabled = true;
        asteroid.GetComponent<MeshCollider>().enabled = true;
        asteroid.GetComponent<Rigidbody>().velocity = Vector3.zero;
        asteroid.GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(-maxInitialForce, maxInitialForce), 0, Random.Range(-maxInitialForce, maxInitialForce)));

    }
}
