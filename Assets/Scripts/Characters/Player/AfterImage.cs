using UnityEngine;

public class AfterImage : MonoBehaviour
{
    public float duration = 0.4f;
    private SpriteRenderer sr;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (sr == null) return;

        Color c = sr.color;
        c.a -= Time.deltaTime / duration;
        sr.color = c;

        if (c.a <= 0f)
            Destroy(gameObject);
    }
}
