using System.Collections;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class DoubleTurret : Turret
{
    [SerializeField] GameObject pointer1;
    [SerializeField] GameObject pointer2;
    [SerializeField] private float secondShotCooldown = 0.2f;

    protected override void Shoot(Transform target) {
        StartCoroutine(ShootRoutine(target));
    }
    private IEnumerator ShootRoutine(Transform target) {
        GameObject bullet = bulletPool.Get();
        bullet.GetComponent<Bullet>().FiringInit(target, pointer1.transform.position);
        yield return new WaitForSeconds(secondShotCooldown);
        bullet = bulletPool.Get();
        bullet.GetComponent<Bullet>().FiringInit(target, pointer2.transform.position);
    }
}
