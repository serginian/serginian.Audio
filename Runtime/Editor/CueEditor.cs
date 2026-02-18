using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;

namespace serginian.Audio.Editor
{
    [CustomEditor(typeof(Cue), true)]
    public class CueEditor : UnityEditor.Editor
    {
        private static readonly GUIContent AudioSourceLabel = new GUIContent("Audio Source");
        private static readonly GUIContent AudioNameLabel = new GUIContent("Audio Name");
        private static readonly GUIContent AudioClipLabel = new GUIContent("Audio Clip");
        private static readonly GUIContent ClipListLabel = new GUIContent("Clip List");
        private static readonly GUIContent MixerGroupLabel = new GUIContent("Mixer Group (Optional)");
        private static readonly GUIContent AdvancedLabel = new GUIContent("Advanced");

        private SerializedProperty _sourceProp;
        private SerializedProperty _audioNameProp;
        private SerializedProperty _audioClipProp;
        private SerializedProperty _clipListProp;
        private SerializedProperty _mixerGroupProp;

        private void OnEnable()
        {
            _sourceProp = serializedObject.FindProperty(nameof(Cue.source));
            _audioNameProp = serializedObject.FindProperty(nameof(Cue.audioName));
            _audioClipProp = serializedObject.FindProperty(nameof(Cue.audioClip));
            _clipListProp = serializedObject.FindProperty(nameof(Cue.clipList));
            _mixerGroupProp = serializedObject.FindProperty(nameof(Cue.mixerGroup));
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            // Source dropdown
            EditorGUILayout.PropertyField(_sourceProp, AudioSourceLabel);

            EditorGUILayout.Space(5);

            // Show appropriate field based on source
            Cue.AudioSource currentSource = (Cue.AudioSource)_sourceProp.enumValueIndex;

            switch (currentSource)
            {
                case Cue.AudioSource.Theme:
                    EditorGUILayout.PropertyField(_audioNameProp, AudioNameLabel);
                    break;

                case Cue.AudioSource.Clip:
                    EditorGUILayout.PropertyField(_audioClipProp, AudioClipLabel);
                    break;

                case Cue.AudioSource.Random:
                    EditorGUILayout.PropertyField(_clipListProp, ClipListLabel, true);
                    break;
            }

            EditorGUILayout.Space(10);

            // Optional mixer group with label
            EditorGUILayout.LabelField(AdvancedLabel, EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(_mixerGroupProp, MixerGroupLabel);

            serializedObject.ApplyModifiedProperties();
        }
    }
}
