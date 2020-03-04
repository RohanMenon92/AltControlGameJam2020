using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPool : MonoBehaviour
{
    public List<GameObject> enemyPrefabs;
    public Transform player;
    public int numOfEnemies;
    public float spawnTime;
    public float radius;

    public List<GameObject> enemyPool;
    void Start()
    {
        player = player.GetComponent<Transform>();
        for (int i = 0; i < numOfEnemies; i++)
        {
            GameObject newEnemy = Instantiate(enemyPrefabs[i%2], transform);
            enemyPool.Add(newEnemy);
            newEnemy.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Check if there are no enemies active
        bool allInactive = true;
        for (int i=0; i<numOfEnemies; i++)
        {
            if (enemyPool[i].activeSelf)
            {
                allInactive = false;
            }
        }
        // If all enemies are inactive
        if (allInactive)
        {
            FindObjectOfType<GameManager>().IncrementWaves();
            int randPosition = Random.Range(numOfEnemies, numOfEnemies*2);
            for (int i = 0; i < numOfEnemies; i++)
            {
                float angle = i * Mathf.PI * 2 / randPosition;
                float x = Mathf.Cos(angle) * radius;
                float z = Mathf.Sin(angle) * radius;
                Vector3 pos = player.position + new Vector3(x, 0, z);
                float angleDegrees = -angle * Mathf.Rad2Deg;
                Quaternion rot = Quaternion.Euler(0, angleDegrees, 0);
                enemyPool[i].transform.position = pos;
                enemyPool[i].transform.rotation = rot;
                enemyPool[i].transform.Rotate(0, -90, 0);
                enemyPool[i].SetActive(true);
            }
        }
    }
}
