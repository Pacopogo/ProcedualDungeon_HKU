using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NumberToText : MonoBehaviour
{
    [SerializeField] private WorldGenerator worldGenerator;
    [SerializeField] private TMP_InputField inputField;

    [SerializeField] private Slider buildSlider;
    [SerializeField] private TMP_Text speedText;

    private void Start()
    {
        SetBuildSpeed();
    }
    public void PrintCode()
    {
        int numb = int.Parse(inputField.text);
        Debug.Log("Current seed:" + numb);
        worldGenerator.worldSeed = numb;
    }

    public void SetBuildSpeed()
    {
        worldGenerator.generationSpeed = buildSlider.value;
        speedText.text = buildSlider.value.ToString("f3");
    }
}
