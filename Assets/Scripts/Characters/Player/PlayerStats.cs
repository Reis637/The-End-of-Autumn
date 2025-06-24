using UnityEngine;
public class PlayerStats : MonoBehaviour
{
    [Header("Health")]
    public int maxHP = 100;

    [Header("Movement")]
    public float moveSpeed = 5f;

    [Header("Attack")]
    public float attackCooldown = 0.5f;
    public int attackDamage = 10;
    public float projectileSpeed = 5f;
    public float projectileLifetime = 2f;

    [Header("Knockback")]
    public float knockbackDistance = 1f;
    public float pushDuration = 0.2f;

    [Header("Damage")]
    public float immunityDuration = 0.5f;
}
