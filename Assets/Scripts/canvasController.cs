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

    [Header("=== DAMAGE OVERLAY (Hit Feedback) ===")]
    [SerializeField] CanvasGroup damageOverlay;

    [Header("=== MENU UI ===")]
    [SerializeField] GameObject gameHUD;           // אופציונלי (POC)
    [SerializeField] GameObject mainMenuPanel;     // אם מחובר – נשתמש בו כדי לדעת אם אנחנו בתפריט
    [SerializeField] GameObject instructionsPanel;
    [SerializeField] GameObject creditsPanel;

    [Header("=== PAUSE ===")]
    [SerializeField] PauseController pauseController;

    Tween damageOverlayFader;

    void Start()
    {
        // אם יש MainMenu מחובר – נציג אותו בהתחלה
        if (mainMenuPanel != null || instructionsPanel != null || creditsPanel != null)
            ShowMainMenu();
    }

    void Update()
    {
        // ✅ POC FIX:
        // אם אין לך GameHUD מחובר - לא עושים return.
        // אם יש תפריט ראשי פעיל (mainMenuPanel) - לא חייבים לעדכן HUD (אבל גם לא נקרוס).
        bool isMenuOpen = mainMenuPanel != null && mainMenuPanel.activeSelf;

        // אם תרצה שלא יעדכן HUD כשהתפריט פתוח:
        // if (isMenuOpen) return;

        // עדכוני HUD עם הגנות null
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

        // אם יש לך gameHUD – נכבה. אם לא – לא נוגעים.
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
    // === DAMAGE OVERLAY ======
    // =========================

    public void StartDamageOverlay()
    {
        if (damageOverlay == null) return;

        damageOverlay.DOFade(0.3f, 0.1f);

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
}
