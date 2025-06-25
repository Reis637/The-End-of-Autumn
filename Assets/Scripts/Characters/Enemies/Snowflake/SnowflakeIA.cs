using UnityEngine;

public class SnowflakeIA : MonoBehaviour
{
    private Transform player;
    private SnowflakeStats stats;
    private Rigidbody2D rb;
    private bool isDead = false;
    private SnowflakeAnim anim;

    public int Damage => stats.damage;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        stats = GetComponent<SnowflakeStats>();
        anim = GetComponent<SnowflakeAnim>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (isDead || player == null) return;

        Vector2 playerPos2D = new(player.position.x, player.position.y);
        Vector2 dir = (playerPos2D - rb.position).normalized;

        rb.MovePosition(rb.position + stats.moveSpeed * Time.deltaTime * dir);
    }

    public void TakeDamage(int dmg)
    {
        if (isDead) return;

        stats.currentHP -= dmg;
        if (stats.currentHP <= 0)
        {
            stats.currentHP = 0;
            Die();
        }
    }

    void Die()
    {
        isDead = true;
        rb.linearVelocity = Vector2.zero;

        Collider2D col = GetComponent<Collider2D>();
        if (col != null) col.enabled = false;

        float deathDuration = anim.PlayDeath();
        Destroy(gameObject, deathDuration);
    }
}