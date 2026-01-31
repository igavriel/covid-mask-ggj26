using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using DG.Tweening;

public class canvasController : MonoBehaviour
{
    [Header("=== GAME HUD ===")]
    [SerializeField] Slider slider;
    [SerializeField] FloatSO health;
    [SerializeField] FloatSO maskSO;
    [SerializeField] TMP_Text text;

    [Header("=== OVERLAYS ===")]
    [Tooltip("Damage overlay CanvasGroup (מה שהיה canvasDMG)")]
    [SerializeField] CanvasGroup damageOverlay;

    [Tooltip("Mask overlay CanvasGroup (מה שהיה canvasMask)")]
    [SerializeField] CanvasGroup maskOverlay;

    [Header("=== MENU UI ===")]
    [SerializeField] GameObject gameHUD;           
    [SerializeField] GameObject mainMenuPanel;
    [SerializeField] GameObject instructionsPanel;
    [SerializeField] GameObject creditsPanel;

    [Header("=== PAUSE ===")]
    [SerializeField] PauseController pauseController;

    Tween damageOverlayFader;

    void Start()
    {
        if (mainMenuPanel != null || instructionsPanel != null || creditsPanel != null)
            ShowMainMenu();
    }

    void Update()
    {
        if (slider != null && health != null)
            slider.value = health.Value;

        if (text != null && maskSO != null)
        {
            if (maskSO.Value > 0)
                text.text = MathF.Ceiling(maskSO.Value).ToString();
            else
                text.text = "";
        }
    }

    // =========================
    // ===== MENU METHODS ======
    // =========================

    public void ShowMainMenu()
    {
        if (pauseController != null)
        {
            pauseController.canPause = false;
            pauseController.SetPaused(false);
        }

        if (mainMenuPanel != null) mainMenuPanel.SetActive(true);
        if (instructionsPanel != null) instructionsPanel.SetActive(false);
        if (creditsPanel != null) creditsPanel.SetActive(false);

        if (gameHUD != null) gameHUD.SetActive(false);
    }

    public void StartGame()
    {
        if (pauseController != null)
        {
            pauseController.canPause = true;
            pauseController.SetPaused(false);
        }

        if (mainMenuPanel != null) mainMenuPanel.SetActive(false);
        if (instructionsPanel != null) instructionsPanel.SetActive(false);
        if (creditsPanel != null) creditsPanel.SetActive(false);

        if (gameHUD != null) gameHUD.SetActive(true);
    }

    public void OpenInstructions()
    {
        if (mainMenuPanel != null) mainMenuPanel.SetActive(false);
        if (instructionsPanel != null) instructionsPanel.SetActive(true);
        if (creditsPanel != null) creditsPanel.SetActive(false);
    }

    public void OpenCredits()
    {
        if (mainMenuPanel != null) mainMenuPanel.SetActive(false);
        if (instructionsPanel != null) instructionsPanel.SetActive(false);
        if (creditsPanel != null) creditsPanel.SetActive(true);
    }

    public void BackToMenu()
    {
        if (instructionsPanel != null) instructionsPanel.SetActive(false);
        if (creditsPanel != null) creditsPanel.SetActive(false);
        if (mainMenuPanel != null) mainMenuPanel.SetActive(true);
    }

    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    // =========================
    // ===== MASK OVERLAY ======
    // =========================

    public void putMaskOn()
    {
        if (maskOverlay == null) return;
        maskOverlay.DOFade(1f, 0.5f);
    }

    public void putMaskOff()
    {
        if (maskOverlay == null) return;
        if (maskOverlay.alpha >= 1f)
            maskOverlay.DOFade(0f, 0.5f);
        else
            maskOverlay.DOFade(0f, 0.2f);
    }

    // =========================
    // ==== DAMAGE OVERLAY =====
    // =========================

    public void StartDamageOverlay()
    {
        if (damageOverlay == null) return;

        damageOverlay.DOFade(0.3f, 0.1f);

        damageOverlayFader?.Kill();
        damageOverlayFader = damageOverlay
            .DOFade(1f, 0.3f)
            .SetLoops(-1, LoopType.Yoyo);
    }

    public void StopDamageOverlay()
    {
        if (damageOverlay == null) return;

        damageOverlayFader?.Kill();
        damageOverlay.DOFade(0f, 0.3f);
    }

    public void dmgStart() => StartDamageOverlay();
    public void dmgStop()  => StopDamageOverlay();
}
