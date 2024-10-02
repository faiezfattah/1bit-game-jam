using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    [SerializeField] private InputReader inputReader;
    [SerializeField] private PlayerGameTime gameTime;
    [SerializeField] private PlayerEconomy economy;
    [SerializeField] private PlayerBuild build;
    [Header("factory-----------------")]
    [SerializeField] private GameObject[] regularEnemy;
    [SerializeField] private GameObject[] bossEnemy;
    [SerializeField] private GameObject[] pointer;
    [Header("settings ---------------")]
    [SerializeField] private float spawnInterval = 0.5f;
    [SerializeField] private int minEnemy = 5;

    private bool isPaused = false;
    private void SpawnEnemy()
    { 
        StartCoroutine(SpawningRoutine());
    }
    IEnumerator SpawningRoutine()
    {
        // 1. randomize which pointer to be spawned at CHECK
        // 2. ramdomize what kind of enemy to be spawned ???
        // 3. randomize how which enemy CHECK
        // 4. randomize how many ?

        Transform spawningPoint = pointer[Randomizer(pointer.Length)].GetComponent<Transform>();
        GameObject spawningEnemy = regularEnemy[Randomizer(regularEnemy.Length)];


        int enemiesToSpawn = minEnemy + gameTime.dayCount;

        for (int i = 0; i < minEnemy; i++)
        {
            Instantiate(spawningEnemy, spawningPoint.transform.position, Quaternion.identity);
            yield return new WaitForSeconds(spawnInterval);
        }
    }
    private void Update()
    {
        gameTime.UpdateGameTime();
    }

    private int Randomizer(int max)
    {
        return Random.Range(0, max);
    }
    private void Pause()
    {
        isPaused = !isPaused;

        if (isPaused) Time.timeScale = 1f;
        if (!isPaused)Time.timeScale = 0f;
    }
    private void OnEnable()
    {
        gameTime.onDayOver += SpawnEnemy;
        inputReader.EscapeEvent += Pause;
    }
    private void OnDisable()
    {
        gameTime.onDayOver -= SpawnEnemy;
        inputReader.EscapeEvent -= Pause;
    }
}

