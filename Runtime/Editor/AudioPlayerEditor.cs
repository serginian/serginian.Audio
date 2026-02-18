using UnityEditor;
using UnityEngine;

namespace serginian.Audio.Editor
{
    [CustomEditor(typeof(AudioPlayer))]
    public class AudioPlayerEditor : UnityEditor.Editor
    {
        private static readonly GUIContent StreamsCountLabel = new GUIContent("Streams Count", "Number of concurrent audio streams for sound effects (2-10)");
        private static readonly GUIContent SmoothTimeLabel = new GUIContent("Smooth Time", "Fade in/out duration for music transitions in seconds (0-20)");

        private SerializedProperty _streamsCountProp;
        private SerializedProperty _smoothTimeProp;

        private void OnEnable()
        {
            _streamsCountProp = serializedObject.FindProperty("streamsCount");
            _smoothTimeProp = serializedObject.FindProperty("smoothTime");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            // Description Block
            EditorGUILayout.LabelField("About AudioPlayer", EditorStyles.boldLabel);
            EditorGUILayout.HelpBox(
                "AudioPlayer is a singleton service that unifies audio management in the game. " +
                "It eliminates the clutter of numerous AudioSources on the scene and allows programmers to work with sound as an abstraction. " +
                "The specific implementation is left to the audio engineer, freeing them from needing to dive into code. " +
                "This is a win-win solution for small and medium-sized projects.",
                MessageType.None
            );

            EditorGUILayout.Space(10);

            // Player Settings Block
            EditorGUILayout.LabelField("Player Settings", EditorStyles.boldLabel);

            // Streams Count Slider (2-10)
            _streamsCountProp.intValue = EditorGUILayout.IntSlider(
                StreamsCountLabel,
                _streamsCountProp.intValue,
                2,
                10
            );

            // Smooth Time Slider (0-20)
            _smoothTimeProp.floatValue = EditorGUILayout.Slider(
                SmoothTimeLabel,
                _smoothTimeProp.floatValue,
                0f,
                20f
            );

            EditorGUILayout.Space(15);

            // Author & Credits Block
            DrawCreditsSection();

            serializedObject.ApplyModifiedProperties();
        }

        private void DrawCreditsSection()
        {
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);

            // Product name
            GUIStyle titleStyle = new GUIStyle(EditorStyles.boldLabel)
            {
                alignment = TextAnchor.MiddleCenter,
                fontSize = 12
            };
            EditorGUILayout.LabelField("Unified Audio Management for Unity", titleStyle);

            EditorGUILayout.Space(5);

            // Author
            GUIStyle authorStyle = new GUIStyle(EditorStyles.label)
            {
                alignment = TextAnchor.MiddleCenter,
                fontStyle = FontStyle.Italic
            };
            EditorGUILayout.LabelField("Created by serginian", authorStyle);

            EditorGUILayout.Space(8);

            // Links
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();

            if (GUILayout.Button("LinkedIn", GUILayout.Width(100)))
            {
                Application.OpenURL("https://www.linkedin.com/in/serginian/");
            }

            GUILayout.Space(10);

            if (GUILayout.Button("GitHub", GUILayout.Width(100)))
            {
                Application.OpenURL("https://github.com/serginian");
            }

            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space(5);

            EditorGUILayout.EndVertical();
        }
    }
}
