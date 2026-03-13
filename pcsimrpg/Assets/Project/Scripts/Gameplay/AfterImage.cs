using UnityEngine;

public class AfterImage : MonoBehaviour
{
    public float lifeTime = 0.3f;

    private float timer;
    private SpriteRenderer sr;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        timer += Time.deltaTime;

        float alpha = Mathf.Lerp(0.7f, 0f, timer / lifeTime);
        sr.color = new Color(0f, 1f, 1f, alpha);

        if (timer >= lifeTime)
        {
            Destroy(gameObject);
        }
    }
}