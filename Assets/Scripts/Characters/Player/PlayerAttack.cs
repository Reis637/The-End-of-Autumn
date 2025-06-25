using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public GameObject currentProjectilePrefab;

    private float _lastAttackTime = -Mathf.Infinity;
    private PlayerAnim _anim;

    void Start()
    {
        _anim = GetComponent<PlayerAnim>();
    }

    void Update()
    {
        var stats = PlayerCore.Instance.Stats;
        var health = PlayerCore.Instance.Health;

        if (currentProjectilePrefab == null)
        {
            Debug.LogError("Projectile Prefab is not assigned in PlayerAttack!");
            return;
        }

        if (Input.GetMouseButton(0) && Time.time >= _lastAttackTime + stats.attackCooldown && !health.IsDead)
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

        GameObject projectileGO = Instantiate(currentProjectilePrefab, transform.position, Quaternion.identity);

        Projectile projectile = projectileGO.GetComponent<Projectile>();
        if (projectile != null)
        {
            projectile.Initialize(stats.projectileSpeed, stats.projectileLifetime, stats.attackDamage, direction);
        }
        else
        {
            Debug.LogError("El prefab asignado no tiene un componente BaseProjectile o derivado.");
            Destroy(projectileGO);
        }

        _anim.PlayAttack();
        _lastAttackTime = Time.time;
    }
}