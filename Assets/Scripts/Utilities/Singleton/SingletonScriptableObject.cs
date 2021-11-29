using UnityEngine;

namespace Utilities.Singleton
{
    public abstract class SingletonScriptableObject<T> : ScriptableObject where T : ScriptableObject
    {
        private static T _instance;

        public static T Instance
        {
            get
            {
                if (_instance) return _instance;

                _instance = Resources.Load<T>(typeof(T).ToString());
                (_instance as SingletonScriptableObject<T>)?.OnInit();

                return _instance;
            }
        }

        protected virtual void OnInit()
        {
        }
    }
}