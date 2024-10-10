using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{
    [SerializeField] private PlayerGameTime gameTime;
    [SerializeField] private VoidChannel resetRelay;
    [Header("enemies ---------------")]
    [SerializeField] private GameObject[] regularEnemy;
    [SerializeField] private GameObject[] bossEnemy;
    [SerializeField] private GameObject[] pointer;
    [Header("settings ---------------")]
    [SerializeField] private float spawnInterval = 2f;
    [SerializeField] private int minEnemy = 3;
    [SerializeField] private int dayToIncreaseEnemy = 2;
    [SerializeField] private int maxEnemyPerLane =10;

    private GameObject spawningEnemy;
    private Transform currentSpawningPoint;
    IEnumerator SpawningRoutine(int enemiesToSpawn, Vector3 position) {       

        for (int j = 0; j < enemiesToSpawn; j++) {
            Instantiate(spawningEnemy, position, Quaternion.identity);
            yield return new WaitForSeconds(spawnInterval);
        }
    }
    private void SpawnEnemy() {
        spawningEnemy = regularEnemy[Randomizer(regularEnemy.Length)];
        currentSpawningPoint = pointer[Randomizer(pointer.Length)].GetComponent<Transform>();

        int enemiesToSpawn = minEnemy + Mathf.FloorToInt(gameTime.dayCount / dayToIncreaseEnemy);

        if (enemiesToSpawn > maxEnemyPerLane) {
            while (enemiesToSpawn > maxEnemyPerLane) {
                StartCoroutine(SpawningRoutine(maxEnemyPerLane, currentSpawningPoint.position));
                Transform next = pointer[Randomizer(pointer.Length)].GetComponent<Transform>();
                if (currentSpawningPoint == next) {
                    while (currentSpawningPoint == next) {
                        next = pointer[Randomizer(pointer.Length)].GetComponent<Transform>();
                    }
                }
                currentSpawningPoint = next;
                enemiesToSpawn -= maxEnemyPerLane;
            }
        }

        StartCoroutine(SpawningRoutine(enemiesToSpawn, currentSpawningPoint.position));
    }
    private int Randomizer(int max) {
        return Random.Range(0, max);
    }
    private void KillAllEnemy() {
        Enemy[] enemy = FindObjectsByType<Enemy>(FindObjectsSortMode.None);
        foreach (Enemy i in enemy) {
            Destroy(i.gameObject);
        }
    }
    private void OnEnable() {
        gameTime.onDayOver += SpawnEnemy;
        resetRelay.OnEvenRaised += KillAllEnemy;
    }
    private void OnDisable() {
        gameTime.onDayOver -= SpawnEnemy;
        resetRelay.OnEvenRaised -= KillAllEnemy;
    }
}
