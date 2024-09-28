using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 0;
    public float damage = 0;
    public Transform target = null;
    private void Update()
    {
        Vector3 dir = (target.position - transform.position).normalized;
        transform.position += speed * Time.deltaTime * dir;
    }
    public void Setup(float speed, float damage, Transform target)
    {
        this.speed = speed;
        this.damage = damage;
        this.target = target;
    }

    //TODO: Detect collision and damage the collider;
}
