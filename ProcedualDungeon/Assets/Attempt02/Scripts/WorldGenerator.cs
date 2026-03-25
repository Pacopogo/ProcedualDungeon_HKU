using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using TMPro;
using UnityEngine;

public class WorldGenerator : MonoBehaviour
{

    [Header("Camera")]
    [SerializeField] private Transform cameraTrans;
    [SerializeField] private float cameraStepBack = -2f;
    [SerializeField] private float cameraOffsetPercentage = 0.8f; //numbs between 0 and 1 as the %

    [Header("Seed")]
    [SerializeField] private TMP_Text seedText;
    [SerializeField] private Vector2Int seedSize = new Vector2Int(-10000, 10000);

    [Header("Prefabs")]
    [SerializeField] private GameObject prefab;

    [Header("World Settings")]
    public float generationSpeed = 0.1f;
    public int worldSeed = 42;
    public int worldSize = 16;

    [Header("Tile settings")]
    public int amountCombat = 1;

    private Vector2Int origin = Vector2Int.zero;

    public Dictionary<Vector2Int, GameObject> tiles = new Dictionary<Vector2Int, GameObject>();

    private Vector2Int[] directions = new Vector2Int[]
    {
        new Vector2Int(1, 0),
        new Vector2Int(-1, 0),
        new Vector2Int(0, 1),
        new Vector2Int(0, -1)
    };

    private bool isBuilding;

    [Header("Tile Types")]
    [SerializeField] private TileType StartType;
    [SerializeField] private TileType FinishType;
    [SerializeField] private TileType TreasureType;
    [SerializeField] private TileType DangerType;
    [SerializeField] private TileType PuzzleType;
    [SerializeField] private TileType CombatType;

    private void Start()
    {
        seedText.text = worldSeed.ToString();
    }
    public void PlaceRandomSeed()
    {
        worldSeed = Random.Range(seedSize.x, seedSize.y);
        Replace();
    }

    [ContextMenu("Place Tiles")]
    public void PlaceRandomTiles()
    {
        if (isBuilding)
            return;

        isBuilding = true;

        Random.InitState(worldSeed);
        seedText.text = worldSeed.ToString();

        for (int x = 0; x < worldSize; x++)
        {
            GameObject obj = Instantiate(prefab);

            obj.transform.position = new Vector2(origin.x, origin.y);
            tiles.Add(origin, obj);

            obj.transform.parent = transform;

            obj.SetActive(false);

            NewOrigin();
        }

        SetRooms();



        if (generationSpeed > 0)
        {
            StartCoroutine(GenerateTiles());
        }
        else
        {
            foreach (var tile in tiles)
            {
                tile.Value.gameObject.SetActive(true);
            }

            isBuilding = false;
        }

    }

    private IEnumerator GenerateTiles()
    {
        foreach (var tile in tiles)
        {
            tile.Value.gameObject.SetActive(true);
            yield return new WaitForSeconds(generationSpeed);
        }

        isBuilding = false;
        yield return null;
    }

    private void SetRooms()
    {

        //Set Finish as furtherst room from the first placed tile
        Tile finish = GetFurthersFromSpot(tiles[Vector2Int.zero].transform);
        finish.SetType(FinishType);

        //Set Start as furtherst room from Finish
        Tile Start = GetFurthersFromSpot(finish.transform);
        Start.SetType(StartType);

        //Set treasure 
        Tile Treasure = GetMiddleTile(Start.transform, finish.transform);
        Treasure.SetType(TreasureType);

        //Set danger rooms around treasure room
        foreach (var tile in GetNeigbours(Treasure.transform))
        {
            tile.SetType(DangerType);
        }

        //Dead ends have Puzzle tiles
        foreach (var tile in tiles)
        {
            //the <= is just if to see if it has only 1 tile attached to this current tile
            if (GetNeigbours(tile.Value.transform).Count <= 1)
                tile.Value.GetComponent<Tile>().SetType(PuzzleType);
        }

        //Add random combat rooms
        if(amountCombat > 0)
        {
            for (int i = 0; i < amountCombat; i++)
            {
                AssignRandomRoom(CombatType);
            }
        }

        cameraTrans.position = Treasure.transform.position;
        float zoomOffset = Vector3.Distance(Start.transform.position, finish.transform.position) * cameraOffsetPercentage;
        cameraTrans.Translate(Vector3.forward * cameraStepBack * zoomOffset);

    }

    private void NewOrigin()
    {
        Vector2Int rndDir = RandomVector();

        while (IsOccupied(origin))
        {
            origin += rndDir;
        }
    }

    private Vector2Int RandomVector() => directions[Random.Range(0, directions.Length)];

    private bool IsOccupied(Vector2Int pos) => tiles.ContainsKey(pos);

    [ContextMenu("Replace")]
    public void Replace()
    {
        if (isBuilding)
            return;

        foreach (var tile in tiles)
        {
            Destroy(tile.Value.gameObject);
        }
        tiles.Clear();
        
        origin = Vector2Int.zero;

        PlaceRandomTiles();
    }

    /// <summary>
    /// This function gets the futherst tile from the given transform
    /// </summary>
    /// <param name="origin"></param>
    /// <returns></returns>
    private Tile GetFurthersFromSpot(Transform origin)
    {
        Tile furtherst = null;
        Transform current = null;
        Vector3 distance = Vector3.zero;
        Vector3 previous = Vector3.zero;

        foreach (var tile in tiles)
        {
            current = tile.Value.transform;
            distance = current.position - origin.position;

            if (distance.magnitude <= previous.magnitude)
                continue;

            previous = distance;
            furtherst = tile.Value.GetComponent<Tile>();
        }

        return furtherst;
    }

    private Tile GetMiddleTile(Transform posA, Transform posB)
    {
        Tile middleTile = tiles[Vector2Int.zero].GetComponent<Tile>();

        Vector3 middleVec = posB.position + (posA.position - posB.position) * 0.5f;

        float distance;
        float previous = float.MaxValue;

        foreach (var tile in tiles)
        {
            distance = Vector3.Distance(middleVec, tile.Value.transform.position);

            if (distance > previous)
                continue;

            previous = distance;
            middleTile = tile.Value.GetComponent<Tile>();
        }

        return middleTile;
    }

    private List<Tile> GetNeigbours(Transform origin)
    {
        List<Tile> neigbours = new List<Tile>();

        for (int i = 0; i < directions.Length; i++)
        {
            Vector2Int currentPos = new Vector2Int(Mathf.RoundToInt(origin.position.x), Mathf.RoundToInt(origin.position.y)) + directions[i];

            GameObject tile;
            if (tiles.TryGetValue(currentPos, out tile))
            {
                neigbours.Add(tile.GetComponent<Tile>());
            }
        }

        return neigbours;
    }

    /// <summary>
    /// Assigns truly random rooms that aren't typed
    /// </summary>
    /// <param name="type"></param>
    private void AssignRandomRoom(TileType type)
    {
        List<Vector2Int> keys = tiles.Keys.ToList();
        
        tiles.TryGetValue(keys[Random.Range(0, keys.Count)], out GameObject tileObj);

        while (tileObj.GetComponent<Tile>().isTyped)
        {
            tiles.TryGetValue(keys[Random.Range(0, keys.Count)], out tileObj);
        }
        tileObj.GetComponent<Tile>().SetType(type);
    }
}
