using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BuildingSystem : MonoBehaviour
{
    //singletone pattern
    public static BuildingSystem current;

    //grid
    public GridLayout gridLayout;
    //indication tilemap - for checking placement availability
    public Tilemap MainTilemap;
    //tile to indicate its taken
    public TileBase takenTile;

    private void Awake()
    {
        //initialize singletone
        current = this;
    }

    #region Tilemap Management

    /*
     * Gets an array of tiles from the tilemap
     *
     * UNITY'S METHOD GetTilesBlock CAUSES EDITOR CRASH
     * (at least in my case)
     * 
     * BoundsInt area - tiles come from this area;
     * it has a position on the tilemap and size (2, 2, 1) etc.
     * Tilemap tilemap - tilemap from which we get tiles
     *  @returns an array of TilesBase
     *
     * Use this method when we check if area under the house is available
     * (white TileBase means available)
     */
    private static TileBase[] GetTilesBlock(BoundsInt area, Tilemap tilemap)
    {
        //create an array to store the tiles
        TileBase[] array = new TileBase[area.size.x * area.size.y * area.size.z];
        int counter = 0;
        
        //go through each position from the area
        foreach (var v in area.allPositionsWithin)
        {
            //store position and change z position to 0 - needed to get the right "layer" of tiles
            Vector3Int pos = new Vector3Int(v.x, v.y, 0);
            //get TileBase from that position
            array[counter] = tilemap.GetTile(pos);
            counter++;
        }
        
        return array;
    }

    /*
     * Sets TileBases on a tilemap
     * BoundsInt area - tiles set on this area;
     * it has a position on the tilemap and size (2, 2, 1) etc.
     * TileBase tileBase - which tiles to set
     * Tilemap tilemap - tilemap on which we set tiles
     */
    private static void SetTilesBlock(BoundsInt area, TileBase tileBase, Tilemap tilemap)
    {
        //create an array to store the tiles
        TileBase[] tileArray = new TileBase[area.size.x * area.size.y];
        //fill this array with TileBases of the chosen type
        FillTiles(tileArray, tileBase);
        //set the tiles on the tilemap
        tilemap.SetTilesBlock(area, tileArray);
    }

    /*
     * Fills an array of tiles with the chosen TileType
     */
    private static void FillTiles(TileBase[] arr, TileBase tileBase)
    {
        //go through each tile and set it
        for (int i = 0; i < arr.Length; i++)
        {
            arr[i] = tileBase;
        }
    }

    /*
     * Set an area (that the house was standing on) to empty
     */
    public void ClearArea(BoundsInt area, Tilemap tilemap)
    {
        //passing null as a tile base to set an empty tile
        SetTilesBlock(area, null, tilemap);
    }
    
    #endregion

    #region Building Placement

    
    public GameObject InitializeWithObject(GameObject building, Vector3 pos)
    {
        //calculate the position to place the object
        pos.z = 0;
        pos.y -= building.GetComponent<SpriteRenderer>().bounds.size.y / 2f;
        Vector3Int cellPos = gridLayout.WorldToCell(pos);
        Vector3 position = gridLayout.CellToLocalInterpolated(cellPos);

        //instantiate an object
        GameObject obj = Instantiate(building, position, Quaternion.identity);
        PlaceableObject temp = obj.transform.GetComponent<PlaceableObject>();
        temp.gameObject.AddComponent<ObjectDrag>();
        
        //return object instantiated
        return obj;
    }

    /*
     * Check if an area is available for placement
     * BoundsInt area - area to check
     */
    public bool CanTakeArea(BoundsInt area)
    {
        //get TileBases from the Main tilemap at this area
        TileBase[] baseArray = GetTilesBlock(area, MainTilemap);
        
        //check each TileBase
        foreach (var b in baseArray)
        {
            if (b == takenTile)
            {
                //taken - cannot place
                return false;
            }
        }

        return true;
    }

    /*
     * Take the area for a building
     */
    public void TakeArea(BoundsInt area)
    {
        //set tiles on the area to taken TileBase
        SetTilesBlock(area, takenTile, MainTilemap);
    }

    #endregion

    #region MapManagement

    public void LockTerritory(BoundsInt area)
    {
        //fill the area with white tile to indicate the area is unavailable
        SetTilesBlock(area, takenTile, MainTilemap);
    }

    public void UnlockTerritory(BoundsInt area)
    {
        //clear the area to make it available
        ClearArea(area, MainTilemap);
    }
    
    #endregion
}
