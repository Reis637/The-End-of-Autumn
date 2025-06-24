using UnityEngine;

public class DebugTester : MonoBehaviour
{
    private PlayerHealth health;
    private PlayerKnockback knockback;

    void Start()
    {
        health = PlayerCore.Instance.Health;
        knockback = PlayerCore.Instance.Knockback;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            health.TakeDamage(10);
            knockback.ApplyKnockback(Vector2.up);
            Debug.Log("Daño aplicado y knockback hacia arriba");
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            health.Heal(10);
            Debug.Log("Jugador curado");
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            Debug.Log($"HP Actual: {health.CurrentHP} / {PlayerCore.Instance.Stats.maxHP}");
        }
    }
}
