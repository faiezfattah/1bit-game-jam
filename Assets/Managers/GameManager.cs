using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    [SerializeField] private PlayerGameTime gameTime;
    [SerializeField] private GameObject[] regularEnemy;
    [SerializeField] private GameObject[] bossEnemy;
    [SerializeField] private GameObject[] pointer;
    [Header("settings ---------------")]
    [SerializeField] private int minEnemy = 5;
    private void SpawnEnemy()
    { 
        Transform spawningPoint = pointer[Randomizer(pointer.Length)].GetComponent<Transform>();
        GameObject spawningEnemy = regularEnemy[Randomizer(regularEnemy.Length)];

        for (int i = 0; i < minEnemy; i++)
        {
            Instantiate(spawningEnemy, spawningPoint.transform.position, Quaternion.identity);
        }

        // 1. randomize which pointer to be spawned at CHECK
        // 2. ramdomize what kind of enemy to be spawned ???
        // 3. randomize how which enemy CHECK
        // 4. randomize how many ?
    }
    private void Update()
    {
        gameTime.UpdateGameTime();
    }

    private int Randomizer(int max)
    {
        return Random.Range(0, max);
    }

    private void OnEnable()
    {
        gameTime.onDayOver += SpawnEnemy;
    }
    private void OnDisable()
    {
        gameTime.onDayOver -= SpawnEnemy;
    }
}
