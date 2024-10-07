using System.Collections;
using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{
    [SerializeField] private PlayerGameTime gameTime;
    [SerializeField] private VoidChannel gameoverRelay;
    [SerializeField] private VoidChannel restartRelay;
    [Header("enemies ---------------")]
    [SerializeField] private GameObject[] regularEnemy;
    [SerializeField] private GameObject[] bossEnemy;
    [SerializeField] private GameObject[] pointer;
    [Header("settings ---------------")]
    [SerializeField] private float spawnInterval = 2f;
    [SerializeField] private int minEnemy = 3;
    IEnumerator SpawningRoutine() {
        // 1. randomize which pointer to be spawned at CHECK
        // 2. ramdomize what kind of enemy to be spawned ???
        // 3. randomize how which enemy CHECK
        // 4. randomize how many ?

        Transform spawningPoint = pointer[Randomizer(pointer.Length)].GetComponent<Transform>();
        GameObject spawningEnemy = regularEnemy[Randomizer(regularEnemy.Length)];


        int enemiesToSpawn = minEnemy + gameTime.dayCount;

        for (int i = 0; i < enemiesToSpawn; i++) {
            Instantiate(spawningEnemy, spawningPoint.transform.position, Quaternion.identity);
            yield return new WaitForSeconds(spawnInterval);
        }
    }
    private void SpawnEnemy() {
        StartCoroutine(SpawningRoutine());
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
        restartRelay.OnEvenRaised += KillAllEnemy;
    }
    private void OnDisable() {
        gameTime.onDayOver -= SpawnEnemy;
        restartRelay.OnEvenRaised -= KillAllEnemy;
    }
}
