using System.Collections.Generic;
using UnityEngine;

public class SnowflakeAnim : MonoBehaviour
{
    public Animator animator;
    private AnimatorOverrideController overrideController;

    public SpriteRenderer spriteRenderer;
    public AnimationClip idle;
    public AnimationClip death;

    private Dictionary<string, AnimationClip> animations;

    void Start()
    {
        overrideController = new AnimatorOverrideController(animator.runtimeAnimatorController);
        animator.runtimeAnimatorController = overrideController;

        animations = new Dictionary<string, AnimationClip>
        {
            { "idle", idle },
            { "death", death }
        };
    }

    public float PlayDeath()
    {
        overrideController[idle] = animations["death"];
        animator.Play("Current", 0, 0);
        return animations["death"].length;
    }
}
