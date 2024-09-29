using UnityEngine;

public class TowerHealth : MonoBehaviour
{
    [SerializeField] public int maxHealth = 100;

    private int currentHealth;
    private void Start()
    {
        currentHealth = maxHealth;
    }
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth < 0)
            Die();
    }
    private void Die()
    {
        Debug.Log("Tower destroyed");
    }
}
