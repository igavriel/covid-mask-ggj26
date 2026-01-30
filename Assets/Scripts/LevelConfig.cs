using UnityEngine;

[System.Serializable]
public class LevelConfig
{
    [Header("Level Configuration")]
    public int levelNumber;
    
    [Header("3-Star Requirements")]
    [Tooltip("Time in seconds required for 3 stars")]
    public int timeSec;
    
    [Tooltip("Number of collectables required for 3 stars")]
    public int collectable;
    
    [Tooltip("Maximum damage allowed for 3 stars")]
    public float damage;
}
