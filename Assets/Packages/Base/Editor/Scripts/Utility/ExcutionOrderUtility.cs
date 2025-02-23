using System;
using UnityEditor;

namespace Kunnymann.Base.Editor
{
    /// <summary>
    /// 에디터 실행 시, 스크립트 실행 순서를 조정하는 유틸리티 클래스
    /// </summary>
    [InitializeOnLoad]
    public class ExcutionOrderUtility
    {
        /// <summary>
        /// 생성자
        /// </summary>
        static ExcutionOrderUtility()
        {
            foreach (MonoScript script in MonoImporter.GetAllRuntimeMonoScripts())
            {
                if (script.GetClass() != null)
                {
                    foreach (var asset in Attribute.GetCustomAttributes(script.GetClass(), typeof(ScriptExcutionOrder)))
                    {
                        var assetOrder = MonoImporter.GetExecutionOrder(script);
                        var assetNewOrder = ((ScriptExcutionOrder)asset).Order;
                        if (assetOrder != assetNewOrder)
                        {
                            MonoImporter.SetExecutionOrder(script, assetNewOrder);
                        }
                    }
                }
            }
        }
    }
}
