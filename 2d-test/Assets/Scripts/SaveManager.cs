using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class SaveManager : MonoBehaviour
{
    // Json Project Save Path
    private string jsonPathProject;
    // Json External/Real Save Path
    private string jsonPathPersistant;

    private string filename = "SaveGame";

    private PlayerController playerController;
    private UIManager uiManager;

    private void Awake()
    {
        playerController = FindObjectOfType<PlayerController>();
        uiManager = FindObjectOfType<UIManager>();
    }

    private void Start()
    {
        jsonPathProject = Application.dataPath + Path.AltDirectorySeparatorChar;
        jsonPathPersistant = Application.persistentDataPath + Path.AltDirectorySeparatorChar;
    }

    /// <summary>
    /// Checks for a file on disk in expected location for slot
    /// </summary>
    /// <param name="slotNumber">save game slot (1-3)</param>
    /// <returns>true if no file exists (and therefore slot is empty)</returns>
    public bool IsSlotEmpty(int slotNumber)
    {
        if (System.IO.File.Exists(jsonPathProject + filename + slotNumber + ".json"))
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    /// <summary>
    /// creates data object to pass to the file writing function
    /// </summary>
    /// <param name="slotNumber">save game slot (1-3)</param>
    public void SaveGame(int slotNumber)
    {
        AllGameData data = new AllGameData();

        data.playerData = GetPlayerData();

        //data.environmentData = GetEnvironmentData();

        SaveGameDataToJsonFile(data, slotNumber);
    }

    /// <summary>
    /// converts game data to json and writes to file
    /// </summary>
    /// <param name="gameData">game data object</param>
    /// <param name="slotNumber">save game slot (1-3)</param>
    public void SaveGameDataToJsonFile(AllGameData gameData, int slotNumber)
    {
        string json = JsonUtility.ToJson(gameData);
        //string encrypted = EncryptionDecryption(json);


        using (StreamWriter writer = new StreamWriter(jsonPathProject + filename + slotNumber + ".json"))
        {
            writer.Write(json);
            Debug.Log("Saved to json file at: " + jsonPathProject + filename + slotNumber + ".json");
        }
    }

    /// <summary>
    /// collect all data relating to the player state and put in playerData object
    /// </summary>
    /// <returns>a PlayerData object with all relevant data in it</returns>
    private PlayerData GetPlayerData()
    {
        float playerEnergy = 0f;

        Vector3 playerPosition = playerController.rb.position;
        Vector2 playerDirection = playerController.facingDirection;


        return new PlayerData(playerEnergy, playerPosition, playerDirection);
    }

    /// <summary>
    /// Start the game load - switch to game scene if we are in another scene (eg main menu) or close the UI screen if we are loading from pause menu.
    /// </summary>
    /// <param name="slotNumber">save game slot (1-3)</param>
    public void StartLoadedGame(int slotNumber)
    {
        //ActivateLoadingScreen();
        
        playerController.GameLoading = true;

        if (SceneManager.GetActiveScene().name != "GameScene")
        {
            SceneManager.LoadScene("GameScene");
        }
        else
        {
            //close UI screens
            uiManager.CloseUI();
        }
        //isLoading = true;
        StartCoroutine(DelayedLoading(slotNumber));
    }

    /// <summary>
    /// delay loading the save data to enable time for scene to be switched if necessary
    /// </summary>
    /// <param name="slotNumber">save game slot (1-3)</param>
    /// <returns></returns>
    private IEnumerator DelayedLoading(int slotNumber)
    {
        yield return new WaitForSeconds(0.2f);

        LoadGame(slotNumber);

    }


    /// <summary>
    /// Get all game data and send to functions for setting player data and environment data.
    /// </summary>
    /// <param name="slotNumber">save game slot (1-3)</param>
    public void LoadGame(int slotNumber)
    {
        //Player data
        AllGameData gameData = LoadGameDataFromJsonFile(slotNumber);

        SetPlayerData(gameData.playerData);

        //Enviroment data
        //SetEnvironmentData(LoadingTypeSwitch(slotNumber).environmentData);


        //enable character to move once loading completed.
        playerController.GameLoading = false;

        //DisableLoadingScreen();

    }

    /// <summary>
    /// take all player data from the save and apply it to our player character
    /// </summary>
    /// <param name="playerData">all player data that was saved</param>
    private void SetPlayerData(PlayerData playerData)
    {
        playerController.rb.position = playerData.playerPosition;
        playerController.facingDirection = playerData.playerDirection;
    }

    /// <summary>
    /// load save game data from file and convert from json to allgamedata object
    /// </summary>
    /// <param name="slotNumber">save game slot (1-3)</param>
    /// <returns>all game data in object</returns>
    public AllGameData LoadGameDataFromJsonFile(int slotNumber)
    {
        using (StreamReader reader = new StreamReader(jsonPathProject + filename + slotNumber + ".json"))
        {
            string json = reader.ReadToEnd();

            //string decrypted = EncryptionDecryption(json);

            return JsonUtility.FromJson<AllGameData>(json);
        }

    }


}
