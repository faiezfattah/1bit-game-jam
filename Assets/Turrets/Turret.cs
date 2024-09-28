using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.Android;

public class Turret : MonoBehaviour
{
    enum TurretState {Idle, Shooting};

    [SerializeField] TurretState state;
    [SerializeField] private static LayerMask enemyLayer;

    // TODO: move this to scriptable objects
    [SerializeField] GameObject bullet;
    [SerializeField] private float range = 2f;
    [SerializeField] private float shootInterval = 1f;
    [SerializeField] private float bulletSpeed = 1f;
    [SerializeField] private float bulletLifeTime = 2f;
    [SerializeField] private int damage = 1;

    private CircleCollider2D ShootingArea;
    private float shootTimer;
    private void Start()
    { 
        enemyLayer = LayerMask.GetMask("Enemy");
        state = TurretState.Idle;
        ShootingArea = GetComponent<CircleCollider2D>();
        ShootingArea.radius = range;
    }
    void Update()
    {
        switch (state)
        {
            case TurretState.Shooting:
                ShootingState();
                break;
        }
    }

    virtual protected void ShootingState()
    {
        shootTimer -= Time.deltaTime;
        Transform target = GetTarget();
        if (target != null )
        {
            RotateToTarget(target);
            if (shootTimer < 0)
            {
                Shoot(target);
                shootTimer = shootInterval;
            }
        }
        else state = TurretState.Idle;

    }
    private Transform GetTarget()
    {
        float maxDistance = 0f;
        Transform furthestEnemy = null;

        Collider2D[] EnemyInRange = Physics2D.OverlapCircleAll(transform.position, range, enemyLayer);

        foreach (Collider2D enemy in EnemyInRange)
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
    private void Shoot(Transform enemy)
    {
        // TODO: add object pooling for bullets
        Bullet bulletInstance = Instantiate(bullet, transform.position, Quaternion.identity).GetComponent<Bullet>();
        bulletInstance.GetComponent<Bullet>().Setup(bulletSpeed, damage, bulletLifeTime, enemy);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (state != TurretState.Shooting)
            state = TurretState.Shooting;
    }
    void RotateToTarget(Transform target)
    {
        Vector3 direction = target.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;

        Quaternion targetRotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 45f * Time.deltaTime);
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
