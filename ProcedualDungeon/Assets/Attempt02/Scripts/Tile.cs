using System;
using UnityEngine;
using UnityEngine.Events;

public enum tileType
{
    None,
    Start,
    Finish,
    Treasure,
    Danger,
    Puzzle
}

public class Tile : MonoBehaviour
{
    public tileType CurrentType = tileType.None;

    public bool isTyped = false;

    [SerializeField] private SpriteRenderer iconSprite;
    [SerializeField] private Sprite[] Icons;

    public UnityEvent OnTileClick;

    [HideInInspector] public WorldGenerator worldGen;

    public void SetType(tileType type)
    {
        if (isTyped)
            return;

        CurrentType = type;

        if (type != tileType.None)
        {
            isTyped = true;

        }
        else
        {
            isTyped = false;
        }

        iconSprite.gameObject.SetActive(isTyped);

        switch (type)
        {
            case tileType.None:
                GetComponent<SpriteRenderer>().color = Color.white;

                break;
            case tileType.Start:
                GetComponent<SpriteRenderer>().color = Color.green;
                iconSprite.sprite = Icons[0];
                gameObject.name = "Start";


                break;
            case tileType.Finish:
                GetComponent<SpriteRenderer>().color = Color.red;
                iconSprite.sprite = Icons[1];
                gameObject.name = "Finish";

                break;
            case tileType.Treasure:

                gameObject.name = "Treasure";
                GetComponent<SpriteRenderer>().color = Color.yellow;
                iconSprite.sprite = Icons[2];

                break;
            case tileType.Danger:

                gameObject.name = "Danger";
                GetComponent<SpriteRenderer>().color = Color.orange;
                iconSprite.sprite = Icons[3];
                break;

            case tileType.Puzzle:

                gameObject.name = "Puzzle";
                GetComponent<SpriteRenderer>().color = Color.purple;
                iconSprite.sprite = Icons[4];
                break;
        }
    }

    private void OnMouseDown()
    {
        Debug.Log(worldGen.GetNeigbours(transform.position).Count);

        OnTileClick?.Invoke();
    }
}