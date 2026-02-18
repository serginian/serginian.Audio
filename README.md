# serginian.Audio

**Unified Audio Management for Unity**

Stop duplicating AudioSource components and managing audio playback manually in every script. serginian.Audio centralizes all audio logic into a single, persistent singleton that handles sound effect pooling, music crossfading, and theme-based organization. Audio engineers can swap clips, adjust volumes, and configure mixer routing without touching code. Programmers work with clean, simple APIs that abstract away implementation details.

## What You Get

- **🎵 Singleton Audio Manager** — One AudioPlayer to rule them all, persists across scenes
- **🔄 Smooth Music Crossfading** — Dual-stream system with DOTween-powered transitions
- **📦 Theme-Based Organization** — Group sounds by scene, level, or context using ScriptableObjects
- **🎯 Audio Cue System** — Designer-friendly assets for sound effects and music
- **🎚️ Audio Mixer Integration** — Full support for mixer groups and advanced routing
- **⚡ Sound Effect Pooling** — Multiple AudioSource streams for simultaneous playback

## Who Is This For?

serginian.Audio is built **for programmers and audio engineers** working together. You'll need basic knowledge of **C#** and **Unity** to integrate it effectively. If you're comfortable with:
- C# static classes and singleton patterns
- Unity's ScriptableObject system
- DOTween animation library

...then you're ready to unify your game's audio with serginian.Audio.

> **Note:** This is not a visual audio editor or DAW replacement. It's a code-first audio management framework that provides clean APIs and organizational structure to simplify audio implementation in your game.

---

## Table of Contents

- [Requirements](#requirements)
- [Installation](#installation)
- [Quick Start](#quick-start)
- [Core Concepts](#core-concepts)
  - [AudioPlayer](#audioplayer--singleton-audio-manager)
  - [Audio Themes](#audio-themes)
  - [Audio Cues](#audio-cues)
  - [AudioProfile](#audioprofile--scene-audio-setup)
- [API Reference](#api-reference)
  - [Playing Sounds](#playing-sounds)
  - [Playing Music](#playing-music)
  - [Music Control](#music-control)
  - [Theme Management](#theme-management)
- [Best Practices](#best-practices)

## Requirements

- **Unity** 6000.0+
- **DOTween** (Demigiant) — used for smooth audio fade transitions

## Installation

Add the package to your Unity project via the Unity Package Manager using a Git URL:

1. Open **Window → Package Manager**
2. Click **+** → **Add package from git URL…**
3. Enter your repository URL, e.g.:
   ```
   https://github.com/serginian/serginian.Audio.git
   ```

    Or add it directly to your `Packages/manifest.json`:

    ```json
    {
        "dependencies":
        {
            "com.serginian.audio": "https://github.com/serginian/serginian.Audio.git"
        }
    }
    ```

    > Make sure DOTween is already installed in your project.

## Quick Start

Get up and running with serginian.Audio in 5 steps:

### 1. Set Up AudioPlayer

1. Create an empty GameObject in your first scene (e.g., "AudioPlayer")
2. Add the `AudioPlayer` component
3. Configure `streamsCount` (default: 5) - number of simultaneous sound effects
4. Set `smoothTime` (default: 2s) - duration for music fade transitions

The AudioPlayer will automatically persist across scene loads.

### 2. Create an Audio Theme

1. Right-click in Project → **Create → serginian → Audio → Theme**
2. Name it (e.g., "MainMenuTheme")
3. In Inspector, set the size of the `sounds` array
4. For each entry:
   - Set a unique `name` (e.g., "ButtonClick")
   - Assign an `AudioClip`
   - Optionally assign an `AudioMixerGroup`

### 3. Load the Theme

**Option A: Via AudioProfile component (recommended for scene-specific audio)**
1. Add `AudioProfile` component to any GameObject in your scene
2. Assign your `AudioTheme` to the `themeFile` field
3. Optionally enable `playMusicOnStart` and set `musicName`

**Option B: Via code (for dynamic theme switching)**
```csharp
AudioPlayer.LoadTheme(mainMenuTheme);
```

### 4. Play Sounds

Once a theme is loaded, play sounds by name:
```csharp
AudioPlayer.PlaySound("ButtonClick");
AudioPlayer.PlayMusic("MenuTheme");
```

### 5. Create Audio Cues (Optional)

For designer-friendly workflow:
1. Right-click → **Create → serginian → Audio → Sound Cue**
2. Configure the cue:
   - `source`: Theme (by name), Clip (direct), or Random (from list)
   - Set corresponding fields (name, clip, or clipList)
3. Play the cue:
   ```csharp
   [SerializeField] private SoundCue clickSound;

   void OnButtonClicked()
   {
       clickSound.Play();
   }
   ```

---

## Core Concepts

## AudioPlayer — Singleton Audio Manager

`AudioPlayer` is a singleton `MonoBehaviour` that manages all audio playback in your game. It maintains multiple AudioSource streams for sound effects and two streams for crossfading music.

### Initialization

The AudioPlayer initializes automatically when first accessed. It:
- Creates a configurable number of AudioSource components for sound effects (pooled round-robin)
- Creates two AudioSource components for music (for crossfading)
- Persists across scene loads via `DontDestroyOnLoad`

### Setup

| Field | Description |
|---|---|
| `streamsCount` | Number of simultaneous sound effect streams (default: 5) |
| `smoothTime` | Duration for music fade transitions in seconds (default: 1s) |

**Example:**
```csharp
// No manual initialization needed - just call static methods
AudioPlayer.PlaySound("Jump");
AudioPlayer.PlayMusic("BattleTheme");
```

---

## Audio Themes

`AudioTheme` is a ScriptableObject that groups related audio clips together. Use themes to organize audio by scene, level, game mode, or any other logical grouping.

### Creating a Theme

**Create:** *Right-click → Create → serginian → Audio → Theme*

### Structure

Each theme contains an array of `AudioMap` entries:

| Field | Description |
|---|---|
| `name` | Unique identifier for the clip (e.g., "ButtonClick") |
| `clip` | The AudioClip asset |
| `mixerGroup` | Optional AudioMixerGroup for routing |

### Loading and Unloading Themes

```csharp
// Load a theme (adds to current mappings)
AudioPlayer.LoadTheme(mainMenuTheme);

// Load and unload all previous themes
AudioPlayer.LoadTheme(gameplayTheme, unloadCurrent: true);

// Unload a specific theme
AudioPlayer.UnloadTheme(mainMenuTheme);

// Unload all themes
AudioPlayer.UnloadThemes();
```

### Best Practices

- **One theme per scene/context**: Create themes like "MainMenuTheme", "GameplayTheme", "BossTheme"
- **Load on scene start**: Use `AudioProfile` component for automatic theme loading
- **Unload unused themes**: Free memory by unloading themes when transitioning contexts
- **Unique names across themes**: If multiple themes are loaded, ensure clip names don't conflict

---

## Audio Cues

Audio cues are ScriptableObject assets that define **how** audio should be played. They provide three modes for sourcing audio clips and can be assigned in the Inspector for designer-friendly workflows.

### Cue Types

#### SoundCue
For one-shot sound effects (non-looping).

**Create:** *Right-click → Create → serginian → Audio → Sound Cue*

#### MusicCue
For looping background music with crossfade transitions.

**Create:** *Right-click → Create → serginian → Audio → Music Cue*

### Cue Source Modes

| Mode | Description | Use Case |
|---|---|---|
| **Theme** | Reference audio by name from loaded theme | Best for centralized audio management |
| **Clip** | Direct AudioClip reference | Quick setup, self-contained cues |
| **Random** | Randomly select from a list of clips | Audio variation (e.g., footsteps, impacts) |

### Cue Fields

| Field | Description |
|---|---|
| `source` | How to select the audio clip (Theme/Clip/Random) |
| `audioName` | Name in theme (used when source = Theme) |
| `audioClip` | Direct clip reference (used when source = Clip) |
| `clipList` | Array of clips (used when source = Random) |
| `mixerGroup` | Optional AudioMixerGroup for routing |

### Using Cues

```csharp
public class ButtonController : MonoBehaviour
{
    [SerializeField] private SoundCue clickSound;
    [SerializeField] private SoundCue hoverSound;

    public void OnClick()
    {
        clickSound.Play();
    }

    public void OnHover()
    {
        hoverSound.Play(volume: 0.5f);
    }
}
```

### Benefits of Cues

- **Designer-friendly**: Audio engineers can modify cues without touching code
- **Inspector workflow**: Drag-and-drop assignment in Unity Inspector
- **Audio variation**: Random mode eliminates repetitive sound fatigue
- **Separation of concerns**: Programmers reference cues, engineers configure implementation

---

## AudioProfile — Scene Audio Setup

`AudioProfile` is a component that automatically loads an audio theme when a scene starts. Optionally, it can also start playing music.

### Setup

1. Add `AudioProfile` component to any GameObject in your scene
2. Assign an `AudioTheme` to the `themeFile` field
3. (Optional) Enable `playMusicOnStart`
4. (Optional) Set `musicName` to the name of a track in the theme

### Fields

| Field | Description |
|---|---|
| `themeFile` | The AudioTheme to load on scene start |
| `playMusicOnStart` | Whether to automatically play music |
| `musicName` | Name of the music track in the theme |

### Example Use Case

```csharp
// No code needed! Just configure in Inspector:
// - themeFile: "MainMenuTheme"
// - playMusicOnStart: true
// - musicName: "MenuTheme"

// The AudioProfile will automatically:
// 1. Load MainMenuTheme on Start()
// 2. Play "MenuTheme" music with smooth fade-in
```

---

## API Reference

## Playing Sounds

### PlaySound(string name, float volume = 1f)

Plays a sound effect by name from loaded themes.

```csharp
AudioPlayer.PlaySound("ButtonClick");
AudioPlayer.PlaySound("Explosion", volume: 0.8f);
```

### PlaySound(AudioClip clip, float volume = 1f, AudioMixerGroup group = null)

Plays a sound effect from a direct AudioClip reference.

```csharp
[SerializeField] private AudioClip jumpSound;
[SerializeField] private AudioMixerGroup sfxGroup;

void Jump()
{
    AudioPlayer.PlaySound(jumpSound, volume: 1f, group: sfxGroup);
}
```

### PlaySound(SoundCue cue, float volume = 1f)

Plays a sound from a SoundCue asset.

```csharp
[SerializeField] private SoundCue footstepCue;

void OnFootstep()
{
    AudioPlayer.PlaySound(footstepCue, volume: 0.7f);
}
```

---

## Playing Music

All music playback methods automatically handle smooth crossfading between tracks using DOTween.

### PlayMusic(string name, float volume = 1f)

Plays looping music by name from loaded themes.

```csharp
AudioPlayer.PlayMusic("BattleTheme");
AudioPlayer.PlayMusic("VictoryTheme", volume: 0.9f);
```

### PlayMusic(AudioClip clip, float volume = 1f, AudioMixerGroup group = null)

Plays looping music from a direct AudioClip reference.

```csharp
[SerializeField] private AudioClip bossMusic;
[SerializeField] private AudioMixerGroup musicGroup;

void OnBossEncounter()
{
    AudioPlayer.PlayMusic(bossMusic, volume: 1f, group: musicGroup);
}
```

### PlayMusic(MusicCue cue, float volume = 1f)

Plays music from a MusicCue asset.

```csharp
[SerializeField] private MusicCue menuMusicCue;

void ShowMainMenu()
{
    AudioPlayer.PlayMusic(menuMusicCue);
}
```

### Music Behavior

- **Automatic crossfade**: When playing a new track, the current track fades out while the new one fades in
- **Duplicate prevention**: Playing the same track that's already playing is a no-op
- **Looping**: All music automatically loops until stopped or replaced

---

## Music Control

### StopMusic()

Stops currently playing music with a smooth fade-out.

```csharp
AudioPlayer.StopMusic();
```

### MuteMusic() / MuteMusic(float smoothing)

Temporarily mutes music with a fade-out (keeps track position).

```csharp
// Use default smooth time
AudioPlayer.MuteMusic();

// Custom fade duration
AudioPlayer.MuteMusic(smoothing: 0.5f);
```

### UnmuteMusic() / UnmuteMusic(float smoothing)

Resumes muted music with a fade-in.

```csharp
// Use default smooth time
AudioPlayer.UnmuteMusic();

// Custom fade duration
AudioPlayer.UnmuteMusic(smoothing: 0.5f);
```

**Example: Pause Menu**
```csharp
public class PauseMenu : MonoBehaviour
{
    void OnPauseOpened()
    {
        AudioPlayer.MuteMusic();
    }

    void OnPauseClosed()
    {
        AudioPlayer.UnmuteMusic();
    }
}
```

---

## Theme Management

### LoadTheme(AudioTheme theme, bool unloadCurrent = false)

Loads an audio theme, making its clips available by name.

```csharp
[SerializeField] private AudioTheme mainMenuTheme;
[SerializeField] private AudioTheme gameplayTheme;

void Start()
{
    // Load theme (additive by default)
    AudioPlayer.LoadTheme(mainMenuTheme);

    // Load theme and unload all previous themes
    AudioPlayer.LoadTheme(gameplayTheme, unloadCurrent: true);
}
```

### UnloadTheme(AudioTheme theme)

Unloads a specific theme, removing its clips from memory.

```csharp
AudioPlayer.UnloadTheme(mainMenuTheme);
```

### UnloadThemes()

Unloads all currently loaded themes.

```csharp
AudioPlayer.UnloadThemes();
```

### GetAudioClip(string name)

Retrieves an AudioClip by name from loaded themes.

```csharp
AudioClip clip = AudioPlayer.GetAudioClip("ButtonClick");
if (clip != null)
{
    // Use clip directly...
}
```

---

## Best Practices

### Audio Organization

**Theme Structure**
- Create one theme per major context (e.g., "MainMenu", "Level1", "BossEncounter")
- Use consistent naming conventions (e.g., "UI_ButtonClick", "SFX_Footstep", "Music_Battle")
- Group related sounds together (e.g., all footstep variations in one theme)

**Naming Conventions**
```csharp
// Good: Descriptive, context-aware names
"UI_ButtonClick"
"SFX_PlayerJump"
"Music_BossBattle"

// Bad: Vague or generic names
"Sound1"
"Audio"
"Clip"
```

### Performance

**Theme Loading**
- Load themes at scene start using `AudioProfile` component
- Unload unused themes when transitioning between contexts
- Use `LoadTheme(theme, unloadCurrent: true)` when switching contexts completely

**Sound Effect Pooling**
- Increase `streamsCount` if you need more simultaneous sounds (e.g., combat-heavy games)
- Default of 5 streams works well for most games
- Exceeding the stream count will interrupt the oldest playing sound

### Workflow

**Separation of Concerns**
```csharp
// Programmers: Work with abstractions
AudioPlayer.PlaySound("ButtonClick");

// Audio Engineers: Configure in ScriptableObjects
// - Swap clips without touching code
// - Adjust volumes via mixer groups
// - Add variations using Random source mode
```

**Use Cues for Inspector Workflow**
```csharp
// Instead of hardcoded strings:
AudioPlayer.PlaySound("Footstep"); // ❌ Prone to typos

// Use cues for type safety and designer control:
[SerializeField] private SoundCue footstepCue;
footstepCue.Play(); // ✅ Inspector-assignable, refactor-safe
```

### Audio Mixer Integration

**Setup Mixer Groups**
1. Create an Audio Mixer asset in Unity
2. Define groups (e.g., "Master", "SFX", "Music", "UI")
3. Assign groups in AudioTheme entries or Cue assets
4. Control volume, pitch, and effects via mixer at runtime

**Example:**
```csharp
// AudioTheme assigns all UI sounds to "UI" mixer group
// AudioMixer can apply ducking, filters, volume control
// No code changes required - all handled by Unity's mixer
```

---

## Troubleshooting

### Common Issues

**Sound doesn't play**
- Check that the theme containing the sound is loaded
- Verify the sound name matches exactly (case-sensitive)
- Ensure AudioPlayer GameObject exists and hasn't been destroyed
- Check `streamsCount` - may need more simultaneous streams

**Music doesn't crossfade smoothly**
- Verify DOTween is imported and initialized
- Check `smoothTime` value in AudioPlayer component
- Ensure music clips have proper loop points

**Theme not loading**
- Check for null reference errors in console
- Verify AudioTheme asset is assigned correctly
- Ensure `AudioPlayer.LoadTheme()` is called before playing sounds

**AudioPlayer missing in scene**
- AudioPlayer persists via DontDestroyOnLoad - check root hierarchy
- Verify AudioPlayer component is attached to a GameObject
- Check for duplicate AudioPlayer instances (should auto-destroy)

---

## Credits

serginian.Audio is built with:
- **DOTween** by Demigiant (http://dotween.demigiant.com/)
- **Unity** 6000.0+
