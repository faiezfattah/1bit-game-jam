using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    static Transform tower;
    static TowerHealth towerHealth;

    [SerializeField] private float speed = 1f;
    [SerializeField] private float attackRange = 2f;
    [SerializeField] private int maxHealth = 3;
    [SerializeField] private int damage = 1;
    [SerializeField] private float attackRate = 1;

    private Vector3 dir;
    private Rigidbody2D rb;
    private float distance;
    private int currentHealth;
    private bool isAttacking = false;
    void Start()
    {
        if (tower == null)
            tower = GameObject.FindGameObjectWithTag("Tower").GetComponent<Transform>();
        if (towerHealth == null)
            towerHealth = GameObject.FindGameObjectWithTag("Tower").GetComponent<TowerHealth>();

        if (rb == null)
            rb = GetComponent<Rigidbody2D>();
        dir = (tower.position - transform.position).normalized;
        currentHealth = maxHealth;
        RotateSprite();
    }

    private void FixedUpdate()
    {
        distance = Vector2.Distance(transform.position, tower.position);

        if (distance > attackRange)
            Move();
        if (distance <= attackRange)
            Attack();
    }
    private void Move()
    {
        rb.linearVelocity = dir * speed;
    }
    private void StopMove()
    {
        // because of kinematic body it need to be explicitly stopped
        rb.linearVelocity = new Vector2(0, 0);
    }
    private void Attack()
    {
        StopMove();
        if (!isAttacking)
            StartCoroutine(AttackCoroutine());
    }
    private IEnumerator AttackCoroutine()
    {
        isAttacking = true;
        while (isAttacking)
        {
            towerHealth.TakeDamage(damage);
            Debug.Log("attacking");
            yield return new WaitForSeconds(attackRate);
        }
    }
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth < 0)
            Die();
    }
    private void Die()
    {
        Destroy(gameObject);
    }
    private void  RotateSprite() {
        Vector3 targetOrientation = tower.position - transform.position;
        Vector3 currentOrientation = Vector3.right;
        float rotation = Vector3.SignedAngle(currentOrientation, targetOrientation, Vector3.forward);
        transform.rotation = Quaternion.Euler(0, 0, rotation);
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
