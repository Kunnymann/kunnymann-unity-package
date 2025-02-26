using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Kunnymann.Navigation.Sample.Editor
{
    [CustomEditor(typeof(NavigationService))]
    public class NavigationServiceEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            NavigationService navigationService = (NavigationService)target;

            // Draw default inspector
            DrawDefaultInspector();

            if (GUILayout.Button("Start Navigation"))
            {
                navigationService.StartNavigation();
            }

            if (GUILayout.Button("Stop Navigation"))
            {
                navigationService.StopNavigation();
            }
        }
    }
}
