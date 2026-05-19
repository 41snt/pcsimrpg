using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueController : MonoBehaviour
{
<<<<<<< Updated upstream
    public static DialogueController Instance { get; private set; } // Singleton Instance
=======
    public static DialogueController Instance
    {
        get;
        private set;
    }
>>>>>>> Stashed changes

    [Header("UI")]
    public GameObject dialoguePanel;

    public TMP_Text dialogueText;

    public TMP_Text nameText;

    public Image portraitImage;
<<<<<<< Updated upstream
=======

    public Transform choiceContainer;

    public GameObject choiceButtonPrefab;
>>>>>>> Stashed changes

    private void Awake()
    {
<<<<<<< Updated upstream
        if (Instance == null) Instance = this;
        else Destroy(gameObject); // Make sure only one instance
=======
        Instance = this;
>>>>>>> Stashed changes
    }

    private void Start()
    {
        RebindReferences();
    }

    // =========================
    // REBIND UI
    // =========================

    public void RebindReferences()
    {
        // PANEL
        if (dialoguePanel == null)
        {
            dialoguePanel =
                GameObject.Find(
                    "DialoguePanel");
        }

        // CHOICE CONTAINER
        if (choiceContainer == null)
        {
            GameObject obj =
                GameObject.Find(
                    "ChoiceContainer");

            if (obj != null)
            {
                choiceContainer =
                    obj.transform;
            }
        }

        // BUTTON PREFAB
        if (choiceButtonPrefab == null)
        {
            GameObject prefab =
                Resources.Load<GameObject>(
                    "ChoiceButton");

            if (prefab != null)
            {
                choiceButtonPrefab =
                    prefab;
            }
        }
    }

    // =========================
    // SHOW UI
    // =========================

    public void ShowDialogueUI(bool show)
    {
<<<<<<< Updated upstream
        dialoguePanel.SetActive(show); // Toggle UI visibility
=======
        if (dialoguePanel == null)
        {
            RebindReferences();
        }

        if (dialoguePanel == null)
        {
            Debug.LogError(
                "DialoguePanel missing.");

            return;
        }

        dialoguePanel.SetActive(show);
>>>>>>> Stashed changes
    }

    // =========================
    // NPC INFO
    // =========================

    public void SetNPCInfo(
        string npcName,
        Sprite portrait)
    {
        if (nameText != null)
        {
            nameText.text =
                npcName;
        }

        if (portraitImage != null)
        {
            portraitImage.sprite =
                portrait;
        }
    }

    // =========================
    // DIALOGUE TEXT
    // =========================

    public void SetDialogueText(
        string text)
    {
        if (dialogueText != null)
        {
            dialogueText.text =
                text;
        }
    }
<<<<<<< Updated upstream
=======

    // =========================
    // CLEAR CHOICES
    // =========================

    public void ClearChoices()
    {
        if (choiceContainer == null)
        {
            RebindReferences();
        }

        if (choiceContainer == null)
            return;

        foreach (Transform child
            in choiceContainer)
        {
            Destroy(child.gameObject);
        }
    }

    // =========================
    // CREATE BUTTON
    // =========================

    public GameObject CreateChoiceButton(
        string choiceText,
        UnityEngine.Events.UnityAction onClick)
    {
        if (choiceContainer == null)
        {
            RebindReferences();
        }

        if (choiceButtonPrefab == null)
        {
            RebindReferences();
        }

        if (choiceContainer == null)
        {
            Debug.LogError(
                "ChoiceContainer missing.");

            return null;
        }

        if (choiceButtonPrefab == null)
        {
            Debug.LogError(
                "ChoiceButtonPrefab missing.");

            return null;
        }

        GameObject button =
            Instantiate(
                choiceButtonPrefab,
                choiceContainer);

        TMP_Text text =
            button.GetComponentInChildren<TMP_Text>();

        if (text != null)
        {
            text.text =
                choiceText;
        }

        Button btn =
            button.GetComponent<Button>();

        if (btn != null)
        {
            btn.onClick.AddListener(
                onClick);
        }

        return button;
    }
>>>>>>> Stashed changes
}