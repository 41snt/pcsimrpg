using UnityEngine;
using UnityEngine.UI;

public class SaveSlotUI : MonoBehaviour
{
    public int slotNumber;

    public Text slotText;
    public GameObject actionPanel;

    private static SaveSlotUI selectedSlot;

    private void Start()
    {
        actionPanel.SetActive(false);
        Refresh();
    }

    public void OnSlotClicked()
    {
        if (selectedSlot != null && selectedSlot != this)
        {
            selectedSlot.HideActions();
        }

        selectedSlot = this;
        actionPanel.SetActive(true);
    }

    public void HideActions()
    {
        actionPanel.SetActive(false);
    }

    public void Refresh()
    {
        if (SaveSystem.SaveExists(slotNumber))
        {
            slotText.text = "Slot " + slotNumber + "\nSaved Data";
        }
        else
        {
            slotText.text = "Slot " + slotNumber + "\nEmpty";
        }
    }

    public void LoadSlot()
    {
        PlayerPrefs.SetInt("CurrentSaveSlot", slotNumber);
        UnityEngine.SceneManagement.SceneManager.LoadScene("SampleScene");
    }

    public void DeleteSlot()
    {
        SaveSystem.DeleteSave(slotNumber);
        Refresh();
        actionPanel.SetActive(false);
    }
}