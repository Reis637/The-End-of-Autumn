using UnityEngine;

public class WindProjectile : MonoBehaviour
{
    public float speed;
    public float maxTime;
    public int damage;

    private float timer;
    private Vector3 direction;
    private bool directionSet = false;

    private SpriteRenderer spriteRenderer;

    public void SetDirection(Vector3 dir)
    {
        direction = dir.normalized;
        directionSet = true;

        if (direction != Vector3.zero)
        {
            Vector2 spriteForward = new Vector2(-1, 1);
            float angle = Vector2.SignedAngle(spriteForward, direction);
            transform.rotation = Quaternion.Euler(0f, 0f, angle);
        }
    }

    void Start()
    {
        timer = maxTime;
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (!directionSet)
        {
            Debug.LogWarning("WindProjectile no recibió dirección antes de iniciar.");
            Destroy(gameObject);
        }
    }

    void Update()
    {
        transform.position += direction * speed * Time.deltaTime;

        timer -= Time.deltaTime;

        if (timer <= 0.5f && spriteRenderer != null)
        {
            float alpha = Mathf.Clamp01(timer / 0.5f);
            Color color = spriteRenderer.color;
            color.a = alpha;
            spriteRenderer.color = color;
        }

        if (timer <= 0f)
        {
            Destroy(gameObject);
        }
    }
}
