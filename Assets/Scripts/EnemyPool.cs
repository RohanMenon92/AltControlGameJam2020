﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPool : MonoBehaviour
{
    public List<GameObject> enemyPrefabs;
    public int numOfEnemies;
    public float spawnTime;
    public float radius;

    public List<GameObject> enemyPool;
    void Start()
    {
        for (int i = 0; i < numOfEnemies; i++)
        {
            GameObject newEnemy = Instantiate(enemyPrefabs[Random.Range(0, 1)], transform);
            enemyPool.Add(newEnemy.GetComponent<GameObject>());
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
            int randPosition = Random.Range(0, 6);
            for (int i = 0; i < numOfEnemies; i++)
            {
                float angle = i * Mathf.PI * 2 / numOfEnemies*randPosition;
                float x = Mathf.Cos(angle) * radius;
                float z = Mathf.Sin(angle) * radius;
                Vector3 pos = transform.position + new Vector3(x, 0, z);
                float angleDegrees = -angle * Mathf.Rad2Deg;
                Quaternion rot = Quaternion.Euler(0, angleDegrees, 0);
                enemyPool[i].transform.position = pos;
                enemyPool[i].transform.rotation = rot;
                enemyPool[i].SetActive(true);
            }
        }
    }
}
