using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Playables;

public class SaveManager : MonoBehaviour
{
    // Json Project Save Path
    private string jsonPathProject;
    // Json External/Real Save Path
    private string jsonPathPersistant;

    private string filename = "SaveGame";

    private PlayerController playerController;


    private void Awake()
    {
        playerController = FindObjectOfType<PlayerController>();
    }

    private void Start()
    {
        jsonPathProject = Application.dataPath + Path.AltDirectorySeparatorChar;
        jsonPathPersistant = Application.persistentDataPath + Path.AltDirectorySeparatorChar;
    }

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

    public void SaveGame(int slotNumber)
    {
        AllGameData data = new AllGameData();

        data.playerData = GetPlayerData();

        //data.environmentData = GetEnvironmentData();

        SaveGameDataToJsonFile(data, slotNumber);
    }

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

    private PlayerData GetPlayerData()
    {
        float playerEnergy = 0f;

        Vector3 playerPosition = playerController.rb.position;
        Vector2 playerDirection = playerController.facingDirection;


        return new PlayerData(playerEnergy, playerPosition, playerDirection);
    }
}
