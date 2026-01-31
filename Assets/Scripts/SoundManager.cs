using UnityEngine;
using UnityEngine.Audio;
using System.Collections.Generic;

/// <summary>
/// Singleton SoundManager for playing 2D sound effects with pooling and variation.
/// Persists across scenes using DontDestroyOnLoad.
///
/// SETUP INSTRUCTIONS:
/// 1. Create SoundDatabase asset (Right-click → Create → Audio → Sound Database)
/// 2. Add your AudioClips to SoundDefinitions in the database
/// 3. Create empty GameObject named "SoundManager" in your scene
/// 4. Attach SoundManager.cs component to it
/// 5. Assign the SoundDatabase to the Database field in Inspector
/// 6. Use anywhere: SoundManager.Instance.Play(SoundId.Jump);
///
/// EXAMPLE USAGE:
/// SoundManager.Instance.Play(SoundId.Jump);
/// SoundManager.Instance.Play(SoundId.Collect);
/// </summary>
public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    [Header("Config")]
    [Tooltip("Sound Database containing all sound definitions")]
    public SoundDatabaseSO database;

    // Pool size is set based on the number of SoundId enum values
    private readonly int poolSize = System.Enum.GetValues(typeof(SoundId)).Length;

    // Dictionary for fast sound lookup
    private Dictionary<SoundId, SoundDefinition> soundDictionary;

    // Pool of AudioSource components
    private List<AudioSource> audioSourcePool;

    private void Awake()
    {
        // Singleton pattern
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Build sound dictionary from database
        BuildSoundDictionary();

        // Create and configure audio source pool
        CreateAudioSourcePool();
    }

    /// <summary>
    /// Builds a dictionary from the SoundDatabase for fast lookups by SoundId.
    /// </summary>
    private void BuildSoundDictionary()
    {
        soundDictionary = new Dictionary<SoundId, SoundDefinition>();

        if (database == null)
        {
            Debug.LogError("SoundManager: SoundDatabase is not assigned! Please assign it in the Inspector.");
            return;
        }

        if (database.sounds == null || database.sounds.Length == 0)
        {
            Debug.LogWarning("SoundManager: SoundDatabase contains no sounds!");
            return;
        }

        foreach (SoundDefinition soundDef in database.sounds)
        {
            if (soundDictionary.ContainsKey(soundDef.id))
            {
                Debug.LogWarning($"SoundManager: Duplicate SoundId '{soundDef.id}' found in database. Skipping duplicate.");
                continue;
            }

            if (soundDef.clip == null)
            {
                Debug.LogWarning($"SoundManager: SoundId '{soundDef.id}' has no clip assigned!");
                continue;
            }

            soundDictionary[soundDef.id] = soundDef;
        }

        Debug.Log($"SoundManager: Loaded {soundDictionary.Count} sounds from database.");
    }

    /// <summary>
    /// Creates and configures the pool of AudioSource components.
    /// All AudioSources are set to 2D (spatialBlend = 0).
    /// Pool size is automatically set based on SoundId enum size.
    /// </summary>
    private void CreateAudioSourcePool()
    {
        audioSourcePool = new List<AudioSource>(poolSize);

        for (int i = 0; i < poolSize; i++)
        {
            GameObject audioSourceObject = new GameObject($"AudioSource_{i}");
            audioSourceObject.transform.SetParent(transform);
            AudioSource audioSource = audioSourceObject.AddComponent<AudioSource>();

            // Configure for 2D audio
            audioSource.spatialBlend = 0f; // Pure 2D audio
            audioSource.playOnAwake = false;
            audioSource.loop = false;

            audioSourcePool.Add(audioSource);
        }
    }

    /// <summary>
    /// Plays a sound effect by its SoundId.
    /// Applies pitch variation and uses pooled AudioSource.
    /// </summary>
    /// <param name="id">The SoundId of the sound to play</param>
    public void Play(SoundId id)
    {
        if (!soundDictionary.ContainsKey(id))
        {
            Debug.LogWarning($"SoundManager: SoundId '{id}' not found in database!");
            return;
        }

        SoundDefinition soundDef = soundDictionary[id];

        // Get a pooled AudioSource
        AudioSource source = GetPooledSource();
        if (source == null)
        {
            Debug.LogWarning($"SoundManager: No available AudioSource in pool for sound '{id}'!");
            return;
        }

        // Validate clip exists
        if (soundDef.clip == null)
        {
            Debug.LogWarning($"SoundManager: Clip for '{id}' is null!");
            return;
        }

        // Configure AudioSource
        source.clip = soundDef.clip;
        source.volume = soundDef.volume;

        // Apply random pitch variation
        float basePitch = 1f;
        float variationRange = soundDef.pitchVariation - 1f;
        float randomPitch = basePitch + Random.Range(-variationRange, variationRange);
        source.pitch = randomPitch;

        // Play the sound
        source.Play();
    }

    /// <summary>
    /// Gets an available AudioSource from the pool.
    /// Prefers idle (not playing) sources, otherwise returns the quietest currently playing source.
    /// </summary>
    /// <returns>An available AudioSource, or null if pool is exhausted</returns>
    private AudioSource GetPooledSource()
    {
        // First pass: find an idle (not playing) AudioSource
        foreach (AudioSource source in audioSourcePool)
        {
            if (!source.isPlaying)
            {
                return source;
            }
        }

        // Second pass: if all are playing, find the quietest one (lowest volume)
        AudioSource quietestSource = null;
        float quietestVolume = float.MaxValue;

        foreach (AudioSource source in audioSourcePool)
        {
            if (source.volume < quietestVolume)
            {
                quietestVolume = source.volume;
                quietestSource = source;
            }
        }

        // Stop the quietest source and return it for reuse
        if (quietestSource != null)
        {
            quietestSource.Stop();
        }

        return quietestSource;
    }
}
