using UnityEditor;
using UnityEngine;
using UnityEditorInternal;
using Kunnymann.Base.Utility;

namespace Kunnymann.Base.Editor
{
    /// <summary>
    /// ModuleManager 에디터 클래스
    /// </summary>
    [CustomEditor(typeof(ModuleManager))]
    public class ModuleManagerEditor : UnityEditor.Editor
    {
        private ModuleManager _moduleManager = null;
        private SerializedProperty _modules = null;
        private ReorderableList _list;

        private void OnEnable()
        {
            _moduleManager = target as ModuleManager;
            _modules = serializedObject.FindProperty("Modules");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            if (_list == null)
                _list = new ReorderableList(serializedObject, _modules, true, true, false, true)
                {
                    drawHeaderCallback = OnDrawHeader,
                    drawElementCallback = OnDrawElement,
                    onReorderCallback = OnReorder,
                    elementHeightCallback = CalculateCallHeight,
                    onRemoveCallback = OnRemoveItem,
                };

            _list.DoLayoutList();
            if (GUILayout.Button("Reload modules"))
            {
                ManagerInitUtility.InitModules(_moduleManager);
                EditorUtility.SetDirty(_moduleManager);
            }

            serializedObject.ApplyModifiedProperties();
        }

        private void OnDrawHeader(Rect rect)
        {
            string nameField = "Module List";
            EditorGUI.LabelField(rect, nameField);
        }

        private void OnDrawElement(Rect rect, int index, bool isActive, bool isFocused)
        {
            string nameField = _moduleManager.Modules[index].GetType().FullName;

            EditorGUI.LabelField(rect, nameField);
        }

        private void OnRemoveItem(ReorderableList list)
        {
            ModuleBase target = list.serializedProperty.GetArrayElementAtIndex(list.index).objectReferenceValue as ModuleBase;
            
            if (target != null)
            {
                target.Dispose();
            }
            list.serializedProperty.DeleteArrayElementAtIndex(list.index);
            list.serializedProperty.serializedObject.ApplyModifiedProperties();
        }

        private void OnReorder(ReorderableList list)
        {
            serializedObject.ApplyModifiedProperties();
        }

        private float CalculateCallHeight(int idx)
        {
            return EditorGUI.GetPropertyHeight(_list.serializedProperty.GetArrayElementAtIndex(idx));
        }
    }
}