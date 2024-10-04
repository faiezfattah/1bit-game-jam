using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Pool;

public abstract class Turret : Build
{
    protected enum TurretState { Idle, Attacking };
    [SerializeField] protected TurretState state;

    protected static LayerMask enemyLayer;
    protected CircleCollider2D attackArea;

    private int minBulletPool;
    [SerializeField] protected int maxBulletPool = 10000;
    protected ObjectPool<GameObject> bulletPool;
    protected float attackTimer = 0;
    protected virtual void Start()
    {
        enemyLayer = LayerMask.GetMask("Enemy");
        state = TurretState.Idle;

        if (attackArea == null) attackArea = GetComponent<CircleCollider2D>();
        attackArea.radius = data.range;

        minBulletPool = Mathf.FloorToInt(data.bulletSpeed * data.attackInterval * data.range);

        bulletPool = new ObjectPool<GameObject>(() => {
            return Instantiate(data.bullet);
        }, bullet => {
            bullet.GetComponent<Bullet>().Setup(data.bulletSpeed, data.damage, data.bulletLifeTime, ReleaseBullet);
            bullet.SetActive(true);
        }, bullet => {
            bullet.SetActive(false);
        }, bullet => {
            Destroy(bullet);
        }, false, minBulletPool, maxBulletPool);
    }

    protected virtual void Update()
    {
        if (attackArea.radius != data.range)
            attackArea.radius = data.range;
        if (state == TurretState.Attacking)
            AttackState();
    }
    protected virtual void AttackState() {

        Transform target = GetTarget();

        if (target == null) {
            state = TurretState.Idle;
            return;
        }

        RotateToTarget(target);

        Vector2 nozzleLocation = -transform.right;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, nozzleLocation, data.range, enemyLayer);

        attackTimer -= Time.deltaTime;

        if (attackTimer <= 0 && hit.collider != null) {
            Shoot(hit.collider.transform);
            attackTimer = data.attackInterval;
        }
    }
    protected abstract void Shoot(Transform shootingTarget);

    protected virtual Transform GetTarget()
    {
        float maxDistance = 0f;
        Transform furthestEnemy = null;
        Collider2D[] enemiesInRange = Physics2D.OverlapCircleAll(transform.position, data.range, enemyLayer);
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
        private void ReleaseBullet(GameObject bullet) {
        bulletPool.Release(bullet);
    }
    protected virtual void RotateToTarget(Transform target)
    {
        Vector3 direction = target.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 180f;
        Quaternion targetRotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, data.rotationSpeed * Time.deltaTime);
    }
    protected virtual void OnDrawGizmosSelected()
    {
        if (data != null)
        {
            Gizmos.DrawWireSphere(transform.position, data.range);
        }
    }
}