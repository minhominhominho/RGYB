using System.Collections;
using System.Collections.Generic;
using static UnityEngine.GraphicsBuffer;

namespace RGYB
{
#if UNITY_EDITOR
    using UnityEngine;
    using UnityEditor;
    using UnityEditor.SceneManagement;

    [CustomEditor(typeof(GameManager))]
    public class CustomEditor_GameManager : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            GameManager initializer = (GameManager)target;
            if (GUILayout.Button("Read"))
            {
                Debug.Log("Read");
                initializer.LoadSequence();
                Debug.Log("ReadDone");

                EditorUtility.SetDirty(initializer);
            }
        }
    }
#endif
}