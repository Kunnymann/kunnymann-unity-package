using System;

namespace Kunnymann.Base
{
    /// <summary>
    /// 스크립트 실행 순위 조절 Attribute
    /// </summary>
    public class ScriptExcutionOrder : Attribute
    {
        /// <summary>
        /// 실행 순서
        /// </summary>
        public int Order;

        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="order">순위</param>
        public ScriptExcutionOrder(int order)
        {
            this.Order = order;
        }
    }
}