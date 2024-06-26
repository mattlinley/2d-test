using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UIManager : MonoBehaviour
{
    public GameObject inventory;
    public GameObject pauseMenu;
    public GameObject settingsMenu;
    public GameObject saveMenu;
    public GameObject loadMenu;
    [field: SerializeField] public bool UIOpen {  get; private set; }
    [field: SerializeField] public bool InventoryOpen { get; private set; }
    [field: SerializeField] public bool InSubMenu { get; set; }
    [field: SerializeField] public bool IsPaused { get; private set; }  // there may be a better place to put this in future

    public void OpenInventoryScreen()
    {
        inventory.SetActive(true);
        InventoryOpen = true;

        UIOpen = true;

    }

    public void OpenPauseMenu()
    {
        //close any submenus in case we come from here
        settingsMenu.SetActive(false);
        saveMenu.SetActive(false);
        loadMenu.SetActive(false);
        InSubMenu = false;

        pauseMenu.SetActive(true);
        IsPaused = true;

        UIOpen=true;
    }

    public void CloseUI()
    {
        inventory.SetActive(false);
        InventoryOpen=false;

        pauseMenu.SetActive(false);
        settingsMenu.SetActive(false);
        saveMenu.SetActive(false);
        loadMenu.SetActive(false);
        IsPaused = false;
        InSubMenu = false;

        UIOpen = false;

    }

    public void QuitProgram()
    {
        Application.Quit();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            //only open if no UI already open
            if (!UIOpen)
            {
                OpenInventoryScreen();
            }
            else if (InventoryOpen)
            {
                //close if inventory screen already open
                CloseUI();
            }

        }

        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyUp(KeyCode.X)) //X for using in Unity as escape leaves game window
        {
            //escape should open pause menu if no menu already open, and close all UI if open
            if (!UIOpen)
            {
                OpenPauseMenu();
            }
            else if (InSubMenu)
            {
                //in a sub menu eg settings so we want escape to return us to the pause menu
                OpenPauseMenu();
            }
            else
            {
                //close all UI windows on escape
                CloseUI();
            }
            
        }

    }

    public void DeselectButtons()
    {
        GameObject myEventSystem = GameObject.Find("EventSystem");
        myEventSystem.GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(null);
    }
}
