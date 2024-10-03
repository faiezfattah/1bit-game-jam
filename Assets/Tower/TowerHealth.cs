using UnityEngine;

public class TowerHealth : MonoBehaviour
{
    [SerializeField] PlayerBuild build;
    [SerializeField] VoidChannel gameoverRelay;
    // TODO: trigger gameover
    private void Start()
    {
        build.currentTowerHealth = build.maxTowerHealth;
    }
    public void TakeDamage(int damage)
    {
        build.currentTowerHealth -= damage;
        if (build.currentTowerHealth < 0) {
            Die();
        }
    }
    private void Die()
    {
        Debug.Log("Tower destroyed");
    }
}
