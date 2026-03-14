using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEditor;
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

    public List<GameObject> tiles = new List<GameObject>();

    private Vector2Int[] directions = new Vector2Int[]
    {
        new Vector2Int(1, 0),
        new Vector2Int(-1, 0),
        new Vector2Int(0, 1),
        new Vector2Int(0, -1)
    };


    [ContextMenu("Place Tiles")]
    public void PlaceRandomTiles()
    {
        Random.InitState(worldSeed);

        for (int x = 0; x < worldSize; x++)
        {
            GameObject obj = Instantiate(prefab);

            obj.transform.position = new Vector2(origin.x, origin.y);
            tiles.Add(obj);

            NewOrigin();
        }

        SetRooms();
    }

    private void SetRooms()
    {
        tiles[0].GetComponent<Tile>().SetType(tileType.Start);
        GetFurthersFromSpot(tiles[0].transform).SetType(tileType.Finish);

        GetFurthersFromSpot(tiles[tiles.Count - 1].transform).SetType(tileType.Treasure);

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

    private Vector2Int RandomVector()
    {
        int rnd = Random.Range(0, directions.Length);

        return directions[rnd];
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
        foreach (var tile in tiles)
        {
            if (tile.GetComponent<Tile>().isTyped)
                continue;

            rooms.Add(tile.GetComponent<Tile>());
        }

        for (int i = 0; i < treasureCount; i++)
        {
            int rnd = Random.Range(0, rooms.Count);

            rooms[rnd].SetType(tileType.Treasure);
        }
    }

    private Tile GetFurthersFromSpot(Transform origin)
    {
        Tile furtherst = null;
        Transform current = null;
        Vector3 distance = Vector3.zero;
        Vector3 previous = Vector3.zero;

        foreach (var tile in tiles)
        {
            current = tile.transform;
            distance = current.position - origin.position;

            if (distance.magnitude < previous.magnitude)
                continue;

            previous = distance;
            furtherst = tile.GetComponent<Tile>();
        }

        return furtherst;
    }

    private List<Tile> GetNeigbours(Transform origin)
    {
        List<Tile> neigbours = new List<Tile>();



        foreach (var tile in tiles)
        {

        }

        return neigbours;

    }
}
