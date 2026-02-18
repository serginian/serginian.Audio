using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using DG.Tweening;
using serginian.Audio;

/// <summary>
/// Singleton audio manager for playing sound effects and music with smooth transitions.
/// Manages multiple audio streams for sounds and two streams for crossfading music.
/// </summary>
public sealed class AudioPlayer : MonoBehaviour
{
    /// <summary>
    /// Number of simultaneous sound effect streams available.
    /// </summary>
    [Tooltip("Number of simultaneous sound effect streams available")]
    [SerializeField] private int streamsCount = 5;

    /// <summary>
    /// Duration in seconds for smooth music fade transitions.
    /// </summary>
    [Tooltip("Duration in seconds for smooth music fade transitions")]
    [SerializeField] private float smoothTime = 1f;

    /// <summary>
    /// Gets whether the AudioPlayer singleton instance exists.
    /// </summary>
    public static bool IsAvailable => _instance;
    private static bool IsInitialized { get; set; }

    private static readonly Dictionary<string, AudioMap> AudioMappings = new Dictionary<string, AudioMap>();
    private static readonly AudioSource[] MusicSources = new AudioSource[2];
    private static AudioPlayer _instance;
    private static AudioSource[] _sources;
    private static AudioClip _currentMusic = null;
    private static int _curThemeStream = -1;
    private static int _curSourceIndex = 0;


    /******************************* MONO BEHAVIOUR *******************************/

    private void Awake()
    {
        if (_instance)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(gameObject);
        InitializePlayer();
    }

    private void InitializePlayer()
    {
        if (IsInitialized)
            return;

        _sources = new AudioSource[streamsCount];
        for (int i = 0; i < streamsCount; i++)
        {
            var source = gameObject.AddComponent<AudioSource>();
            source.loop = false;
            source.playOnAwake = false;
            _sources[i] = source;
        }

        MusicSources[0] = gameObject.AddComponent<AudioSource>();
        MusicSources[0].loop = true;
        MusicSources[0].playOnAwake = false;
        MusicSources[1] = gameObject.AddComponent<AudioSource>();
        MusicSources[1].loop = true;
        MusicSources[1].playOnAwake = false;

        IsInitialized = true;
    }


    /******************************* PUBLIC INTERFACE *******************************/

    /// <summary>
    /// Retrieves an audio clip by name from loaded themes.
    /// </summary>
    /// <param name="name">Name of the audio clip to retrieve.</param>
    /// <returns>The audio clip if found, otherwise null.</returns>
    public static AudioClip GetAudioClip(string name)
    {
        return AudioMappings.TryGetValue(name, out var map) ? map.clip : null;
    }

    /// <summary>
    /// Loads an audio theme, making its sounds available for playback.
    /// </summary>
    /// <param name="theme">The audio theme to load.</param>
    /// <param name="unloadCurrent">If true, unloads all previously loaded themes first.</param>
    public static void LoadTheme(AudioTheme theme, bool unloadCurrent = false)
    {
        if (!theme)
        {
            Debug.LogError("AudioPlayer: Cannot load null audio theme");
            return;
        }

        if (unloadCurrent)
            UnloadThemes();

        foreach (var map in theme.sounds)
            AudioMappings[map.name] = map;
    }

    /// <summary>
    /// Unloads a specific audio theme, removing its sounds from memory.
    /// </summary>
    /// <param name="theme">The audio theme to unload.</param>
    public static void UnloadTheme(AudioTheme theme)
    {
        foreach (var map in theme.sounds)
            AudioMappings.Remove(map.name);
    }

    /// <summary>
    /// Unloads all currently loaded audio themes.
    /// </summary>
    public static void UnloadThemes()
    {
        AudioMappings.Clear();
    }

    /// <summary>
    /// Plays a sound effect by name from loaded themes.
    /// </summary>
    /// <param name="name">Name of the sound in the loaded themes.</param>
    /// <param name="volume">Volume multiplier (0 to 1).</param>
    public static void PlaySound(string name, float volume = 1f)
    {
        if (!AudioMappings.ContainsKey(name))
        {
            Debug.LogWarning($"AudioPlayer: Sound '{name}' not found in current theme");
            return;
        }

        var map = AudioMappings[name];
        PlaySound(map.clip, volume, map.mixerGroup);
    }

    /// <summary>
    /// Plays a sound effect from an audio clip directly.
    /// </summary>
    /// <param name="clip">The audio clip to play.</param>
    /// <param name="volume">Volume multiplier (0 to 1).</param>
    /// <param name="group">Optional audio mixer group.</param>
    public static void PlaySound(AudioClip clip, float volume = 1f, AudioMixerGroup group = null)
    {
        if (!IsInitialized)
            _instance.InitializePlayer();

        var audio = _sources[_curSourceIndex];
        audio.Stop();
        audio.volume = volume;
        audio.clip = clip;
        audio.outputAudioMixerGroup = group;
        audio.Play();
        _curSourceIndex++;
        if (_curSourceIndex == _sources.Length)
            _curSourceIndex = 0;
    }

    /// <summary>
    /// Plays a sound effect from a sound cue.
    /// </summary>
    /// <param name="cue">The sound cue defining how to play the sound.</param>
    /// <param name="volume">Volume multiplier (0 to 1).</param>
    public static void PlaySound(SoundCue cue, float volume = 1f)
    {
        if (cue.source == Cue.AudioSource.Theme)
            PlaySound(cue.audioName, volume);
        else
            PlaySound(cue.GetClip(), volume, cue.mixerGroup);
    }

    /// <summary>
    /// Plays looping music by name from loaded themes with smooth fade-in.
    /// </summary>
    /// <param name="name">Name of the music track in the loaded themes.</param>
    /// <param name="volume">Volume multiplier (0 to 1).</param>
    public static void PlayMusic(string name, float volume = 1f)
    {
        if (!AudioMappings.ContainsKey(name))
            return;

        var map = AudioMappings[name];
        PlayMusic(map.clip, volume, map.mixerGroup);
    }

    /// <summary>
    /// Plays looping music from an audio clip with smooth crossfade transition.
    /// </summary>
    /// <param name="clip">The audio clip to play.</param>
    /// <param name="volume">Volume multiplier (0 to 1).</param>
    /// <param name="group">Optional audio mixer group.</param>
    // ReSharper disable Unity.PerformanceAnalysis
    public static void PlayMusic(AudioClip clip, float volume = 1f, AudioMixerGroup group = null)
    {
        if (_currentMusic == clip)
            return;

        if (!IsInitialized)
            _instance.InitializePlayer();

        StopMusic();

        _curThemeStream = (_curThemeStream + 1) % MusicSources.Length;

        MusicSources[_curThemeStream].clip = clip;
        MusicSources[_curThemeStream].volume = 0f;
        MusicSources[_curThemeStream].outputAudioMixerGroup = group;
        MusicSources[_curThemeStream].Play();
        MusicSources[_curThemeStream].DOKill();
        MusicSources[_curThemeStream].DOFade(volume, _instance.smoothTime).SetUpdate(true);

        _currentMusic = clip;
    }

    /// <summary>
    /// Plays looping music from a music cue with smooth crossfade transition.
    /// </summary>
    /// <param name="cue">The music cue defining how to play the music.</param>
    /// <param name="volume">Volume multiplier (0 to 1).</param>
    public static void PlayMusic(MusicCue cue, float volume = 1f)
    {
        if (cue.source == Cue.AudioSource.Theme)
            PlayMusic(cue.audioName, volume);
        else
            PlayMusic(cue.GetClip(), volume, cue.mixerGroup);
    }

    /// <summary>
    /// Stops currently playing music with the default smooth fade-out.
    /// </summary>
    public static void StopMusic()
    {
        if (_curThemeStream < 0)
            return;

        var curStream = MusicSources[_curThemeStream];
        if (!curStream.isPlaying && curStream.time <= 0f)
            return;

        _currentMusic = null;

        curStream.DOKill();
        curStream.DOFade(0f, _instance.smoothTime).SetUpdate(true).onComplete = () => curStream.Stop();
    }

    /// <summary>
    /// Mutes currently playing music with the default smooth fade-out.
    /// </summary>
    public static void MuteMusic()
    {
        MuteMusic(_instance.smoothTime);
    }

    /// <summary>
    /// Unmutes currently playing music with the default smooth fade-in.
    /// </summary>
    public static void UnmuteMusic()
    {
        UnmuteMusic(_instance.smoothTime);
    }

    /// <summary>
    /// Mutes currently playing music with a custom fade-out duration.
    /// </summary>
    /// <param name="smoothing">Duration in seconds for the fade-out.</param>
    public static void MuteMusic(float smoothing)
    {
        if (_curThemeStream < 0)
            return;

        var curStream = MusicSources[_curThemeStream];
        curStream.DOKill();
        curStream.DOFade(0f, smoothing).SetUpdate(true).onComplete = () => curStream.Pause();
    }

    /// <summary>
    /// Unmutes currently playing music with a custom fade-in duration.
    /// </summary>
    /// <param name="smoothing">Duration in seconds for the fade-in.</param>
    public static void UnmuteMusic(float smoothing)
    {
        if (_curThemeStream < 0)
            return;

        var curStream = MusicSources[_curThemeStream];
        curStream.UnPause();
        curStream.DOKill();
        curStream.DOFade(1f, smoothing).SetUpdate(true);
    }
    
} // end of class