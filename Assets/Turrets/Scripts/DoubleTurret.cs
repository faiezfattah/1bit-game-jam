using System.Collections;
using UnityEngine;

public class DoubleTurret : Turret
{
    [SerializeField] GameObject pointer1;
    [SerializeField] GameObject pointer2;
    private float attackTimer = 0;
    protected override void AttackState()
    {
        attackTimer -= Time.deltaTime;
        Transform target = GetTarget();
        if (target != null)
        {
            RotateToTarget(target);
            if (attackTimer <= 0)
            {
                StartCoroutine(ShootRoutine(target));
                attackTimer = turretData.attackInterval;
            }
        }
        else state = TurretState.Idle;
    }
    private void Shoot(Transform enemy, Vector2 position)
    {
        // TODO: object pooling here
        Bullet bulletInstance = Instantiate(turretData.bullet, position, Quaternion.identity).GetComponent<Bullet>();
        bulletInstance.GetComponent<Bullet>().Setup(turretData.bulletSpeed, turretData.damage, turretData.bulletLifeTime, enemy);
    }
    private IEnumerator ShootRoutine(Transform enemy)
    {
        Shoot(enemy, pointer1.transform.position);
        yield return new WaitForSeconds(turretData.attackInterval / 5);
        Shoot(enemy, pointer2.transform.position);
    }
}
