using System;
using UnityEngine;
using UnityEngine.Pool;

public class Bullet : MonoBehaviour
{
    public float speed = 0;
    public int damage = 0;
    public float lifeTime = 2f;
    public Transform target = null;
    public Vector3 targetLocation;
    public Action<GameObject> releaseObject;

    [SerializeField] private LocalAudioEvent localAudioRelay;
    [SerializeField] private AudioClip hitSfx;

    private bool hasHitEnemy;
    private void Update()
    {
        lifeTime -= Time.deltaTime;
        if (lifeTime < 0)
            releaseObject(this.gameObject);

        if (target != null)
            targetLocation = target.position;
        if (target == null) releaseObject(this.gameObject);

        Vector3 dir = (targetLocation - transform.position).normalized;
        transform.position += speed * Time.deltaTime * dir;
    }
    public void Setup(float speed, int damage, float lifeTime, Action<GameObject> release)
    {
        this.speed = speed;
        this.damage = damage;
        this.lifeTime = lifeTime;
        releaseObject = release;
        hasHitEnemy = false;
    }
    public void FiringInit(Transform target, Vector2 firingPosition) {
        this.target = target;
        this.transform.position = firingPosition;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!hasHitEnemy && collision.gameObject.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<Enemy>().TakeDamage(damage);
            releaseObject(this.gameObject);
            hasHitEnemy = true;
            localAudioRelay.RaiseEvent(hitSfx, transform.position);
        }
    }
}
