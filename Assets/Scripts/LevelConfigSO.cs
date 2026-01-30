using UnityEngine;

[CreateAssetMenu(fileName = "LevelConfigSO", menuName = "Scriptable Objects/Level Config SO")]
public class LevelConfigSO : ScriptableObject
{
    [Header("Level Configurations")]
    [Tooltip("Array of level configurations. Each entry defines the 3-star requirements for that level.")]
    public LevelConfig[] levels;

    /// <summary>
    /// Gets the configuration for a specific level number.
    /// </summary>
    /// <param name="levelNumber">The level number to retrieve</param>
    /// <returns>LevelConfig for the specified level, or null if not found</returns>
    public LevelConfig GetLevelConfig(int levelNumber)
    {
        foreach (var level in levels)
        {
            if (level.levelNumber == levelNumber)
            {
                return level;
            }
        }
        return null;
    }

    /// <summary>
    /// Calculates the star rating based on performance metrics using weighted formula:
    /// Time = 20%, Collectables = 50%, Health = 30%
    /// </summary>
    /// <param name="levelNumber">The level number</param>
    /// <param name="timeTaken">Time taken in seconds</param>
    /// <param name="collectablesFound">Number of collectables found</param>
    /// <param name="damageTaken">Total damage taken</param>
    /// <returns>Star rating (0-3)</returns>
    public int CalculateStars(int levelNumber, float timeTaken, int collectablesFound, float damageTaken)
    {
        LevelConfig config = GetLevelConfig(levelNumber);
        if (config == null)
        {
            Debug.LogWarning($"Level config not found for level {levelNumber}");
            return 0;
        }

        // Calculate percentage score for each metric (0-100%)
        // Time: less is better, score based on how much under the requirement
        float timeScore = 0f;
        if (config.timeSec > 0)
        {
            if (timeTaken <= config.timeSec)
            {
                timeScore = 100f; // Perfect time
            }
            else
            {
                // Penalty for going over time (linear decrease)
                float overTime = timeTaken - config.timeSec;
                timeScore = Mathf.Max(0f, 100f - (overTime / config.timeSec) * 100f);
            }
        }

        // Collectables: more is better, score based on how many collected vs required
        float collectableScore = 0f;
        if (config.collectable > 0)
        {
            collectableScore = Mathf.Min(100f, (collectablesFound / (float)config.collectable) * 100f);
        }
        else if (collectablesFound > 0)
        {
            collectableScore = 100f; // If no requirement but found some, give full score
        }

        // Health/Damage: less damage is better, score based on how much under the max damage
        float healthScore = 0f;
        if (config.damage > 0)
        {
            if (damageTaken <= config.damage)
            {
                // Score based on how little damage taken (less damage = higher score)
                // At max damage (damageTaken = damage), score = 75% (meets requirement)
                // At no damage (damageTaken = 0), score = 100% (perfect)
                healthScore = Mathf.Max(0f, 100f - (damageTaken / config.damage) * 25f);
            }
            else
            {
                // Penalty for exceeding max damage
                healthScore = 0f;
            }
        }
        else if (damageTaken == 0)
        {
            healthScore = 100f; // No damage requirement and took no damage
        }

        // Apply weights: Time = 20%, Collectables = 50%, Health = 30%
        float weightedScore = (timeScore * 0.2f) + (collectableScore * 0.5f) + (healthScore * 0.3f);

        // Convert weighted score (0-100) to stars (0-3)
        // 0-33% = 0 stars, 34-66% = 1 star, 67-83% = 2 stars, 84-100% = 3 stars
        int stars = 0;
        if (weightedScore >= 84f)
        {
            stars = 3;
        }
        else if (weightedScore >= 67f)
        {
            stars = 2;
        }
        else if (weightedScore >= 34f)
        {
            stars = 1;
        }

        return stars;
    }
}
