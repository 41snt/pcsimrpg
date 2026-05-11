using UnityEngine;

public class LoadPlayerPosition : MonoBehaviour
{
    private void Start()
    {
        if (PlayerPrefs.HasKey("PlayerX"))
        {
            float x = PlayerPrefs.GetFloat("PlayerX");
            float y = PlayerPrefs.GetFloat("PlayerY");

            transform.position = new Vector3(x, y, transform.position.z);
        }
    }
}