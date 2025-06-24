using UnityEngine;

[RequireComponent(typeof(PlayerStats))]
[RequireComponent(typeof(PlayerHealth))]
[RequireComponent(typeof(PlayerKnockback))]
public class PlayerCore : MonoBehaviour
{
    public static PlayerCore Instance { get; private set; }

    public PlayerStats Stats { get; private set; }
    public PlayerHealth Health { get; private set; }
    public PlayerKnockback Knockback { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;    

        Stats = GetComponent<PlayerStats>();
        Health = GetComponent<PlayerHealth>();
        Knockback = GetComponent<PlayerKnockback>();

        Health.OnDeath += HandleDeath;
    }

    void HandleDeath()
    {
        Debug.Log("PlayerCore: el jugador ha muerto.");
    }
}
