using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace Kunnymann.Base.Editor
{
    /// <summary>
    /// 커스텀 하이어라키
    /// </summary>
    [InitializeOnLoad]
    public class CustomHierarchy
    {
        /// <summary>
        /// 생성자
        /// </summary>
        static CustomHierarchy()
        {
            EditorApplication.hierarchyWindowItemOnGUI += HandleHierarchyWindowItemOnGUI;
        }

        private static void HandleHierarchyWindowItemOnGUI(int instanceID, Rect selectionRect)
        {
            var obj = EditorUtility.InstanceIDToObject(instanceID);

            if (obj == null)
                return;

            if (Regex.IsMatch(obj.name, CustomHierarchyConst.PatternModuleManager))
            {
                DrawIcon(selectionRect, Resources.Load<Texture2D>(CustomHierarchyConst.PathIcon));
                return;
            }

            if (Regex.IsMatch(obj.name, CustomHierarchyConst.PatternDivisionLine))
            {
                var dpName = Regex.Replace(obj.name, CustomHierarchyConst.PatternDivisionLine, string.Empty);
                DrawDivisionLine(selectionRect, dpName, Color.white, FontStyle.Bold);
                return;
            }
        }

        private static void DrawIcon(Rect selectionRect, Texture2D icon)
        {
            Rect iconRect = new Rect(selectionRect.x, selectionRect.y, CustomHierarchyConst.IconSize, selectionRect.height);
            EditorGUI.DrawPreviewTexture(iconRect, icon);
        }

        private static void DrawIcon(Rect selectionRect, string content)
        {
            EditorGUI.LabelField(selectionRect, EditorGUIUtility.IconContent(content));
        }

        private static void DrawLabel(Rect selectionRect, string dpName, Color fontColor, FontStyle fontStyle, string hexToBackgroundColor)
        {
            EditorGUI.DrawRect(selectionRect, ColorUtility.TryParseHtmlString(hexToBackgroundColor, out var color) ? color : Color.gray);
            EditorGUI.LabelField(selectionRect, dpName, new GUIStyle(GUI.skin.label)
            {
                padding = new RectOffset(CustomHierarchyConst.IconSize, 0, 0, 0),
                border = new RectOffset(1, 1, 1, 1),
                normal = new GUIStyleState { textColor = fontColor },
                fontStyle = fontStyle
            });
        }

        private static void DrawDivisionLine(Rect selectionRect, string dpName, Color fontColor, FontStyle fontStyle)
        {
            EditorGUI.DrawRect(selectionRect, ColorUtility.TryParseHtmlString(CustomHierarchyConst.ColorUnityBackground, out var color) ? color : Color.gray);
            EditorGUI.LabelField(selectionRect, dpName, new GUIStyle(GUI.skin.label)
            {
                normal = new GUIStyleState { textColor = fontColor },
                alignment = TextAnchor.MiddleCenter,
                fontStyle = fontStyle
            });
        }
    }
}