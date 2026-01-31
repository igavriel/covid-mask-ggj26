using UnityEngine;
using System.Collections;

/// <summary>
/// Randomly plays audio clips from a list at configurable intervals.
/// Can be started and stopped programmatically.
/// </summary>
public class RandomSoundGenerator : MonoBehaviour
{
    [Header("Audio Configuration")]
    [Tooltip("List of audio clips to randomly play from")]
    [SerializeField] AudioClip[] audioClips;

    [Tooltip("AudioSource component to play sounds. If not assigned, one will be created automatically.")]
    [SerializeField] AudioSource audioSource;

    [Header("Timing Configuration")]
    [Tooltip("Minimum time between sounds (in seconds). Default: 2 seconds")]
    [SerializeField] float minIntervalSeconds = 2f;

    [Tooltip("Maximum time between sounds (in seconds). Default: 5 seconds")]
    [SerializeField] float maxIntervalSeconds = 5f;

    [Tooltip("Volume for the random sounds (0-1)")]
    [SerializeField] [Range(0f, 1f)] float volume = 1f;

    private Coroutine soundCoroutine;
    private bool isPlaying = false;

    void Awake()
    {
        // Create AudioSource if not assigned
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
            audioSource.loop = false;
            audioSource.spatialBlend = 0f; // 2D audio
        }
    }

    /// <summary>
    /// Starts playing random sounds. Will continue until Stop() is called.
    /// </summary>
    public void StartPlaying()
    {
        if (isPlaying)
        {
            Debug.LogWarning("RandomSoundGenerator: Already playing sounds!");
            return;
        }

        if (audioClips == null || audioClips.Length == 0)
        {
            Debug.LogWarning("RandomSoundGenerator: No audio clips assigned! Cannot play sounds.");
            return;
        }

        // Filter out null clips
        int validClips = 0;
        foreach (var clip in audioClips)
        {
            if (clip != null) validClips++;
        }

        if (validClips == 0)
        {
            Debug.LogWarning("RandomSoundGenerator: All audio clips are null! Cannot play sounds.");
            return;
        }

        isPlaying = true;
        soundCoroutine = StartCoroutine(PlayRandomSoundsCoroutine());
    }

    /// <summary>
    /// Stops playing random sounds.
    /// </summary>
    public void StopPlaying()
    {
        if (!isPlaying)
        {
            return;
        }

        isPlaying = false;

        if (soundCoroutine != null)
        {
            StopCoroutine(soundCoroutine);
            soundCoroutine = null;
        }

        // Stop any currently playing sound
        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }

    /// <summary>
    /// Coroutine that continuously plays random sounds at random intervals.
    /// Waits for each sound to finish playing before starting the next interval.
    /// </summary>
    private IEnumerator PlayRandomSoundsCoroutine()
    {
        while (isPlaying)
        {
            // Wait for a random interval (in seconds)
            float waitTimeSeconds = Random.Range(minIntervalSeconds, maxIntervalSeconds);
            yield return new WaitForSeconds(waitTimeSeconds);

            // Only play if still playing (in case Stop() was called during wait)
            if (isPlaying)
            {
                AudioClip clipToPlay = GetRandomClip();
                if (clipToPlay != null)
                {
                    // Play the sound and wait for it to finish completely
                    yield return StartCoroutine(PlaySoundAndWait(clipToPlay));
                }
            }
        }
    }

    /// <summary>
    /// Gets a random valid clip from the audio clips list.
    /// </summary>
    private AudioClip GetRandomClip()
    {
        if (audioClips == null || audioClips.Length == 0)
        {
            return null;
        }

        // Get a random valid clip
        AudioClip clipToPlay = null;
        int attempts = 0;
        const int maxAttempts = 50; // Prevent infinite loop

        while (clipToPlay == null && attempts < maxAttempts)
        {
            int randomIndex = Random.Range(0, audioClips.Length);
            clipToPlay = audioClips[randomIndex];
            attempts++;
        }

        if (clipToPlay == null)
        {
            Debug.LogWarning("RandomSoundGenerator: Could not find a valid audio clip to play!");
        }

        return clipToPlay;
    }

    /// <summary>
    /// Plays a sound and waits for it to finish playing completely.
    /// </summary>
    private IEnumerator PlaySoundAndWait(AudioClip clip)
    {
        if (audioSource == null || clip == null)
        {
            yield break;
        }

        // Play the sound
        audioSource.clip = clip;
        audioSource.volume = volume;
        audioSource.Play();

        // Wait for the entire sound to finish playing
        yield return new WaitForSeconds(clip.length);
    }

    /// <summary>
    /// Returns whether the generator is currently playing sounds.
    /// </summary>
    public bool IsPlaying()
    {
        return isPlaying;
    }

    void OnDestroy()
    {
        StopPlaying();
    }
}
