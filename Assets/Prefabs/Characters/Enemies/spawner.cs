using UnityEngine;

public class spawner : MonoBehaviour
{
    public GameObject prefab;

    public float spawnTime = 2f;
    private float timer;

    void Start()
    {
        timer = 0;
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer > spawnTime) {
            timer = 0;
            GameObject enemy = Instantiate(prefab, transform.position + new Vector3(0,0,-1), Quaternion.identity);
        }
    }
}
