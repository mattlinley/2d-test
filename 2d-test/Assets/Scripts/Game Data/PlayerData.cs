using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public float playerEnergy;
    public Vector3 playerPosition; // position x,y,z rotation x,y,z
    public Vector2 playerDirection; // 0: North; 1: East; 2: south; 3: west
    //public string[] inventoryContent;


    public PlayerData(float _playerEnergy, Vector3 _playerPosition, Vector2 _playerDirection)
    {
        playerEnergy = _playerEnergy;
        playerPosition = _playerPosition;
        playerDirection = _playerDirection;
    }
}
