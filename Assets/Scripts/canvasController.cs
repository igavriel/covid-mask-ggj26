using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class canvasController : MonoBehaviour
{
    [SerializeField] Slider slider;
    [SerializeField] FloatSO health;
    [SerializeField] FloatSO maskSO;
    [SerializeField] TMP_Text text;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        slider.value = health.Value;
        if (maskSO.Value > 0)
            text.text = MathF.Ceiling(maskSO.Value).ToString();
        else
            text.text = "";
    }
}
