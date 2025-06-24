using UnityEngine;

public class SnowflakeIA : MonoBehaviour
{
    private Transform player;
    private SnowflakeStats stats;
    private Rigidbody2D rb;
    private bool isDead = false;
    private SnowflakeAnim anim;

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

        Vector2 dir = (player.position - transform.position).normalized;
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
        anim.PlayDeath();
        Destroy(gameObject, 1.5f);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (isDead) return;
        if (col.gameObject.CompareTag("Player"))
        {
            PlayerHealth health = col.gameObject.GetComponent<PlayerHealth>();
            if (health != null)
            {
                health.TakeDamage(stats.damage);
            }

            PlayerKnockback knockback = col.gameObject.GetComponent<PlayerKnockback>();
            if (knockback != null)
            {
                Vector2 dir = (col.transform.position - transform.position).normalized;
                knockback.ApplyKnockback(dir);
            }
        }


        if (col.gameObject.CompareTag("PlayerProyectile"))
        {

            WindProjectile proyectile = col.gameObject.GetComponent<WindProjectile>();



            if (proyectile != null)
            {
                TakeDamage(proyectile.damage);
            }
        }
    }
}
