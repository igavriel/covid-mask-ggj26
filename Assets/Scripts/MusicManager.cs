using UnityEngine;
using System.Collections;

public class MusicManager : MonoBehaviour
{
    [Header("Audio Sources")]
    [SerializeField] AudioSource mainTrack;
    [SerializeField] AudioSource secondaryTrack;

    [Header("Fade Settings")]
    [SerializeField] float fadeInDuration = 1f;
    [SerializeField] float fadeOutDuration = 1f;
    [SerializeField] float secondaryTrackVolume = 1f;

    private Coroutine fadeCoroutine;
    private bool isSecondaryActive = false;

    void Start()
    {
        // Initialize audio sources if not assigned
        if (mainTrack == null)
        {
            mainTrack = gameObject.AddComponent<AudioSource>();
        }
        if (secondaryTrack == null)
        {
            secondaryTrack = gameObject.AddComponent<AudioSource>();
        }

        // Configure main track
        mainTrack.loop = true;
        mainTrack.volume = 1f;
        mainTrack.Play();

        // Configure secondary track
        secondaryTrack.loop = true;
        secondaryTrack.volume = 0f;
        secondaryTrack.Play();
    }

    /// <summary>
    /// Activates the secondary track for a specified duration, then deactivates it.
    /// </summary>
    /// <param name="duration">Duration in seconds to keep the secondary track active</param>
    public void ActivateSecondaryTrack(float duration)
    {
        if (duration <= 0)
        {
            Debug.LogWarning("Duration must be greater than 0");
            return;
        }

        // Stop any existing fade coroutine
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
        }

        // Start activation and auto-deactivation
        fadeCoroutine = StartCoroutine(ActivateSecondaryTrackCoroutine(duration));
    }

    /// <summary>
    /// Manually activates the secondary track (fades in).
    /// </summary>
    public void ActivateSecondaryTrack()
    {
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
        }

        fadeCoroutine = StartCoroutine(FadeInSecondaryTrack());
    }

    /// <summary>
    /// Manually deactivates the secondary track (fades out).
    /// </summary>
    public void DeactivateSecondaryTrack()
    {
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
        }

        fadeCoroutine = StartCoroutine(FadeOutSecondaryTrack());
    }

    /// <summary>
    /// Coroutine that activates the secondary track for a duration, then deactivates it.
    /// </summary>
    private IEnumerator ActivateSecondaryTrackCoroutine(float duration)
    {
        // Fade in
        yield return StartCoroutine(FadeInSecondaryTrack());
        Debug.Log("Fade in secondary track done");
        // Wait for the specified duration
        yield return new WaitForSeconds(duration);
        Debug.Log("Wait for duration");
        // Fade out
        yield return StartCoroutine(FadeOutSecondaryTrack());
    }

    /// <summary>
    /// Fades in the secondary track.
    /// </summary>
    private IEnumerator FadeInSecondaryTrack()
    {
        isSecondaryActive = true;
        float startVolume = secondaryTrack.volume;
        float elapsedTime = 0f;

        while (elapsedTime < fadeInDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / fadeInDuration;
            secondaryTrack.volume = Mathf.Lerp(startVolume, secondaryTrackVolume, t);
            yield return null;
        }

        secondaryTrack.volume = secondaryTrackVolume;
    }

    /// <summary>
    /// Fades out the secondary track.
    /// </summary>
    private IEnumerator FadeOutSecondaryTrack()
    {
        isSecondaryActive = false;
        float startVolume = secondaryTrack.volume;
        float elapsedTime = 0f;

        while (elapsedTime < fadeOutDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / fadeOutDuration;
            secondaryTrack.volume = Mathf.Lerp(startVolume, 0f, t);
            yield return null;
        }

        secondaryTrack.volume = 0f;
    }

    /// <summary>
    /// Sets the audio clips for both tracks.
    /// </summary>
    /// <param name="mainClip">Audio clip for the main track</param>
    /// <param name="secondaryClip">Audio clip for the secondary track</param>
    public void SetAudioClips(AudioClip mainClip, AudioClip secondaryClip)
    {
        if (mainTrack != null)
        {
            mainTrack.clip = mainClip;
            if (mainTrack.isPlaying)
            {
                mainTrack.Stop();
            }
            mainTrack.Play();
        }

        if (secondaryTrack != null)
        {
            secondaryTrack.clip = secondaryClip;
            if (secondaryTrack.isPlaying)
            {
                secondaryTrack.Stop();
            }
            secondaryTrack.Play();
        }
    }

    /// <summary>
    /// Returns whether the secondary track is currently active.
    /// </summary>
    public bool IsSecondaryTrackActive()
    {
        return isSecondaryActive;
    }
}
