using UnityEngine;

namespace serginian.Audio
{
    /// <summary>
    /// Component that loads an audio theme on scene start and optionally plays music automatically.
    /// Attach this to a GameObject to initialize audio settings for a scene.
    /// </summary>
    public class AudioProfile : MonoBehaviour
    {
        /// <summary>
        /// The audio theme containing sound and music clips to load for this scene.
        /// </summary>
        [Tooltip("The audio theme containing sound and music clips to load for this scene")]
        public AudioTheme themeFile;

        /// <summary>
        /// Whether to unload the previous theme before loading this one.
        /// </summary>
        [Tooltip("Whether to unload the previous theme before loading this one")]
        public bool unloadPreviousTheme = true;

        /// <summary>
        /// Whether to automatically play music when the scene starts.
        /// </summary>
        [Tooltip("Whether to automatically play music when the scene starts")]
        public bool playMusicOnStart = false;

        /// <summary>
        /// Name of the music track to play on start (must exist in the loaded theme).
        /// </summary>
        [Tooltip("Name of the music track to play on start (must exist in the loaded theme)")]
        public string musicName;
        

        private void Start()
        {
            AudioPlayer.LoadTheme(themeFile, unloadPreviousTheme);
            if (playMusicOnStart && !string.IsNullOrEmpty(musicName)) 
                AudioPlayer.PlayMusic(musicName);
        }
    }
}