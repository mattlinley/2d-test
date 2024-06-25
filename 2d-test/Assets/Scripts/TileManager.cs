using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileManager : MonoBehaviour
{
    public GameObject player;
    public Tilemap waterTilemap;
    public TileBase lakeShallowTile;
    public Tilemap grassTilemap;

    private Transform highlightWaterTile;
    private Transform highlightPlayerTile;
    public bool canFish; //TODO: valid fishing equipment and standing by water
    public bool showPlayerTile;

    private void Awake()
    {
        //get reference to water highlight sprite
        highlightWaterTile = transform.Find("HighlightWaterTile");
        highlightPlayerTile = transform.Find("HighlightPlayerTile");
    }

    private void Update()
    {
        //Check if we need to highlight water tiles for fishing
        if (canFish)
        {
            if (GetTileAtMousePosition(waterTilemap) == lakeShallowTile)
            {
                highlightWaterTile.position = GetTileWorldPositionFromMousePosition(waterTilemap);
                highlightWaterTile.gameObject.SetActive(true);
            }
            else
            {
                highlightWaterTile.gameObject.SetActive(false);
            }
        }
        else
        {
            highlightWaterTile.gameObject.SetActive(false);
        }


        //show tile player is standing on, this is currently just for debugging
        if (showPlayerTile)
        {
            highlightPlayerTile.position = GetTileWorldPositionFromPlayerPosition(grassTilemap);
            highlightPlayerTile.gameObject.SetActive(true);
        }
        else
        {
            highlightPlayerTile.gameObject.SetActive(false);
        }
        
    }

    public TileBase GetTileAtMousePosition(Tilemap tileMap)
    {
        //Get cell position from mouse position, via worldpoint
        Vector3Int tilePos = tileMap.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        return tileMap.GetTile(tilePos);
    }

    public TileBase GetTileAtPlayerPosition(Tilemap tileMap)
    {
        Vector3Int tilePos = tileMap.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        return tileMap.GetTile(tilePos);
    }

    public Vector3 GetTileWorldPositionFromCellPosition(Tilemap tileMap, Vector3Int cellPos)
    {
        Vector3 tilePos = tileMap.CellToWorld(cellPos);
        return GetAdjustedTileWorldPosition(tilePos);
    }

    public Vector3 GetTileWorldPositionFromMousePosition(Tilemap tileMap)
    {
        Vector3 tilePos = tileMap.CellToWorld(tileMap.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition)));
        return GetAdjustedTileWorldPosition(tilePos);
    }

    public Vector3 GetTileWorldPositionFromPlayerPosition(Tilemap tileMap)
    {
        Vector3 playerPosition = player.transform.position;
        playerPosition.y = playerPosition.y + 0.05f; //raise the point we are choosing the tile from slightly
        Vector3 tilePos = tileMap.CellToWorld(tileMap.WorldToCell(playerPosition));
        
        return GetAdjustedTileWorldPosition(tilePos);
    }

    /// <summary>
    /// CellToWorld does not provide the position a 16x16 gameobject needs to properly overlap the tile. This function
    /// does the required adjustments
    /// </summary>
    /// <param name="tilePos">Vector3 data from celltoworld call </param>
    /// <returns>Clean vector3 position of the tile</returns>
    private Vector3 GetAdjustedTileWorldPosition(Vector3 tilePos)
    {
        tilePos.x = Mathf.Ceil(tilePos.x) + 0.5f;
        tilePos.y = Mathf.Ceil(tilePos.y) + 0.5f;
        tilePos.z = 0;
        return tilePos;
    }
}
