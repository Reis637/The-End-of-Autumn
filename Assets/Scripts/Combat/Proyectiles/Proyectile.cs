using UnityEngine;

public abstract class Projectile : MonoBehaviour
{
    public float Speed { get; protected set; }
    public float MaxTime { get; protected set; }
    public int Damage { get; protected set; }
    public Vector3 Direction { get; protected set; }

    protected float _timer;
    protected bool _directionSet = false;

    private ParticleSystem trail;

    public virtual void Initialize(float speed, float maxTime, int damage, Vector3 direction)
    {
        Speed = speed;
        MaxTime = maxTime;
        Damage = damage;
        Direction = direction.normalized;
        _timer = maxTime;
        _directionSet = true;

        if (Direction != Vector3.zero)
        {
            Vector2 spriteForward = new Vector2(-1, 1);
            float angle = Vector2.SignedAngle(spriteForward, Direction);
            transform.rotation = Quaternion.Euler(0f, 0f, angle);
        }

        trail = GetComponentInChildren<ParticleSystem>();
    }

    protected virtual void Start()
    {
        if (!_directionSet)
        {
            Destroy(gameObject);
            return;
        }

        if (trail == null)
            trail = GetComponentInChildren<ParticleSystem>();
    }

    protected virtual void Update()
    {
        transform.position += Direction * Speed * Time.deltaTime;
        _timer -= Time.deltaTime;

        if (_timer <= 0f)
            OnLifetimeEnd();
    }

    protected abstract void OnHitEnemy(SnowflakeIA enemy);

    protected virtual void OnLifetimeEnd()
    {
        if (trail != null)
        {
            trail.transform.parent = null;
            trail.Stop();
            Destroy(trail.gameObject, trail.main.duration + trail.main.startLifetime.constantMax);
        }

        Destroy(gameObject);
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            SnowflakeIA enemy = other.GetComponent<SnowflakeIA>();
            if (enemy != null)
                OnHitEnemy(enemy);
        }
        else if (other.CompareTag("Environment"))
        {
            OnLifetimeEnd();
        }
    }
}
