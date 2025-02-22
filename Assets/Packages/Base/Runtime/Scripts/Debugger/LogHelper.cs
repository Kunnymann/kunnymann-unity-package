using UnityEngine;
using System;

namespace Kunnymann.Base.Debugger
{
    /// <summary>
    /// Log level
    /// </summary>
    public enum LogLevel
    {
        /// <summary>
        /// All level
        /// </summary>
        All = 0,
        /// <summary>
        /// Debug level
        /// </summary>
        Debug,
        /// <summary>
        /// Info level
        /// </summary>
        Info,
        /// <summary>
        /// Warning level
        /// </summary>
        Warning,
        /// <summary>
        /// Error level
        /// </summary>
        Error,
        /// <summary>
        /// Fatal level
        /// </summary>
        Fatal
    }

    /// <summary>
    /// Log helper 클래스
    /// </summary>
    public class LogHelper
    {
        /// <summary>
        /// 레벨에 따른 로그를 출력합니다
        /// </summary>
        /// <param name="lv">로그 레벨</param>
        /// <param name="msg"></param>
        /// <exception cref="Exception">유효하지 않는 레벨에 대한 에러</exception>
        public void Log(LogLevel lv, object msg)
        {
            string logLevelWord = string.Empty;
            string logMessage = msg.ToString();

            switch (lv)
            {
                
                case LogLevel.Debug:
                    logLevelWord = string.Format(LogConst.LOG_LEVEL_WORD_INFO, lv.ToString());
                    logMessage = string.Format(LogConst.LOG_FORMAT, logLevelWord, logMessage);
                    Debug.Log(logMessage);
                    break;
                case LogLevel.Info:
                    logLevelWord = string.Format(LogConst.LOG_LEVEL_WORD_INFO, lv.ToString());
                    logMessage = string.Format(LogConst.LOG_FORMAT, logLevelWord, logMessage);
                    Debug.Log(logMessage);
                    break;
                case LogLevel.Warning:
                    logLevelWord = string.Format(LogConst.LOG_LEVEL_WORD_WARNING, lv.ToString());
                    logMessage = string.Format(LogConst.LOG_FORMAT, logLevelWord, logMessage);
                    Debug.LogWarning(logMessage);
                    break;
                case LogLevel.Error:
                    logLevelWord = string.Format(LogConst.LOG_LEVEL_WORD_ERROR, lv.ToString());
                    logMessage = string.Format(LogConst.LOG_FORMAT, logLevelWord, logMessage);
                    Debug.LogError(logMessage);
                    break;
                case LogLevel.Fatal:
                    logLevelWord = string.Format(LogConst.LOG_LEVEL_WORD_FATAL, lv.ToString());
                    logMessage = string.Format(LogConst.LOG_FORMAT, logLevelWord, logMessage);
                    Debug.LogError(logMessage);
                    break;
                default:
                    throw new Exception("Invalid log level");
            }
        }
    }
}
