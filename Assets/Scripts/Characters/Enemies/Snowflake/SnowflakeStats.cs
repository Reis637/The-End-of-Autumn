using UnityEngine;

public class SnowflakeStats : MonoBehaviour
{
    public int maxHP = 10;
    public float moveSpeed = 1.5f;
    public int damage = 5;

    [HideInInspector] public int currentHP;

    void Start()
    {
        currentHP = maxHP;
    }
}