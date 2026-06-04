using UnityEngine;

public class RAMMiniGame : MonoBehaviour
{
    public RectTransform signalA;
    public RectTransform signalB;

    public float speed = 2f;
    private bool active = false;

    void Update()
    {
        if (!active) return;

        float move = Mathf.PingPong(Time.time * speed, 200);

        signalA.anchoredPosition = new Vector2(move, 0);
        signalB.anchoredPosition = new Vector2(200 - move, 0);

        if (Input.GetMouseButtonDown(0))
        {
            if (Mathf.Abs(signalA.anchoredPosition.x - signalB.anchoredPosition.x) < 10f)
            {
                PCRepairManager.Instance.AddProgress(15f);
                Debug.Log("RAM synced!");
            }
        }
    }

    public void StartGame()
    {
        active = true;
    }
}