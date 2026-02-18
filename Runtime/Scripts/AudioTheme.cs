using UnityEngine;
using UnityEngine.Serialization;

namespace serginian.Audio
{
    /// <summary>
    /// Container for a collection of audio clips organized by name.
    /// Use this to group related sounds (e.g., all sounds for a level or game mode).
    /// Create instances via the asset menu: serginian/Audio/Theme.
    /// </summary>
    [CreateAssetMenu(fileName = "Audio Theme", menuName = "serginian/Audio/Theme")]
    public class AudioTheme : ScriptableObject
    {
        /// <summary>
        /// Array of audio mappings (name to clip associations) in this theme.
        /// </summary>
        [Tooltip("Array of audio mappings (name to clip associations) in this theme")]
        [FormerlySerializedAs("audioLibrary")]
        public AudioMap[] sounds;
    }
}