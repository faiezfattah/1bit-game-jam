using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    static Transform tower;
    static TowerHealth towerHealth;

    [SerializeField] private PlayerGameTime gameTime;
    [SerializeField] private PlayerEconomy economy;
    [SerializeField] private float speed = 1f;
    [SerializeField] private float attackRange = 2f;
    [SerializeField] private int maxHealth = 3;
    [SerializeField] private int damage = 1;
    [SerializeField] private float attackRate = 3;
    [SerializeField] private int minDropChance = 1;
    [SerializeField] private int maxDropChance = 5;
    [SerializeField] private int dropChance;
    [SerializeField] private int statIncrementOnDayCount = 3;
    [SerializeField] private float speedIncrement = 0.1f;
    [Header("Sound")]
    [SerializeField] private LocalAudioEvent localAudioRelay;
    [SerializeField] private AudioClip deathClip;

    private SpriteRenderer sprite;
    private Vector3 dir;
    private Rigidbody2D rb;
    private float distance;
    private int currentHealth;
    private bool isAttacking = false;
    private bool nearTower = false;
    void Start()
    {
        if (gameTime == null) Resources.Load<PlayerGameTime>("Resources/PlayerGameTime");
        if (economy == null) Resources.Load<PlayerEconomy>("Resources/PlayerEconomy");
        if (tower == null)
            tower = GameObject.FindGameObjectWithTag("Tower").GetComponent<Transform>();
        if (towerHealth == null)
            towerHealth = GameObject.FindGameObjectWithTag("Tower").GetComponent<TowerHealth>();
        if (rb == null)
            rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();

        Init();
    }
    private void Init() {
        dir = (tower.position - transform.position).normalized;

        int increase = Mathf.FloorToInt(gameTime.dayCount / statIncrementOnDayCount);

        damage += increase;
        dropChance = Mathf.Max(minDropChance, (maxDropChance - increase));

        speed += increase * speedIncrement;
        attackRate += increase * speedIncrement;
        currentHealth = maxHealth + increase;

        RotateSprite();
    }
    private void FixedUpdate()
    {
        distance = Vector2.Distance(transform.position, tower.position);

        if (distance > attackRange && !nearTower)
            Move();
        if (distance <= attackRange)
            nearTower = true;
        if (nearTower)
            Attack();
    }
    private void Move()
    {
        rb.linearVelocity = (dir * speed);
    }
    private void Attack()
    {
        if (!isAttacking)
            StartCoroutine(AttackCoroutine());
    }
    private IEnumerator AttackCoroutine() {
        isAttacking = true;
        while (isAttacking) {
            dir = (tower.position - transform.position).normalized;

            rb.linearVelocity = (dir * speed);
            yield return new WaitForSeconds(attackRate);
        }
    }
    public void TakeDamage(int damage)
    {
        StartCoroutine(AnimatedTakeDamage());
        currentHealth -= damage;
        if (currentHealth < 0)
            Die();
    }
    private void RotateSprite() {
        Vector3 targetOrientation = tower.position - transform.position;
        Vector3 currentOrientation = Vector3.right;
        float rotation = Vector3.SignedAngle(currentOrientation, targetOrientation, Vector3.forward);
        transform.rotation = Quaternion.Euler(0, 0, rotation);
    }
    private void Die()
    {
        Drops();
        localAudioRelay.RaiseEvent(deathClip, transform.position);
        Destroy(gameObject);
    }
    private void Drops() {
        int dropCount = Mathf.FloorToInt(dropChance);
        economy.AddIron(dropCount);
    }
    public void SetSpriteAlpha(float alphaValue)
    {
        Color spriteColor = sprite.color;
        spriteColor.a = alphaValue;
        sprite.color = spriteColor;
    }
    private IEnumerator AnimatedTakeDamage() {
        SetSpriteAlpha(0.1f);
        yield return new WaitForSeconds(0.2f);
        SetSpriteAlpha(1f);
    }
    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Tower")) {
            collision.gameObject.GetComponent<TowerHealth>().TakeDamage(damage);
            rb.linearVelocity = (-dir * speed / 2);
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
