using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private Vector2 moveDir;

    private PlayerHealth health;
    private PlayerStats stats;
    private PlayerKnockback knockback;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        health = PlayerCore.Instance.Health;
        stats = PlayerCore.Instance.Stats;
        knockback = PlayerCore.Instance.Knockback;
    }

    void Update()
    {
        InputManagement();
    }

    void FixedUpdate()
    {
        if (health.IsDead || knockback.IsBeingPushed) return;
        Move();
    }

    void InputManagement()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        moveDir = new Vector2(moveX, moveY).normalized;
    }

    void Move()
    {
        rb.linearVelocity = moveDir * stats.moveSpeed;
    }
}
