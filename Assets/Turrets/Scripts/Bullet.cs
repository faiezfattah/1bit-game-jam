using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 0;
    public int damage = 0;
    public float lifeTime = 2f;
    public Transform target = null;
    public Vector3 targetLocation;
    private void Update()
    {
        lifeTime -= Time.deltaTime;
        if (lifeTime < 0)
            Destroy(gameObject);

        if (target != null)
            targetLocation = target.position;

        Vector3 dir = (targetLocation - transform.position).normalized;
        transform.position += speed * Time.deltaTime * dir;
    }
    public void Setup(float speed, int damage, float lifeTime, Transform target)
    {
        this.speed = speed;
        this.damage = damage;
        this.lifeTime = lifeTime;
        this.target = target;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<Enemy>().TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
