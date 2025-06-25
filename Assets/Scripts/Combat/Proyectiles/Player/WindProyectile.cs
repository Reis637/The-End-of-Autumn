using UnityEngine;

public class WindProjectile : Projectile
{
    public int pierceCount = 1;
    private int _enemiesPierced = 0;

    private SpriteRenderer _spriteRenderer;

    protected override void Start()
    {
        base.Start();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    protected override void Update()
    {
        base.Update();

        if (_timer <= 0.5f && _spriteRenderer != null)
        {
            float alpha = Mathf.Clamp01(_timer / 0.5f);
            Color color = _spriteRenderer.color;
            color.a = alpha;
            _spriteRenderer.color = color;
        }
    }

    protected override void OnHitEnemy(SnowflakeIA enemy)
    {
        enemy.TakeDamage(Damage);
        _enemiesPierced++;

        if (_enemiesPierced > pierceCount)
        {
            explosion.Play();
            OnLifetimeEnd();
        }
    }

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            if (other.TryGetComponent<SnowflakeIA>(out var enemy))
            {
                OnHitEnemy(enemy);
            }
        }
        else if (other.CompareTag("Environment"))
        {
            Destroy(gameObject);
        }
    }
}