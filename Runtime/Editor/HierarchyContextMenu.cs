using serginian.Audio;
using UnityEngine;
using UnityEditor;

public class HierarchyContextMenu
{
    private const string AUDIO_PLAYER_NAME = "Audio Player";
    private const string AUDIO_PLAYER_GUID = "6766a0fa825b1490e80b20dee574ecb6";
    private const string AUDIO_PROFILE_NAME = "Audio Profile";
    private const string AUDIO_PROFILE_GUID = "ab34f7d1ead134f51af686d36e646289";
    
    [MenuItem("GameObject/Audio/Audio Player", false, 10)]
    static void CreateAudioPlayer(MenuCommand menuCommand)
    {
        string path = AssetDatabase.GUIDToAssetPath(AUDIO_PLAYER_GUID);
        GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
        GameObject go;

        if (prefab != null)
        {
            go = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
        }
        else
        {
            go = new GameObject(AUDIO_PLAYER_NAME);
            go.AddComponent<AudioPlayer>();
        }

        GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject);
        Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
        Selection.activeObject = go;
    }
    
    [MenuItem("GameObject/Audio/Audio Profile", false, 9)]
    static void CreateAudioProfile(MenuCommand menuCommand)
    {
        string path = AssetDatabase.GUIDToAssetPath(AUDIO_PROFILE_GUID);
        GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
        GameObject go;

        if (prefab != null)
        {
            go = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
        }
        else
        {
            go = new GameObject(AUDIO_PROFILE_NAME);
            go.AddComponent<AudioProfile>();
        }

        GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject);
        Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
        Selection.activeObject = go;
    }
}