using UnityEngine;

public class Enemy : MonoBehaviour
{
    static Transform tower;

    [SerializeField] private float speed = 1f;
    [SerializeField] private float attackRange = 2f;

    private Vector3 dir;
    private Rigidbody2D rb;
    private float distance;
    void Start()
    {
        if (tower == null)
            tower = GameObject.FindGameObjectWithTag("Tower").GetComponent<Transform>();
        if (rb == null)
            rb = GetComponent<Rigidbody2D>();
        dir = (tower.position - transform.position).normalized;
    }

    private void Update()
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
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
