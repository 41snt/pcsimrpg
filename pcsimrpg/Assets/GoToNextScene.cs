using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GoToNextScene : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(NextScene());
    }

    IEnumerator NextScene()
    {
        yield return new WaitForSeconds(22.3f);

        SceneManager.LoadScene("SampleScene");
    }
}