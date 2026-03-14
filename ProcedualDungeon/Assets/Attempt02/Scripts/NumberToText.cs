using TMPro;
using UnityEngine;

public class NumberToText : MonoBehaviour
{
    [SerializeField] private WorldGenerator worldGenerator;
    [SerializeField] private TMP_InputField inputField;

    public void PrintCode()
    {
        int numb = int.Parse(inputField.text);
        Debug.Log("Current seed:" + numb);
        worldGenerator.worldSeed = numb;
    }
}
