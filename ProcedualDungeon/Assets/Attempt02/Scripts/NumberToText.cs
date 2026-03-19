using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NumberToText : MonoBehaviour
{
    [SerializeField] private WorldGenerator worldGenerator;
    [SerializeField] private TMP_InputField seedInputField;

    [SerializeField] private TMP_InputField worldSizeField;

    [SerializeField] private Slider buildSlider;
    [SerializeField] private TMP_Text speedText;

    private void Start()
    {
        SetBuildSpeed();
    }
    public void PrintCode()
    {
        int numb = int.Parse(seedInputField.text);
        Debug.Log("Current seed:" + numb);
        worldGenerator.worldSeed = numb;
    }

    public void SetBuildSpeed()
    {
        worldGenerator.generationSpeed = buildSlider.value;
        speedText.text = buildSlider.value.ToString("f3");
    }

    public void SetWorldSize()
    {
        int numb = int.Parse(worldSizeField.text);
        Debug.Log("Current World Size:" + numb);

        if(numb < 4)
            numb = 4;

        worldGenerator.worldSize = numb;
    }
}
