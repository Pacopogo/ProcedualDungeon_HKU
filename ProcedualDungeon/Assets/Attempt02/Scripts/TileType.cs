using UnityEngine;

[CreateAssetMenu(fileName = "TileData", menuName = "Tiles/TileData", order = 1)]
public class TileType : ScriptableObject
{
    public string TileName = "Tile";
    public Color TileColor = Color.white;
    public Sprite TileIcon = null;

}
