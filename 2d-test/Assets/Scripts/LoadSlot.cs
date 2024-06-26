using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadSlot : MonoBehaviour
{
    private Button button;
    private Text buttonText;

    private SaveManager saveManager;
    private UIManager uiManager;

    public int slotNumber;

    private void Awake()
    {
        saveManager = FindObjectOfType<SaveManager>();
        uiManager = FindObjectOfType<UIManager>();

        button = GetComponent<Button>();
        buttonText = transform.GetChild(0).GetComponent<Text>();
    }

    private void Update()
    {
        if (saveManager.IsSlotEmpty(slotNumber))
        {
            buttonText.text = "Slot " + slotNumber + " (Empty)";
        }
        else
        {
            buttonText.text = PlayerPrefs.GetString("Slot" + slotNumber + "Description");
        }
    }

    private void Start()
    {
        button.onClick.AddListener(() =>
        {
            if (saveManager.IsSlotEmpty(slotNumber) == false)
            {
                saveManager.StartLoadedGame(slotNumber);
                uiManager.DeselectButtons();
            }
            else
            {
                //if empty do nothing
            }
        });
    }



}
