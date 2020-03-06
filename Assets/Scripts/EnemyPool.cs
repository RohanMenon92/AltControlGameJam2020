using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPool : MonoBehaviour
{
    public List<GameObject> enemyPrefabs;
    public Transform player;
    public int numOfEnemies;
    public int numOfEnemiesInPool;
    public float spawnTime;
    public float radius;

    public int hardModeWave;
    public float minFirePerSeconds;
    public float maxFirePerSeconds;

    public float minFirePerSecondsInHardMode;
    public float maxFirePerSecondsInHardMode;

    public List<GameObject> enemyPool;
    public List<GameObject> activeEnemy;
    void Start()
    {
        player = GetComponent<Transform>();
        for (int i = 0; i < numOfEnemiesInPool; i++)
        {
            GameObject newEnemy = Instantiate(enemyPrefabs[i%enemyPrefabs.Count], transform);
            enemyPool.Add(newEnemy);
            newEnemy.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Check if there are no enemies active

        //bool allInactive = true;
        //for (int i=0; i<numOfEnemies; i++)
        //{
        //    if (enemyPool[i].activeSelf)
        //    {
        //        allInactive = false;
        //    }
        //}
        // If all enemies are inactive
        //if (allInactive)
        if(activeEnemy.Count == 0)
        {
            FindObjectOfType<GameManager>().IncrementWaves();
            int randPosition = Random.Range(numOfEnemies, numOfEnemies*2);
            for (int i = 0; i < numOfEnemies; i++)
            {
                int randomIndexInPool = Random.Range(0, enemyPool.Count);
                float angle = i * Mathf.PI * 2 / randPosition;
                float x = Mathf.Cos(angle) * radius;
                float z = Mathf.Sin(angle) * radius;
                Vector3 pos = player.position + new Vector3(x, 0, z);
                float angleDegrees = -angle * Mathf.Rad2Deg;
                Quaternion rot = Quaternion.Euler(0, angleDegrees, 0);
                GameObject selectedEnemy = enemyPool[randomIndexInPool];
                selectedEnemy.transform.position = pos;
                selectedEnemy.transform.rotation = rot;
                selectedEnemy.transform.Rotate(0, -90, 0);
                selectedEnemy.SetActive(true);
                randomGunPortParams(enemyPool[randomIndexInPool].GetComponent<Enemy>().gunPorts);
                activeEnemy.Add(selectedEnemy);
                enemyPool.Remove(selectedEnemy);

            }
        }
    }

    void randomGunPortParams(List<GunPort> gunPorts)
    {
        foreach (GunPort gun in gunPorts)
        {
            if (FindObjectOfType<GameManager>().currentWave >= hardModeWave)
            {
                gun.firePerSeconds = Random.Range(minFirePerSecondsInHardMode, maxFirePerSecondsInHardMode);
                gun.gunType = (GameConstants.GunTypes)Random.Range(0, 3);
            }
            else
            {
                gun.firePerSeconds = Random.Range(minFirePerSeconds, maxFirePerSeconds);
                gun.gunType = (GameConstants.GunTypes)Random.Range(0, 2);
            }
            gun.resetCanFireOnRespawn();
        }
    }
}
