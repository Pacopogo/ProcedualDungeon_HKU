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


    public void SetType(tileType type)
    {
        CurrentType = type;

        if(type != tileType.None)
        {
            isTyped = true;

        }
        else
        {
            isTyped = false;
        }

        switch (type)
        {
            case tileType.None:
                GetComponent<SpriteRenderer>().color = Color.white;

                break;
            case tileType.Start:
                GetComponent<SpriteRenderer>().color = Color.green;
                gameObject.name = "start";

                break;
            case tileType.Finish:
                GetComponent<SpriteRenderer>().color = Color.red;
                gameObject.name = "finish";

                break;
            case tileType.Treasure:
                GetComponent<SpriteRenderer>().color = Color.yellow;

                break;
            case tileType.Danger:
                GetComponent<SpriteRenderer>().color = Color.purple;

                break;
        }
    }
}