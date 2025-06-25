using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private Vector2 moveDir;

    private PlayerHealth health;
    private PlayerStats stats;
    private PlayerKnockback knockback;

    [Header("Visual Tilt")]
    public Transform visual;
    public float tiltAngle = 15f;
    public float tiltSpeed = 5f;

    private float dashMultiplier = 1f;
    private bool isDashing = false;

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
        HandleTilt();
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
        float effectiveSpeed = stats.moveSpeed * dashMultiplier;
        rb.linearVelocity = moveDir * effectiveSpeed;
    }

    void HandleTilt()
    {
        float targetZ = moveDir.x * -tiltAngle;
        Quaternion targetRot = Quaternion.Euler(0, 0, targetZ);
        visual.localRotation = Quaternion.Lerp(visual.localRotation, targetRot, Time.deltaTime * tiltSpeed);
    }

    public bool IsDashing() => isDashing;
    public void SetIsDashing(bool value) => isDashing = value;
    public void SetDashMultiplier(float value) => dashMultiplier = Mathf.Max(1f, value);
}
