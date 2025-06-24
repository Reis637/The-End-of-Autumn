using UnityEngine;
using System.Collections.Generic;

public class PlayerAnim : MonoBehaviour
{
    public Animator animator;
    private AnimatorOverrideController overrideController;

    public SpriteRenderer spriteRenderer;

    public AnimationClip walkUp;
    public AnimationClip walkDown;
    public AnimationClip attackUp;
    public AnimationClip attackDown;
    public AnimationClip death;

    private Dictionary<string, AnimationClip> animations;
    private string lastAnimName = "";

    private bool hasDied = false;
    private float attackAnimTimer = 0f;

    private PlayerStats stats;
    private PlayerHealth health;

    void Start()
    {
        stats = PlayerCore.Instance.Stats;
        health = PlayerCore.Instance.Health;

        overrideController = new AnimatorOverrideController(animator.runtimeAnimatorController);
        animator.runtimeAnimatorController = overrideController;

        animations = new Dictionary<string, AnimationClip>
        {
            { "walkUp", walkUp },
            { "walkDown", walkDown },
            { "attackUp", attackUp },
            { "attackDown", attackDown },
            { "death", death }
        };

        health.OnDeath += HandleDeath;
    }

    void Update()
    {
        if (hasDied) return;

        Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 dir = mouseWorldPos - (Vector2)transform.position;

        bool flip = dir.x < 0;
        spriteRenderer.flipX = flip;

        string vertical = dir.y >= 0 ? "Up" : "Down";

        if (attackAnimTimer > 0f)
        {
            attackAnimTimer -= Time.deltaTime;
            PlayAnim("attack" + vertical);
            return;
        }

        if (Input.GetMouseButton(0) && !health.IsDead)
        {
            attackAnimTimer = stats.attackCooldown;
            PlayAnim("attack" + vertical);
        }
        else
        {
            PlayAnim("walk" + vertical);
        }
    }

    public void PlayAttack()
    {
        if (hasDied) return;

        Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 dir = mouseWorldPos - (Vector2)transform.position;
        string vertical = dir.y >= 0 ? "Up" : "Down";
        PlayAnim("attack" + vertical);
    }

    void PlayAnim(string animName)
    {
        if (lastAnimName == animName) return;
        lastAnimName = animName;

        if (!animations.ContainsKey(animName)) return;
        overrideController[walkDown] = animations[animName];
    }

    void HandleDeath()
    {
        if (hasDied) return;
        hasDied = true;

        overrideController[walkDown] = animations["death"];
        animator.Play("Current", 0, 0);

        CameraMovement camScript = Camera.main.GetComponent<CameraMovement>();
        if (camScript != null) { 
            camScript.FocusOnlyOnTarget(true);
        }

        StartCoroutine(DeathRoutine());
    }

    private System.Collections.IEnumerator DeathRoutine()
    {
        yield return new WaitForSeconds(animations["death"].length);
        gameObject.SetActive(false);
    }
}
