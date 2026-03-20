using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public bool isTyped = false;
    [SerializeField] private SpriteRenderer iconSprite;

    public void SetType(TileType type)
    {
        if (isTyped)
            return;

        isTyped = true;

        iconSprite.gameObject.SetActive(isTyped);

        GetComponent<SpriteRenderer>().color    = type.TileColor;
        iconSprite.sprite                       = type.TileIcon;
        gameObject.name                         = type.TileName;

    }
}