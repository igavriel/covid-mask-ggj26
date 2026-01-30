using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using DG;
using DG.Tweening;

public class canvasController : MonoBehaviour
{
    [SerializeField] Slider slider;
    [SerializeField] FloatSO health;
    [SerializeField] FloatSO maskSO;
    [SerializeField] TMP_Text text;
    [SerializeField] CanvasGroup dmg;

    Tween dmgFader;
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

    public void dmgStart()
    {
        dmg.DOFade(0.3f, 0.1f);
        dmgFader = dmg.DOFade(1, 0.3f).SetLoops(-1, LoopType.Yoyo);
    }

    public void dmgStop()
    {
        dmgFader.Kill();
        dmg.DOFade(0, 0.3f);
    }
}
