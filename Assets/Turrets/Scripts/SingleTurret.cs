using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;

public class SingleTurret : Turret
{
    protected override void Shoot(Transform target)
    {
        GameObject bullet = bulletPool.Get();
        bullet.GetComponent<Bullet>().FiringInit(target, transform.position);
    }
}
