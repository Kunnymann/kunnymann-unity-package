using System;

namespace Kunnymann.Base.Debugger
{
    /// <summary>
    /// Kunnymann 패키지의 디버거 클래스
    /// </summary>
    public class Debugger
    {
        /// <summary>
        /// LogHelper
        /// </summary>
        private static LogHelper _LogHelper = new LogHelper();
        /// <summary>
        /// 로그 레벨
        /// </summary>
        private static LogLevel _LogLevel = LogLevel.Error;

        /// <summary>
        /// Log level
        /// </summary>
        public static LogLevel LogLevel
        {
            get => _LogLevel;
            set => _LogLevel = value;
        }

        /// <summary>
        /// 생성자
        /// </summary>
        /// <exception cref="Exception">Log level 설정 실패</exception>
        static Debugger()
        {
            _LogHelper = new LogHelper();
            _LogLevel = DebuggerConfig.Instance.LogLevel;

            string logLevelWord = string.Empty;

            switch (_LogLevel)
            {

                case LogLevel.Debug:
                    logLevelWord = string.Format(LogConst.LOG_LEVEL_WORD_INFO, _LogLevel.ToString());
                    break;
                case LogLevel.Info:
                    logLevelWord = string.Format(LogConst.LOG_LEVEL_WORD_INFO, _LogLevel.ToString());
                    break;
                case LogLevel.Warning:
                    logLevelWord = string.Format(LogConst.LOG_LEVEL_WORD_WARNING, _LogLevel.ToString());
                    break;
                case LogLevel.Error:
                    logLevelWord = string.Format(LogConst.LOG_LEVEL_WORD_ERROR, _LogLevel.ToString());
                    break;
                case LogLevel.Fatal:
                    logLevelWord = string.Format(LogConst.LOG_LEVEL_WORD_FATAL, _LogLevel.ToString());
                    break;
                default:
                    throw new Exception("Invalid log level");
            }

            _LogHelper.Log(LogLevel.Info, string.Format("HyperDebugger Initialized - Log level : {0}", logLevelWord));
        }

        /// <summary>
        /// Log의 레벨 수준을 비교합니다
        /// </summary>
        /// <param name="lv">대상</param>
        /// <returns>레벨 이하인지 판단한 결과</returns>
        public static bool HasLogLevel(LogLevel lv)
        {
            return _LogLevel <= lv;
        }

        /// <summary>
        /// Debug 레벨의 로그를 출력합니다
        /// </summary>
        /// <param name="msg">메시지</param>
        public static void Debug(object msg)
        {
            if (HasLogLevel(LogLevel.Debug))
            {
                _LogHelper.Log(LogLevel.Debug, msg);
            }
        }

        /// <summary>
        /// Debug 레벨의 로그를 출력합니다
        /// </summary>
        /// <param name="format">메시지 포멧</param>
        /// <param name="args">파라미터</param>
        public static void Debug(string format, params object[] args)
        {
            if (HasLogLevel(LogLevel.Debug))
            {
                _LogHelper.Log(LogLevel.Debug, string.Format(format, args));
            }
        }

        /// <summary>
        /// Info 레벨의 로그를 출력합니다
        /// </summary>
        /// <param name="msg">메시지</param>
        public static void Info(object msg)
        {
            if (HasLogLevel(LogLevel.Info))
            {
                _LogHelper.Log(LogLevel.Info, msg);
            }
        }

        /// <summary>
        /// Info 레벨의 로그를 출력합니다
        /// </summary>
        /// <param name="format">메시지 포멧</param>
        /// <param name="args">파라미터</param>
        public static void Info(string format, params object[] args)
        {
            if (HasLogLevel(LogLevel.Info))
            {
                _LogHelper.Log(LogLevel.Info, string.Format(format, args));
            }
        }

        /// <summary>
        /// Warning 레벨의 로그를 출력합니다
        /// </summary>
        /// <param name="msg">메시지</param>
        public static void Warning(object msg)
        {
            if (HasLogLevel(LogLevel.Warning))
            {
                _LogHelper.Log(LogLevel.Warning, msg);
            }
        }

        /// <summary>
        /// Warning 레벨의 로그를 출력합니다
        /// </summary>
        /// <param name="format">메시지 포멧</param>
        /// <param name="args">파라미터</param>
        public static void Warning(string format, params object[] args)
        {
            if (HasLogLevel(LogLevel.Warning))
            {
                _LogHelper.Log(LogLevel.Warning, string.Format(format, args));
            }
        }

        /// <summary>
        /// Error 레벨의 로그를 출력합니다
        /// </summary>
        /// <param name="msg">메시지</param>
        public static void Error(object msg)
        {
            if (HasLogLevel(LogLevel.Error))
            {
                _LogHelper.Log(LogLevel.Error, msg);
            }
        }

        /// <summary>
        /// Error 레벨의 로그를 출력합니다
        /// </summary>
        /// <param name="format">메시지 포멧</param>
        /// <param name="args">파라미터</param>
        public static void Error(string format, params object[] args)
        {
            if (HasLogLevel(LogLevel.Error))
            {
                _LogHelper.Log(LogLevel.Error, string.Format(format, args));
            }
        }

        /// <summary>
        /// Fatal 레벨의 로그를 출력합니다
        /// </summary>
        /// <param name="msg">메시지</param>
        public static void Fatal(object msg)
        {
            if (HasLogLevel(LogLevel.Fatal))
            {
                _LogHelper.Log(LogLevel.Fatal, msg);
            }
        }

        /// <summary>
        /// Fatal 레벨의 로그를 출력합니다
        /// </summary>
        /// <param name="format">메시지 포멧</param>
        /// <param name="args">파라미터</param>
        public static void Fatal(string format, params object[] args)
        {
            if (HasLogLevel(LogLevel.Fatal))
            {
                _LogHelper.Log(LogLevel.Fatal, string.Format(format, args));
            }
        }
    }
}