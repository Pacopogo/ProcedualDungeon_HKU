using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NumberToText : MonoBehaviour
{
    [Header("World Seed")]
    [SerializeField] private WorldGenerator worldGenerator;
    [SerializeField] private TMP_InputField seedInputField;

    [Header("World Size")]
    [SerializeField] private Slider worldSizeSlider;
    [SerializeField] private TMP_Text worldSizeText;
    
    [Header("Build Speed")]
    [SerializeField] private Slider buildSlider;
    [SerializeField] private TMP_Text speedText;

    [Header("Combat amount")]
    [SerializeField] private Slider combatSlider;
    [SerializeField] private TMP_Text combatText;


    private void Start()
    {
        SetBuildSpeed();
        SetWorldSize();
        SetCombat();
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
        int numb = Mathf.RoundToInt(worldSizeSlider.value);
        Debug.Log("Current World Size:" + numb);

        worldSizeText.text = numb.ToString();
        worldGenerator.worldSize = numb;
    }

    public void SetCombat()
    {
        int numb = Mathf.RoundToInt(combatSlider.value);

        combatText.text = numb.ToString();
        worldGenerator.amountCombat = numb;
    }
}
