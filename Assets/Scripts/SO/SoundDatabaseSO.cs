using UnityEngine;

/// <summary>
/// ScriptableObject containing all sound effect definitions for the game.
///
/// SETUP INSTRUCTIONS:
/// 1. Right-click in Project window → Create → Audio → Sound Database
/// 2. Select the created SoundDatabase asset
/// 3. In Inspector, expand the Sounds array
/// 4. For each sound:
///    - Set the Sound ID (e.g., Jump, Explosion, Hit, Collect)
///    - Assign an AudioClip reference
///    - Adjust Volume (0-1) and Pitch Variation (0.8-1.2) as needed
///    - Optionally assign an Audio Mixer Group
/// 5. Assign this SoundDatabase to the SoundManager component in your scene
/// </summary>
[CreateAssetMenu(fileName = "SoundDatabaseSO", menuName = "Scriptable Objects/Sound Database SO")]
public class SoundDatabaseSO : ScriptableObject
{
    [Tooltip("Array of all sound effect definitions")]
    public SoundDefinition[] sounds;
}
