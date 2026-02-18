# Changelog

All notable changes to serginian.Audio will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [1.0.0] - 2026-02-18

### Initial Release

serginian.Audio's first public release - a unified audio management system for Unity 6000.0+.

#### Added - Core Features

**AudioPlayer**
- Singleton audio manager with automatic initialization
- Multiple simultaneous sound effect streams (configurable count)
- Dual-stream music system with smooth crossfading
- DOTween-powered fade transitions for professional audio mixing
- `PlaySound()` methods with theme name, direct clip, or cue support
- `PlayMusic()` methods with automatic crossfade transitions
- Music control: `StopMusic()`, `MuteMusic()`, `UnmuteMusic()` with smooth fading
- Theme management: `LoadTheme()`, `UnloadTheme()`, `UnloadThemes()`
- Clip retrieval by name: `GetAudioClip()`
- DontDestroyOnLoad persistence across scenes

**Audio Organization**
- `AudioTheme` ScriptableObject for grouping related sounds
- `AudioMap` struct for name-to-clip mapping with mixer group support
- Theme-based audio loading and unloading
- Audio Mixer integration for advanced routing

**Cue System**
- `Cue` base class with three source modes: Theme, Clip, Random
- `SoundCue` for one-shot sound effects
- `MusicCue` for looping background music
- Random clip selection for audio variation
- Create via asset menu: *serginian → Audio → Sound Cue / Music Cue*

**Scene Integration**
- `AudioProfile` component for automatic theme loading on scene start
- Optional auto-play music configuration
- Simplified per-scene audio setup

#### Technical Details

- **Unity Version**: 6000.0+
- **Dependencies**:
  - DOTween (Demigiant) - smooth audio fade transitions
- **Architecture**: Singleton pattern with ScriptableObject-based audio assets
- **Performance**: Multiple AudioSource pooling for sound effects
- **Audio Mixer Support**: Full integration for advanced audio routing

#### Documentation

- Complete README with Quick Start guide
- API reference for all major components
- Code examples for common use cases
- Best practices for audio organization

---

[1.0.0]: https://github.com/serginian/serginian.Audio/releases/tag/v1.0.0
