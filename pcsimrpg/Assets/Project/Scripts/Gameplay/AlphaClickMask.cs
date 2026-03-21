using UnityEngine;
using UnityEngine.UI;

public class AlphaClickMask : MonoBehaviour
{
    void Start()
    {
        // 0.1f means it will ignore anything more than 90% transparent
        GetComponent<Image>().alphaHitTestMinimumThreshold = 0.1f;
    }
}