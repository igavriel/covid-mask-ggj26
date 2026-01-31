using UnityEngine;
using UnityEngine.Audio;

/// <summary>
/// Defines a sound effect with playback parameters.
/// </summary>
[System.Serializable]
public class SoundDefinition
{
    [Tooltip("Unique identifier for this sound")]
    public SoundId id;

    [Tooltip("Audio clip to play for this sound")]
    public AudioClip clip;

    [Range(0f, 1f)]
    [Tooltip("Volume level for this sound (0 = silent, 1 = full volume)")]
    public float volume = 1f;

    [Range(0.8f, 1.2f)]
    [Tooltip("Pitch variation range. 1.0 = normal pitch. Random pitch will be selected within this range.")]
    public float pitchVariation = 1f;
}
