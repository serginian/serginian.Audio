using UnityEngine;

namespace serginian.Audio
{
    /// <summary>
    /// A music cue for playing looping background music with smooth transitions.
    /// Create instances via the asset menu: serginian/Audio/Music Cue.
    /// </summary>
    [CreateAssetMenu(fileName = "Music Cue", menuName = "serginian/Audio/Music Cue")]
    public class MusicCue: Cue
    {
        /// <summary>
        /// Plays the music track with the specified volume and smooth fade-in.
        /// </summary>
        /// <param name="volume">Volume multiplier (0 to 1).</param>
        public override void Play(float volume = 1f)
        {
            AudioPlayer.PlayMusic(this, volume);
        }
    }
}