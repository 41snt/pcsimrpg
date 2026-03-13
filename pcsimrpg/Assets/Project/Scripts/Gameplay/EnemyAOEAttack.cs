using UnityEngine;

public class EnemyAOEAttack : MonoBehaviour
{
    public Transform aoeCircle;
    public float chargeTime = 1.5f;
    public float maxSize = 3f;

    float timer;
    bool charging;

    void Start()
    {
        aoeCircle.localScale = Vector3.zero;
        aoeCircle.gameObject.SetActive(false);
    }

    void Update()
    {
        if (!charging) return;

        timer += Time.deltaTime;

        float progress = timer / chargeTime;
        float size = Mathf.Lerp(0f, maxSize, progress);

        aoeCircle.localScale = new Vector3(size, size, 1f);

        if (timer >= chargeTime)
        {
            Explode();
        }
    }

    public void StartAOE()
    {
        charging = true;
        timer = 0f;

        aoeCircle.localScale = Vector3.zero;
        aoeCircle.gameObject.SetActive(true);
    }

    void Explode()
    {
        charging = false;

        aoeCircle.localScale = Vector3.zero;
        aoeCircle.gameObject.SetActive(false);

        Debug.Log("AOE Attack!");
    }
}