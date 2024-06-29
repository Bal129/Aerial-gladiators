using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public static readonly int TypeChase = 0;
    public static readonly int TypeThrow = 1;

    [Header("Enemy Prefabs")]
    public GameObject gladiatorPrefab;
    public GameObject[] enemyTypes;

    public Transform spawnZone;

    public GameObject SpawnGladiator()
    {
        return Instantiate(gladiatorPrefab, spawnZone.position, Quaternion.identity);
    }

    public GameObject SpawnEnemy(int type)
    {
        return Instantiate(enemyTypes[type], spawnZone.position, Quaternion.identity);
    }
}