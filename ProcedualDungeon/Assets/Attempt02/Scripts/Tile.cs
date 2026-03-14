using System;
using UnityEngine;

public enum tileType
{
    None,
    Start,
    Finish,
    Treasure,
    Danger
}

public class Tile : MonoBehaviour
{
    public tileType CurrentType = tileType.None;

    public bool isTyped = false;

    [SerializeField] private SpriteRenderer iconSprite;
    [SerializeField] private Sprite[] Icons;

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
                gameObject.name = "start";


                break;
            case tileType.Finish:
                GetComponent<SpriteRenderer>().color = Color.red;
                iconSprite.sprite = Icons[1];
                gameObject.name = "finish";

                break;
            case tileType.Treasure:
                GetComponent<SpriteRenderer>().color = Color.yellow;
                iconSprite.sprite = Icons[2];

                break;
            case tileType.Danger:
                GetComponent<SpriteRenderer>().color = Color.purple;
                iconSprite.sprite = Icons[3];

                break;
        }
    }
}