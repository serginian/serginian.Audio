using UnityEditor;
using UnityEngine;

namespace serginian.Audio.Editor
{
    [CustomEditor(typeof(AudioProfile))]
    public class AudioProfileEditor : UnityEditor.Editor
    {
        private static readonly GUIContent ThemeFileLabel = new GUIContent("Audio Theme", "Theme file for this level");
        private static readonly GUIContent UnloadPreviousThemeLabel = new GUIContent("Unload Previous Theme", "Whether to unload the previous theme before loading this one");
        private static readonly GUIContent PlayOnStartLabel = new GUIContent("Auto-play Music", "Automatically play music when level starts");
        private static readonly GUIContent MusicSelectionLabel = new GUIContent("Music Selection", "Choose which music track to play on start");
        private static readonly GUIContent MusicTrackLabel = new GUIContent("Music Track", "Select track from the theme");

        private SerializedProperty _themeFileProp;
        private SerializedProperty _unloadPreviousThemeProp;
        private SerializedProperty _playMusicOnStartProp;
        private SerializedProperty _musicNameProp;

        private void OnEnable()
        {
            _themeFileProp = serializedObject.FindProperty(nameof(AudioProfile.themeFile));
            _unloadPreviousThemeProp = serializedObject.FindProperty(nameof(AudioProfile.unloadPreviousTheme));
            _playMusicOnStartProp = serializedObject.FindProperty(nameof(AudioProfile.playMusicOnStart));
            _musicNameProp = serializedObject.FindProperty(nameof(AudioProfile.musicName));
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            // Theme selection
            EditorGUILayout.LabelField("Level Settings", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(_themeFileProp, ThemeFileLabel);
            EditorGUILayout.PropertyField(_unloadPreviousThemeProp, UnloadPreviousThemeLabel);

            EditorGUILayout.Space(10);

            // Auto-play music toggle
            EditorGUILayout.LabelField("Music Settings", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(_playMusicOnStartProp, PlayOnStartLabel);

            // Show music selection if auto-play is enabled
            if (_playMusicOnStartProp.boolValue)
            {
                AudioTheme theme = _themeFileProp.objectReferenceValue as AudioTheme;

                if (!theme)
                {
                    EditorGUILayout.HelpBox("Assign an Audio Theme to select music tracks.", MessageType.Info);
                }
                else if (theme.sounds == null || theme.sounds.Length == 0)
                {
                    EditorGUILayout.HelpBox("The selected theme has no audio tracks.", MessageType.Warning);
                }
                else
                {
                    EditorGUILayout.LabelField(MusicSelectionLabel, EditorStyles.miniBoldLabel);

                    // Build list of track names
                    string[] trackNames = new string[theme.sounds.Length + 1];
                    trackNames[0] = "(None)";
                    for (int i = 0; i < theme.sounds.Length; i++)
                    {
                        trackNames[i + 1] = string.IsNullOrEmpty(theme.sounds[i].name)
                            ? $"Track {i}"
                            : theme.sounds[i].name;
                    }

                    // Find current selection
                    int currentIndex = 0;
                    string currentMusicName = _musicNameProp.stringValue;
                    if (!string.IsNullOrEmpty(currentMusicName))
                    {
                        for (int i = 0; i < theme.sounds.Length; i++)
                        {
                            if (theme.sounds[i].name == currentMusicName)
                            {
                                currentIndex = i + 1;
                                break;
                            }
                        }
                    }

                    // Draw dropdown
                    int newIndex = EditorGUILayout.Popup(MusicTrackLabel, currentIndex, trackNames);

                    // Update music name if selection changed
                    if (newIndex != currentIndex)
                    {
                        _musicNameProp.stringValue = newIndex == 0 ? "" : theme.sounds[newIndex - 1].name;
                    }
                }
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}
