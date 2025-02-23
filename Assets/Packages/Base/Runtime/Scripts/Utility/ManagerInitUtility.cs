using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Kunnymann.Base.Utility
{
    /// <summary>
    /// ModuleManager 초기화 유틸리티
    /// </summary>
    public class ManagerInitUtility
    {
        /// <summary>
        /// ModuleManager를 초기화하여, 직렬화된 모듈 객체들을 수집합니다
        /// </summary>
        /// <param name="ModuleManager">ModuleManager 객체</param>
        public static void InitModules(ModuleManager ModuleManager)
        {
            foreach (var module in ModuleManager.Modules)
            {
                module.Dispose();
            }

            ModuleManager.Modules.Clear();

            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            List<Type> types = new List<Type>();
            
            foreach (var assembly in assemblies)
            {
                types.AddRange(assembly.GetTypes().Where(t => t.IsSubclassOf(typeof(ModuleBase)) && Attribute.IsDefined(t, typeof(SerializableAttribute))));
            }
                

            types = types.OrderBy(setting => setting.FullName != "SettingModule" && setting.Name != "SettingModule").ToList();

            foreach (var type in types)
            {
                var moduleInstance = ScriptableObject.CreateInstance(type) as ModuleBase;
                ModuleManager.Modules.Add(moduleInstance);
            }

            Debugger.Debugger.Debug($"Initialized module count : {types.Count}");
        }
    }
}