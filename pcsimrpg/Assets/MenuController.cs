using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    public GameObject menuCanvas;
    public GameObject gameplayUI;

    void Start()
    {
        menuCanvas.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            bool isOpen = !menuCanvas.activeSelf;

            menuCanvas.SetActive(isOpen);

            // Hide gameplay UI when menu is open
            gameplayUI.SetActive(!isOpen);
        }
    }
}