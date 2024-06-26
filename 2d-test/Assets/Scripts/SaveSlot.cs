using System;
using UnityEngine;
using UnityEngine.UI;

public class SaveSlot : MonoBehaviour
{
    private Button button;
    private Text buttonText;

    private SaveManager saveManager;
    private UIManager uiManager;

    public int slotNumber;

    public GameObject alertUI;
    Button yesButton;
    Button noButton;

    private void Awake()
    {
        saveManager = FindObjectOfType<SaveManager>();
        uiManager = FindObjectOfType<UIManager>();

        button = GetComponent<Button>();
        buttonText = transform.GetChild(0).GetComponent<Text>();

        yesButton = alertUI.transform.Find("Button_Yes").GetComponent<Button>();
        noButton = alertUI.transform.Find("Button_No").GetComponent<Button>();
    }

    private void Start()
    {
        button.onClick.AddListener(() =>
        {
            if (saveManager.IsSlotEmpty(slotNumber))
            {
                SaveGameConfirmed();
            }
            else
            {
                DisplayOverwriteAlert();
            }
        });
    }

    private void Update()
    {
        if (saveManager.IsSlotEmpty(slotNumber))
        {
            buttonText.text = "Slot " + slotNumber + "(Empty)";
        }
        else
        {
            Debug.Log("Save file exists in slot " + slotNumber.ToString());
            buttonText.text = PlayerPrefs.GetString("Slot" + slotNumber + "Description");
        }
    }

    public void DisplayOverwriteAlert()
    {
        alertUI.SetActive(true);

        yesButton.onClick.AddListener(() =>
        {
            SaveGameConfirmed();
            alertUI.SetActive(false);
        });

        noButton.onClick.AddListener(() =>
        {
            alertUI.SetActive(false);
        });
    }

    private void SaveGameConfirmed()
    {
        saveManager.SaveGame(slotNumber);

        DateTime dt = DateTime.Now;
        string time = dt.ToString("yyyy-MM-dd HH:mm");

        string description = "Saved Game " + slotNumber + " | " + time;

        buttonText.text = description;
        PlayerPrefs.SetString("Slot" + slotNumber + "Description", description);
        PlayerPrefs.Save();

        uiManager.DeselectButtons();
    }
}
