using UnityEditor;
using UnityEngine;

namespace SimpleAudio.Editor
{
    public class SimpleAudioEditorTool
    {
        [MenuItem("GameObject/SimpleAudio/Create Service", false, 1)]
        public static void CreateSimpleAudioService()
        {
            var simpleAudioServiceObject = new GameObject
            {
                name = "SimpleAudioService"
            };

            simpleAudioServiceObject.AddComponent<SimpleAudioService>();

            Selection.activeGameObject = simpleAudioServiceObject;
            
            Undo.RegisterCreatedObjectUndo(simpleAudioServiceObject, "Create SimpleAudioService");
        }
    }
}