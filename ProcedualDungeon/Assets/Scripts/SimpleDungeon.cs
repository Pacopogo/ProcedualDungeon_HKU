using UnityEngine;
using System.Collections.Generic;

public class SimpleDungeon : MonoBehaviour
{
    [SerializeField] private Vector2 mapSize = new Vector2(10,10);
    [SerializeField] private List<TileData> groundTiles;

    private List<GameObject> placedTiles = new List<GameObject>();

    private void SpawnGround()
    {
        for (int i = 0; i < mapSize.x; i++)
        {
            for(int j = 0; j < mapSize.y; j++)
            {
                GameObject obj = Instantiate(GetRandomWeightTile(groundTiles));
                obj.transform.position = new Vector3(i, 0, j);
                obj.transform.parent = transform;
                placedTiles.Add(obj);
            }
        } 
    }

    [ContextMenu("Spawn Objects")]
    public void Debug_SpawnGround()
    {
        ClearAllObj();
        SpawnGround();
    }

    [ContextMenu("CLEAR OBJECTS")]
    public void ClearAllObj()
    {
        foreach (GameObject obj in placedTiles)
        {
            if(obj != null)
                Destroy(obj);
        }
    }

    private int SumTileCost(List<TileData> data)
    {
        int sum = 0;
        foreach(TileData tile in data)
        {
            sum += tile.TileCost;
        }
        return sum;
    }

    private GameObject GetRandomWeightTile(List<TileData> data)
    {
        GameObject prefab = null;
        int rnd = Random.Range(0, SumTileCost(data));
    
        foreach(TileData tile in data)
        {
            rnd -= tile.TileCost;

            if(rnd < 0)
            {
                prefab = tile.Prefab;
                break;
            }
        }

        return prefab;
    }
}
