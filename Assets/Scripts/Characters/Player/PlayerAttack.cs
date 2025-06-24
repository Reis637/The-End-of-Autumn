using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public GameObject prefab;

    private float lastAttackTime = -Mathf.Infinity;
    private PlayerAnim anim;

    void Start()
    {
        anim = GetComponent<PlayerAnim>();
    }

    void Update()
    {
        var stats = PlayerCore.Instance.Stats;  
        var health = PlayerCore.Instance.Health;

        if (Input.GetMouseButton(0) && Time.time >= lastAttackTime + stats.attackCooldown && !health.IsDead)
        {
            Attack();
        }
    }

    void Attack()
    {
        var stats = PlayerCore.Instance.Stats;

        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0;
        Vector3 direction = (mouseWorldPos - transform.position).normalized;

        GameObject projectile = Instantiate(prefab, transform.position, Quaternion.identity);

        WindProjectile wp = projectile.GetComponent<WindProjectile>();
        if (wp != null)
        {
            wp.speed = stats.projectileSpeed;
            wp.maxTime = stats.projectileLifetime;
            wp.damage = stats.attackDamage;
            wp.SetDirection(direction);
        }

        anim.PlayAttack();
        lastAttackTime = Time.time;
    }
}
