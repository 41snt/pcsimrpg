using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryTabs : MonoBehaviour {
    public GameObject statsPanel;
    public GameObject inventoryPanel;
    public GameObject questPanel;
    public GameObject settingsPanel;
    public Text panelLabel;

    void HideAll() {
        statsPanel.SetActive(false);
        inventoryPanel.SetActive(false);
        questPanel.SetActive(false);
        settingsPanel.SetActive(false);
    }

    public void OpenStats() {
        HideAll();
        statsPanel.SetActive(true);
        panelLabel.text = "STATS TAB";
    }

    public void OpenInventory() {
        HideAll();
        inventoryPanel.SetActive(true);
        panelLabel.text = "INVENTORY TAB";
    }

    public void OpenQuest() {
        HideAll();
        questPanel.SetActive(true);
        panelLabel.text = "QUEST TAB";
    }

    public void OpenSettings() {
        HideAll();
        settingsPanel.SetActive(true);
        panelLabel.text = "SETTINGS TAB";
    }
}
