using UnityEngine;

public class PlayerKnockback : MonoBehaviour
{
    private Rigidbody2D rb;
    private PlayerStats stats;

    private Vector2 pushStartPos;
    private Vector2 pushTargetPos;
    private float pushTimer = 0f;

    private PlayerHealth health;
    public bool IsBeingPushed => pushTimer > 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        stats = PlayerCore.Instance.Stats;
        health = PlayerCore.Instance.Health;
    }

    void Update()
    {
        if (!IsBeingPushed) return;

        pushTimer -= Time.deltaTime;
        float t = 1f - (pushTimer / stats.pushDuration);
        rb.MovePosition(Vector2.Lerp(pushStartPos, pushTargetPos, t));
    }

    public void ApplyKnockback(Vector2 direction)
    {
        if (direction == Vector2.zero) return;

        pushStartPos = rb.position;
        pushTargetPos = rb.position + direction.normalized * stats.knockbackDistance;
        pushTimer = stats.pushDuration;
    }
}
