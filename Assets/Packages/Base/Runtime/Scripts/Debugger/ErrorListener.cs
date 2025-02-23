using System.Diagnostics;
using System;

namespace Kunnymann.Base.Debugger
{
    /// <summary>
    /// 에러 리스너
    /// </summary>
    public class ErrorListener
    {
        /// <summary>
        /// 에러를 체크합니다
        /// </summary>
        /// <param name="error">Exception</param>
        /// <param name="description">설명</param>
        /// <param name="needthrowerror">Throw 여부</param>
        /// <param name="stackSize">stack 사이즈, 기본 값은 2레벨 까지</param>
        public static void Check(Exception error, string description = "", bool needthrowerror = false, bool report = true)
        {
            StackTrace stackedFrame = new StackTrace(true);

            string query = string.Format("{0} {1} {2}", description, Environment.NewLine, GetStackTraceInfo(stackedFrame));

            // 여기에 에러 리포트하는 로직 추가

            if (needthrowerror)
            {
                /*
                if (report)
                {
                    HyperDebugger.Error(query);
                }
                */
                throw error;
            }
            else
            {
                /*
                if (report)
                {
                    LogNest.Instance.UploadLogToS3();
                }
                */
                Debugger.Error(query);
            }
        }

        /// <summary>
        /// 에러 StackTrace 정보를 가져옵니다
        /// </summary>
        /// <param name="stackTrace">StackTrace</param>
        /// <returns>포팅된 StackTrace</returns>
        private static string GetStackTraceInfo(StackTrace stackTrace)
        {
            System.Text.StringBuilder builder = new System.Text.StringBuilder();

            for (int i = 1; i < stackTrace.FrameCount; i++)
            {
                StackFrame frame = stackTrace.GetFrame(i);
                var method = frame.GetMethod();
                var declaringType = method.DeclaringType;
                string sourceFileName = System.IO.Path.GetFileName(frame.GetFileName());
                int sourceFileLineNumber = frame.GetFileLineNumber();

                // 이벤트나 델레게이트 경우 스킵
                if (sourceFileName == string.Empty || sourceFileLineNumber == 0)
                {
                    continue;
                }

                if (declaringType != null)
                {
                    builder.AppendFormat("{0}.{1} (in {2}, line {3})", declaringType.Name, method.Name, sourceFileName, sourceFileLineNumber);
                    
                    if (i < stackTrace.FrameCount - 1)
                    {
                        builder.Append(" -> ");
                    }
                }
            }

            return builder.ToString();
        }
    }
}
