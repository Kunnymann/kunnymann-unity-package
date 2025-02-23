using System.Collections.Generic;
using UnityEngine;

namespace Kunnymann.Base
{
    /// <summary>
    /// Module manager
    /// </summary>
    [ScriptExcutionOrder(-100)]
    public class ModuleManager : MonoBehaviour
    {
        /// <summary>
        /// 모듈 컨테이너
        /// </summary>
        [SerializeField] public List<ModuleBase> Modules = new List<ModuleBase>();

        /// <summary>
        /// 모듈 딕셔너리
        /// </summary>
        private Dictionary<string, ModuleBase> _modulesDic = new Dictionary<string, ModuleBase>();

        /// <summary>
        /// Singleton
        /// </summary>
        private static ModuleManager instance = null;
        /// <summary>
        /// Singleton 객체
        /// </summary>
        public static ModuleManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<ModuleManager>();

                    if (instance == null)
                    {
                        var moduleManager = GameObject.Find(ConstValue.ModuleManagerName);
                        if (moduleManager == null)
                            instance = new GameObject(ConstValue.ModuleManagerName).AddComponent<ModuleManager>();
                        else
                            instance = moduleManager.AddComponent<ModuleManager>();
                    }
                }
                return instance;
            }
        }

        /// <summary>
        /// 모듈 객체를 반환합니다
        /// </summary>
        /// <param name="moduleName">모듈 이름</param>
        /// <returns>모듈 객체</returns>
        public ModuleBase GetModule(string moduleName)
        {
            if (_modulesDic.ContainsKey(moduleName))
                return _modulesDic[moduleName];
            else
            {
                foreach (var module in Modules)
                {
                    if (module != null && (module.GetType().FullName == moduleName || module.GetType().Name == moduleName))
                    {
                        _modulesDic[moduleName] = module;
                        return module;
                    }
                }

                return null;
            }
        }

        /// <summary>
        /// 모듈 객체를 반환합니다.
        /// </summary>
        /// <typeparam name="T">모듈 클래스</typeparam>
        /// <returns>모듈 객체</returns>
        public T GetModule<T>() where T : ModuleBase
        {
            foreach (var module in Modules)
            {
                if (typeof(T).IsAssignableFrom(module.GetType()))
                    return module as T;
            }

            return null;
        }

        private void Update()
        {
            foreach (var module in Modules)
            {
                module?.Process();
            }
        }

        private void OnDestroy()
        {
            foreach (var module in Modules)
            {
                module?.Process(TransitionCommand.Terminate);
            }

            instance = null;
        }
    }
}