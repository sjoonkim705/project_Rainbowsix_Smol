using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnPoint : MonoBehaviour
{
    private float _spawnTimer = 0;
    public float SpawnInterval = 3f;
    public GameObject EnemyPrefab;
    private void OnEnable()
    {
        _spawnTimer = 0;
    }
    void Update()
    {
        _spawnTimer += Time.deltaTime;
        if (_spawnTimer > SpawnInterval)
        {
            GameObject newEnemy = GameObject.Instantiate(EnemyPrefab);
            newEnemy.transform.position = this.transform.position;
            _spawnTimer = 0;
        }
    }
}
