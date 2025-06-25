using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int CurrentHP { get; private set; }
    public bool IsDead => CurrentHP <= 0;
    public bool IsImmune => immunityTimer > 0f;

    private float immunityTimer = 0f;
    private PlayerStats stats;

    public event System.Action OnDeath;

    private bool hasReceivedHitThisFrame = false;

    void Start()
    {
        stats = PlayerCore.Instance.Stats;
        CurrentHP = stats.maxHP;
    }

    void Update()
    {
        if (immunityTimer > 0f)
            immunityTimer -= Time.deltaTime;

        hasReceivedHitThisFrame = false;
    }

    public void ReceiveHit(int amount, Vector2 knockbackDir)
    {
        if (IsDead || IsImmune || hasReceivedHitThisFrame) return;

        ApplyDamage(amount);
        PlayerCore.Instance.Knockback.ApplyKnockback(knockbackDir);
        GetComponent<PlayerFeedback>().PlayFlash();
        immunityTimer = stats.immunityDuration;
        hasReceivedHitThisFrame = true;
    }

    private void ApplyDamage(int amount)
    {
        CurrentHP -= amount;
        if (CurrentHP <= 0)
        {
            CurrentHP = 0;
            Die();
        }
    }

    public void Heal(int amount)
    {
        if (IsDead) return;

        CurrentHP += amount;
        if (CurrentHP > stats.maxHP)
            CurrentHP = stats.maxHP;
    }

    void Die()
    {
        OnDeath?.Invoke();
    }

    public void ApplyTemporaryImmunity(float time)
    {
        immunityTimer = Mathf.Max(immunityTimer, time);
    }


    void OnTriggerStay2D(Collider2D col)
    {
        if (IsDead || IsImmune || hasReceivedHitThisFrame) return;

        if (col.gameObject.CompareTag("Enemy"))
        {
            SnowflakeIA enemy = col.GetComponent<SnowflakeIA>();
            if (enemy != null)
            {
                Vector2 dir = (transform.position - col.transform.position).normalized;
                ReceiveHit(enemy.Damage, dir);
            }
        }
    }
}