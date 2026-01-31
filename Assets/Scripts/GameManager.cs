using UnityEngine;

public class GameManager : MonoBehaviour
{

    float time = 0f;
    [SerializeField] FloatSO health;
    [SerializeField] IntSO collectables;

    [Header("Level Configuration")]
    [SerializeField] LevelConfigSO levelConfigSO;
    [SerializeField] int currentLevelNumber = 1;

    [Header("Random Sound Generator")]
    [Tooltip("Random sound generator that plays ambient sounds during gameplay")]
    [SerializeField] RandomSoundGenerator randomSoundGenerator;

    private bool isLevelActive = false;
    private float initialHealth;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (isLevelActive)
        {
            time += Time.deltaTime;
        }
    }

    public void startLevel()
    {
        time = 0f;
        collectables.Value = 0;
        initialHealth = health.Value;
        isLevelActive = true;
        randomSoundGenerator.StartPlaying();
    }

    public void startLevel(int levelNumber)
    {
        currentLevelNumber = levelNumber;
        startLevel();
    }

    public void endLevel()
    {
        isLevelActive = false;
        randomSoundGenerator.StopPlaying();
        CalculateLevelCompletion();
    }

    /// <summary>
    /// Calculates and logs the star rating when a level is completed.
    /// </summary>
    private void CalculateLevelCompletion()
    {
        if (levelConfigSO == null)
        {
            Debug.LogWarning("LevelConfigSO is not assigned in GameManager!");
            return;
        }

        float timeTaken = time;
        int collectablesFound = collectables.Value;
        float damageTaken = initialHealth - health.Value;

        int stars = levelConfigSO.CalculateStars(currentLevelNumber, timeTaken, collectablesFound, damageTaken);

        Debug.Log($"Level {currentLevelNumber} Completed!\n" +
                  $"Time: {timeTaken:F2}s (Required: {GetLevelTimeRequirement()}s)\n" +
                  $"Collectables: {collectablesFound} (Required: {GetLevelCollectableRequirement()})\n" +
                  $"Damage Taken: {damageTaken:F2} (Max Allowed: {GetLevelDamageRequirement()})\n" +
                  $"Stars Earned: {stars}/3");
    }

    /// <summary>
    /// Gets the time requirement for the current level.
    /// </summary>
    private int GetLevelTimeRequirement()
    {
        LevelConfig config = levelConfigSO?.GetLevelConfig(currentLevelNumber);
        return config != null ? config.timeSec : 0;
    }

    /// <summary>
    /// Gets the collectable requirement for the current level.
    /// </summary>
    private int GetLevelCollectableRequirement()
    {
        LevelConfig config = levelConfigSO?.GetLevelConfig(currentLevelNumber);
        return config != null ? config.collectable : 0;
    }

    /// <summary>
    /// Gets the damage requirement for the current level.
    /// </summary>
    private float GetLevelDamageRequirement()
    {
        LevelConfig config = levelConfigSO?.GetLevelConfig(currentLevelNumber);
        return config != null ? config.damage : 0f;
    }

    public void pauseLevel()
    {
        isLevelActive = false;
        randomSoundGenerator.StopPlaying();
    }

    public void resumeLevel()
    {
        isLevelActive = true;
        randomSoundGenerator.StartPlaying();
    }
}
