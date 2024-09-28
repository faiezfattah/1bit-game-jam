using Unity.VisualScripting;
using UnityEngine;

public class SingleTurret : Turret
{
    protected float attackTimer = 0;
    protected override void AttackState()
    {
        attackTimer -= Time.deltaTime;
        Transform target = GetTarget();
        if (target != null)
        {
            RotateToTarget(target);
            if (attackTimer <= 0)
            {
                Shoot(target);
                attackTimer = turretData.attackInterval;
            }
        }
        else state = TurretState.Idle;
    }
    private void Shoot(Transform enemy)
    {
        // TODO: add object pooling for bullets
        Bullet bulletInstance = Instantiate(turretData.bullet, transform.position, Quaternion.identity).GetComponent<Bullet>();
        bulletInstance.GetComponent<Bullet>().Setup(turretData.bulletSpeed, turretData.damage, turretData.bulletLifeTime, enemy);
    }
}
