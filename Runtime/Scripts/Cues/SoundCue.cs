using UnityEngine;

namespace serginian.Audio
{
    /// <summary>
    /// A sound cue for playing short, non-looping sound effects.
    /// Create instances via the asset menu: serginian/Audio/Sound Cue.
    /// </summary>
    [CreateAssetMenu(fileName = "Sound Cue", menuName = "serginian/Audio/Sound Cue")]
    public class SoundCue : Cue
    {
        /// <summary>
        /// Plays the sound effect with the specified volume.
        /// </summary>
        /// <param name="volume">Volume multiplier (0 to 1).</param>
        public override void Play(float volume = 1f)
        {
            AudioPlayer.PlaySound(this, volume);
        }
    }
}