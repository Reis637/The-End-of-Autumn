using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int CurrentHP { get; private set; }
    public bool IsDead => CurrentHP <= 0;
    public bool IsImmune => immunityTimer > 0f;

    private float immunityTimer = 0f;

    private PlayerStats stats;

    public event System.Action OnDeath;

    void Start()
    {
        stats = PlayerCore.Instance.Stats;
        CurrentHP = stats.maxHP;
    }

    void Update()
    {
        if (immunityTimer > 0f)
            immunityTimer -= Time.deltaTime;
    }

    public void TakeDamage(int amount)
    {
        if (IsDead || IsImmune)
            return;

        CurrentHP -= amount;
        if (CurrentHP <= 0)
        {
            CurrentHP = 0;
            Die();
        }

        immunityTimer = stats.immunityDuration;
    }

    public void Heal(int amount)
    {
        if (IsDead) return;

        CurrentHP += amount;
        if (CurrentHP > PlayerCore.Instance.Stats.maxHP)
            CurrentHP = PlayerCore.Instance.Stats.maxHP;
    }

    void Die()
    {
        OnDeath?.Invoke();
    }
}
