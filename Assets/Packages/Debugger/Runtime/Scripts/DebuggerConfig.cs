using Kunnymann.Utility;
using UnityEngine;

namespace Kunnymann.Debugger
{
    /// <summary>
    /// Debugger config
    /// </summary>
    [CreateAssetMenu(fileName = "DebuggerConfig", menuName = "Kunnymann/Debugger config")]
    public class DebuggerConfig : SingletonScripatableObject<DebuggerConfig>
    {
        /// <summary>
        /// Log level
        /// </summary>
        public LogLevel LogLevel = LogLevel.Debug;
    }
}