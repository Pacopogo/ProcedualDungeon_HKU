using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WorldGenerator : MonoBehaviour
{
    [SerializeField] private Transform cameraTrans;
    [SerializeField] private float cameraStepBack = -14f;

    [SerializeField] private TMP_Text seedText;

    public float generationSpeed = 0.1f;

    [SerializeField] private GameObject prefab;

    [Header("World Settings")]
    public int worldSeed = 42;
    [SerializeField] private int worldSize = 4;

    private Vector2Int origin = Vector2Int.zero;

    public List<GameObject> tiles = new List<GameObject>();

    private Vector2Int[] directions = new Vector2Int[]
    {
        new Vector2Int(1, 0),
        new Vector2Int(-1, 0),
        new Vector2Int(0, 1),
        new Vector2Int(0, -1)
    };

    private bool isBuilding;

    private void Start()
    {
        seedText.text = worldSeed.ToString();
    }
    public void PlaceRandomSeed()
    {
        worldSeed = Random.Range(-10000, 10000);
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
            obj.transform.parent = transform;
            tiles.Add(obj);

            obj.SetActive(false);

            NewOrigin();
        }

        SetRooms();

        cameraTrans.position = GetMiddleTile(tiles[0].transform, GetFurthersFromSpot(tiles[0].transform).transform).transform.position;
        cameraTrans.Translate(Vector3.forward * cameraStepBack);

        if (generationSpeed > 0)
        {
            StartCoroutine(GenerateTiles());
        }
        else
        {
            foreach (var tile in tiles)
            {
                tile.gameObject.SetActive(true);
            }

            isBuilding = false;
        }

    }

    private IEnumerator GenerateTiles()
    {
        foreach (var tile in tiles)
        {
            tile.SetActive(true);
            yield return new WaitForSeconds(generationSpeed);
        }

        isBuilding = false;
        yield return null;
    }

    private void SetRooms()
    {

        //Set Finish as furtherst room from the first placed tile
        Tile finish = GetFurthersFromSpot(tiles[0].transform);
        finish.SetType(tileType.Finish);

        //Set Start as furtherst room from Finish
        Tile Start = GetFurthersFromSpot(finish.transform);
        Start.SetType(tileType.Start);

        //Set treasure 
        Tile Treasure = GetMiddleTile(tiles[0].transform, finish.transform);
        Treasure.SetType(tileType.Treasure);

        //Set danger rooms around treasure room
        foreach (var tile in GetNeigbours(Treasure.transform))
        {
            tile.SetType(tileType.Danger);
        }
        
        //Dead ends have Puzzle tiles
        foreach (var tile in tiles)
        {
            if(GetNeigbours(tile.transform).Count <= 1)
                tile.GetComponent<Tile>().SetType(tileType.Puzzle);
            
        }
    }

    private void NewOrigin()
    {
        Vector2Int o = origin;

        origin += RandomVector();

        if (GetNeigbours(tiles[tiles.Count - 1].transform).Count < 4)
        {
            if (IsOccupied(origin))
            {
                origin = o;
                NewOrigin();
            }
        }
        else
        {
            Vector2Int rndDir = RandomVector();
            while (IsOccupied(origin))
            {
                origin += rndDir;
            }
        }
    }

    private Vector2Int RandomVector()
    {
        int rnd = Random.Range(0, directions.Length);
        return directions[rnd];
    }

    private bool IsOccupied(Vector2Int pos)
    {

        foreach (var tile in tiles)
        {
            if (tile.transform.position.x == pos.x &&
                tile.transform.position.y == pos.y)
            {
                return true;
            }
        }

        return false;
    }


    [ContextMenu("Replace")]
    public void Replace()
    {
        if (isBuilding)
            return;

        foreach (var tile in tiles)
        {
            Destroy(tile.gameObject);
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
            current = tile.transform;
            distance = current.position - origin.position;

            if (distance.magnitude < previous.magnitude)
                continue;

            previous = distance;
            furtherst = tile.GetComponent<Tile>();
        }

        return furtherst;
    }

    private Tile GetMiddleTile(Transform posA, Transform posB)
    {
        Tile middleTile = tiles[0].GetComponent<Tile>();

        Vector3 middleVec = (posB.position - posA.position) * 0.5f;

        Vector3 distance = Vector3.zero;
        Vector3 previous = middleVec;

        foreach (var tile in tiles)
        {
            distance = middleVec - tile.transform.position;

            if (distance.magnitude > previous.magnitude)
                continue;

            previous = distance;
            middleTile = tile.GetComponent<Tile>();
        }

        return middleTile;
    }

    private List<Tile> GetNeigbours(Transform origin)
    {
        List<Tile> neigbours = new List<Tile>();

        foreach (var tile in tiles)
        {
            for (int i = 0; i < directions.Length; i++)
            {
                Vector3 currentPos = origin.position + new Vector3(directions[i].x, directions[i].y);
                if (tile.gameObject.transform.position == currentPos)
                {
                    neigbours.Add(tile.GetComponent<Tile>());
                }
            }
        }

        return neigbours;
    }
}
