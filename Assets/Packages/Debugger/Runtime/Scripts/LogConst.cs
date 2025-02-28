namespace Kunnymann.Debugger
{
    /// <summary>
    /// 디버거 로그 상수
    /// </summary>
    public class LogConst
    {
        /// <summary>
        /// Info 로그 레벨 Prefix
        /// </summary>
        internal const string LOG_LEVEL_WORD_INFO       = "<color=white>[{0}]</color>";
        /// <summary>
        /// Warning 로그 레벨 Prefix
        /// </summary>
        internal const string LOG_LEVEL_WORD_WARNING    = "<color=yellow>[{0}]</color>";
        /// <summary>
        /// Error 로그 레벨 Prefix
        /// </summary>
        internal const string LOG_LEVEL_WORD_ERROR      = "<color=red>[{0}]</color>";
        /// <summary>
        /// Fatal 로그 레벨 Prefix
        /// </summary>
        internal const string LOG_LEVEL_WORD_FATAL      = "<color=red><b>[{0}]</b></color>";

        /// <summary>
        /// 로그 포맷
        /// </summary>
        internal const string LOG_FORMAT = "{0} {1}";
    }
}