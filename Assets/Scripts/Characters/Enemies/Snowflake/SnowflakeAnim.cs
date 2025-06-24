using System.Collections.Generic;
using UnityEngine;

public class SnowflakeAnim : MonoBehaviour
{
    public Animator animator;

    public void PlayDeath()
    {
        animator.Play("snowflakeDeath");
    }
}
