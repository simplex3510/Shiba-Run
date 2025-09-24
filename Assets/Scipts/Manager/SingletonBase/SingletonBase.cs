using UnityEngine;

namespace Singleton
{
    public abstract class SingletonBase<T> : MonoBehaviour where T : SingletonBase<T>
    {
        private static T instance;
        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    GameObject singletonObject = GameObject.Find(typeof(T).Name);
                    if (singletonObject == null)
                    {
                        singletonObject = new GameObject(typeof(T).Name);
                        instance = singletonObject.AddComponent<T>();
                    }
                    else
                    {
                        instance = singletonObject.GetComponent<T>();
                    }
                }

                return instance;
            }
        }
    }
}