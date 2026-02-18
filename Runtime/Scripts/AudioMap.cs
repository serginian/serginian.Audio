using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Serialization;

namespace serginian.Audio
{
    /// <summary>
    /// Maps a string name to an audio clip and optional mixer group.
    /// Used by AudioTheme to organize and access audio assets by name.
    /// </summary>
    [Serializable]
    public struct AudioMap
    {
        /// <summary>
        /// Unique identifier for this audio clip.
        /// </summary>
        [Tooltip("Unique identifier for this audio clip")]
        public string name;

        /// <summary>
        /// The audio clip asset.
        /// </summary>
        [Tooltip("The audio clip asset")]
        public AudioClip clip;

        /// <summary>
        /// Optional audio mixer group for controlling audio output.
        /// </summary>
        [Tooltip("Optional audio mixer group for controlling audio output")]
        [FormerlySerializedAs("group")]
        public AudioMixerGroup mixerGroup;
    }
}