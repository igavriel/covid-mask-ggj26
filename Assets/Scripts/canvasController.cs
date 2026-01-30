using UnityEngine;
using UnityEngine.UI;

public class canvasController : MonoBehaviour
{
    [SerializeField] Slider slider;
    [SerializeField] FloatSO health;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        slider.value = health.Value;
    }
}
