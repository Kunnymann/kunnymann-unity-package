namespace Kunnymann.Base
{
    /// <summary>
    /// 모듈 이벤트 핸들러
    /// </summary>
    public delegate void EventHandler();
    /// <summary>
    /// 모듈 이벤트 핸들러
    /// </summary>
    /// <typeparam name="TEventArg">타입</typeparam>
    /// <param name="eventArg">파라미터</param>
    public delegate void EventHandler<TEventArg>(TEventArg eventArg);
    /// <summary>
    /// 모듈 이벤트 핸들러
    /// </summary>
    /// <typeparam name="TEventArg1">타입 1</typeparam>
    /// <typeparam name="TEventArg2">타입 2</typeparam>
    /// <param name="eventArg1">파라미터 1</param>
    /// <param name="eventArg2">파라미터 2</param>
    public delegate void EventHandler<TEventArg1, TEventArg2>(TEventArg1 eventArg1, TEventArg2 eventArg2);
}