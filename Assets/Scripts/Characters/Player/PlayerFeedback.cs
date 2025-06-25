using UnityEngine;

public class PlayerFeedback : MonoBehaviour
{
    [Header("Visual Settings")]
    public SpriteRenderer spriteRenderer;
    public Color damageFlashColor = Color.white;
    public float flashDuration = 0.1f;
    public float immunityBlinkInterval = 0.1f;

    private Color originalColor;
    private float immunityBlinkTimer = 0f;
    private bool isFlickering = false;

    private PlayerHealth health;

    void Start()
    {
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();

        originalColor = spriteRenderer.color;
        health = PlayerCore.Instance.Health;
    }

    void Update()
    {
        if (health.IsImmune)
        {
            if (!isFlickering)
                StartFlicker();

            Flicker();
        }
        else if (isFlickering)
        {
            StopFlicker();
        }
    }

    public void PlayFlash()
    {
        StopAllCoroutines();
        StartCoroutine(FlashCoroutine());
    }

    System.Collections.IEnumerator FlashCoroutine()
    {
        spriteRenderer.color = damageFlashColor;
        yield return new WaitForSeconds(flashDuration);
        spriteRenderer.color = originalColor;
    }

    void StartFlicker()
    {
        isFlickering = true;
        immunityBlinkTimer = 0f;
    }

    void Flicker()
    {
        immunityBlinkTimer -= Time.deltaTime;
        if (immunityBlinkTimer <= 0f)
        {
            spriteRenderer.enabled = !spriteRenderer.enabled;
            immunityBlinkTimer = immunityBlinkInterval;
        }
    }

    void StopFlicker()
    {
        isFlickering = false;
        spriteRenderer.enabled = true;
        spriteRenderer.color = originalColor;
    }
}
