using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UIElements;

public class WorldGenerator : MonoBehaviour
{
    [SerializeField] private GameObject prefab;

    [Header("World Settings")]
    [SerializeField] private int worldSeed = 42;
    [SerializeField] private int worldSize = 4;

    private Vector2Int origin = Vector2Int.zero;

    [Header("Map Settings")]
    [SerializeField] private int treasureCount = 1;
    [SerializeField] private int dangerCount = 1;

    [SerializeField] private List<GameObject> tiles = new List<GameObject>();

    private void Start()
    {
        Random.InitState(worldSeed);
    }

    [ContextMenu("PRINT RND")]
    public void RandomNumbs()
    {
        Random.InitState(worldSeed);

        int rnd = 0;
        for (int i = 0; i < worldSeed; i++)
        {
            rnd = Random.Range(0, 10000);
            Debug.Log(rnd);
        }
    }

    [ContextMenu("Place Tiles")]
    public void PlaceRandomTiles()
    {
        Random.InitState(worldSeed);

        for (int x = 0; x < worldSize; x++)
        {
            for (int y = 0; y < worldSize; y++)
            {
                GameObject obj = Instantiate(prefab);

                if (y == 0 && x == 0)
                    obj.GetComponent<Tile>().SetType(tileType.Start);

                obj.transform.position = new Vector2(origin.x, origin.y);
                tiles.Add(obj);

                NewOrigin();
            }
        }

        tiles[tiles.Count - 1].GetComponent<Tile>().SetType(tileType.Finish);
    }

    private Vector2Int RandomVector()
    {
        int rnd = Random.Range(0, 4);

        switch (rnd)
        {
            case 0:
                return Vector2Int.up;
            case 1:
                return Vector2Int.down;
            case 2:
                return Vector2Int.right;
            case 3:
                return Vector2Int.left;
        }

        return Vector2Int.up;
    }

    private bool IsOccupied(Vector2 pos)
    {

        foreach (var tile in tiles)
        {
            if (tile.transform.position.x == pos.x && tile.transform.position.y == pos.y)
            {
                return true;
            }
        }

        return false;
    }

    private void NewOrigin()
    {
        Vector2Int o = origin;

        origin += RandomVector();

        if (IsOccupied(origin))
        {
            origin = o;
            NewOrigin();
        }
    }

    [ContextMenu("Replace")]
    private void Replace()
    {
        foreach (var tile in tiles)
        {
            Destroy(tile.gameObject);
        }
        tiles.Clear();

        origin = Vector2Int.zero;

        PlaceRandomTiles();
    }

    [ContextMenu("Generate interesting Rooms")]
    private void PlaceRooms()
    {
        Random.InitState(worldSeed);

        List<Tile> rooms = new List<Tile>();
        foreach(var tile in tiles)
        {
            if(tile.GetComponent<Tile>().isTyped)
                continue;

            rooms.Add(tile.GetComponent<Tile>());
        }

        for (int i = 0; i < treasureCount; i++)
        {
            int rnd = Random.Range(0, rooms.Count);

            rooms[rnd].SetType(tileType.Treasure);
        }
    }
}
