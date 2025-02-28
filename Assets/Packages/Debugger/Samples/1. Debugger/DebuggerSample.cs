using System;
using UnityEngine;

namespace Kunnymann.Debugger.Sample
{
    public class DebuggerSample : MonoBehaviour
    {
        private void Start()
        {
            Debugger.Debug("Hello World!");
            Debugger.Info("Hello World!");
            Debugger.Warning("Hello World!");
            Debugger.Error("Hello World!");
            Debugger.Fatal("Hello World!");

            ErrorListener.Check(new NullReferenceException(), "Invoke error handling", true, false);
        }
    }
}