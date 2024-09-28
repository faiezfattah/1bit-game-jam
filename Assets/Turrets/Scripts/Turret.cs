using UnityEngine;
using System.Collections.Generic;

public abstract class Turret : MonoBehaviour
{
    protected enum TurretState { Idle, Attacking };
    [SerializeField] protected TurretState state;
    [SerializeField] protected TurretData turretData;

    protected static LayerMask enemyLayer;
    protected CircleCollider2D attackArea;

    protected virtual void Start()
    {
        enemyLayer = LayerMask.GetMask("Enemy");
        state = TurretState.Idle;
        attackArea = GetComponent<CircleCollider2D>();
        attackArea.radius = turretData.range;
    }

    protected virtual void Update()
    {
        if (state == TurretState.Attacking)
        {
            AttackState();
        }
    }
    protected abstract void AttackState();

    protected virtual Transform GetTarget()
    {
        float maxDistance = 0f;
        Transform furthestEnemy = null;
        Collider2D[] enemiesInRange = Physics2D.OverlapCircleAll(transform.position, turretData.range, enemyLayer);
        foreach (Collider2D enemy in enemiesInRange)
        {
            float distance = Vector2.Distance(transform.position, enemy.transform.position);
            if (distance > maxDistance)
            {
                maxDistance = distance;
                furthestEnemy = enemy.transform;
            }
        }
        return furthestEnemy;
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (state != TurretState.Attacking)
            state = TurretState.Attacking;
    }

    protected virtual void RotateToTarget(Transform target)
    {
        Vector3 direction = target.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 180f;
        Quaternion targetRotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, turretData.rotationSpeed * Time.deltaTime);
    }

    protected virtual void OnDrawGizmosSelected()
    {
        if (turretData != null)
        {
            Gizmos.DrawWireSphere(transform.position, turretData.range);
        }
    }
}