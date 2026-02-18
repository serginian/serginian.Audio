using UnityEngine;
using UnityEngine.Audio;

namespace serginian.Audio
{
    /// <summary>
    /// Base class for audio cues that define how audio clips should be played.
    /// Supports three modes: referencing audio from a theme by name, using a specific clip, or randomly selecting from a list.
    /// </summary>
    public abstract class Cue : ScriptableObject
    {
        /// <summary>
        /// Defines where the audio clip comes from.
        /// </summary>
        public enum AudioSource
        {
            /// <summary>Reference audio by name from loaded theme.</summary>
            Theme,
            /// <summary>Use a specific audio clip directly.</summary>
            Clip,
            /// <summary>Randomly select from a list of clips.</summary>
            Random
        }

        /// <summary>
        /// Specifies how the audio clip is selected for playback.
        /// </summary>
        [Tooltip("How the audio clip is selected: Theme (by name), Clip (direct reference), or Random (from list)")]
        public AudioSource source;

        /// <summary>
        /// Name of the audio in the theme (used when source is Theme).
        /// </summary>
        [Tooltip("Name of the audio in the theme (used when source is Theme)")]
        public string audioName;

        /// <summary>
        /// Direct audio clip reference (used when source is Clip).
        /// </summary>
        [Tooltip("Direct audio clip reference (used when source is Clip)")]
        public AudioClip audioClip;

        /// <summary>
        /// List of audio clips for random selection (used when source is Random).
        /// </summary>
        [Tooltip("List of audio clips for random selection (used when source is Random)")]
        public AudioClip[] clipList;

        /// <summary>
        /// Audio mixer group for routing audio output (optional).
        /// </summary>
        [Tooltip("Audio mixer group for routing audio output (optional)")]
        public AudioMixerGroup mixerGroup;

        /// <summary>
        /// Plays the audio cue with the specified volume.
        /// </summary>
        /// <param name="volume">Volume multiplier (0 to 1).</param>
        public abstract void Play(float volume = 1f);

        /// <summary>
        /// Gets the audio clip based on the configured source mode.
        /// </summary>
        /// <returns>The audio clip to play, or null if unavailable.</returns>
        public AudioClip GetClip()
        {
            switch (source)
            {
                case AudioSource.Clip: return audioClip;
                case AudioSource.Random:
                    if (clipList == null)
                        return null;
                    
                    var length = clipList.Length;
                    if (length == 0)
                        return null;
                    
                    return clipList[Random.Range(0, length)];
                default: return AudioPlayer.GetAudioClip(audioName);
            }
        }
        
    } // end of class
}