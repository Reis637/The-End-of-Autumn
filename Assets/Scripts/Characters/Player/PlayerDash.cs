using UnityEngine;

public class PlayerDash : MonoBehaviour
{
    private PlayerMovement movement;
    private PlayerStats stats;
    private PlayerHealth health;
    private SpriteRenderer sr;

    public GameObject afterImagePrefab;

    private float dashTimer = 0f;
    private float afterImageTimer = 0f;
    private float cooldownTimer = 0f;

    private float currentMultiplier = 1f;
    private float elapsedTime = 0f;

    private float initialBoost = 0f;
    private float duration = 0f;

    private const float logCurveK = 6f;

    void Start()
    {
        movement = GetComponent<PlayerMovement>();
        stats = PlayerCore.Instance.Stats;
        health = PlayerCore.Instance.Health;
        sr = GetComponentInChildren<SpriteRenderer>();
    }

    void Update()
    {
        if (cooldownTimer > 0f)
            cooldownTimer -= Time.deltaTime;

        if (!movement.IsDashing() && cooldownTimer <= 0f && Input.GetKeyDown(KeyCode.Space))
        {
            StartDash();
        }

        if (movement.IsDashing())
        {
            dashTimer -= Time.deltaTime;
            elapsedTime += Time.deltaTime;

            float normalizedT = Mathf.Clamp01(elapsedTime / duration);
            float logDecay = Mathf.Log(1f + logCurveK * (1f - normalizedT)) / Mathf.Log(1f + logCurveK);
            currentMultiplier = 1f + initialBoost * logDecay;

            movement.SetDashMultiplier(currentMultiplier);

            afterImageTimer -= Time.deltaTime;
            if (afterImageTimer <= 0f && currentMultiplier > 1.1f)
            {
                CreateAfterImage();
                afterImageTimer = stats.afterImageInterval;
            }

            if (dashTimer <= 0f || currentMultiplier <= 1.01f)
            {
                EndDash();
            }
        }
    }

    void StartDash()
    {
        movement.SetIsDashing(true);
        initialBoost = stats.dashSpeedMultiplier - 1f;
        duration = stats.dashDuration;

        currentMultiplier = stats.dashSpeedMultiplier;
        dashTimer = duration;
        elapsedTime = 0f;

        afterImageTimer = 0f;
        cooldownTimer = stats.dashCooldown;

        movement.SetDashMultiplier(currentMultiplier);
        health.ApplyTemporaryImmunity(stats.dashDuration);
    }

    void EndDash()
    {
        movement.SetIsDashing(false);
        movement.SetDashMultiplier(1f);
    }

    void CreateAfterImage()
    {
        if (afterImagePrefab == null || sr == null) return;

        GameObject image = Instantiate(afterImagePrefab, transform.position, Quaternion.identity);
        SpriteRenderer cloneSR = image.GetComponent<SpriteRenderer>();

        if (cloneSR != null)
        {
            cloneSR.sprite = sr.sprite;
            cloneSR.flipX = sr.flipX;
            cloneSR.color = new Color(1f, 1f, 1f, 0.5f);
        }
    }
}
