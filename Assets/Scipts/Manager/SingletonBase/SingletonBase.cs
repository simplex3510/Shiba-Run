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
                if (!instance)
                {
                    GameObject singletonObject = GameObject.Find(typeof(T).Name);
                    if (singletonObject)
                    {
                        instance = singletonObject.GetComponent<T>();
                    }
                    else
                    {
                        singletonObject = new GameObject(typeof(T).Name);
                        instance = singletonObject.AddComponent<T>();
                    }

                    DontDestroyOnLoad(instance.gameObject);
                }

                return instance;
            }
        }

        private void Awake()
        {
            if (Instance != this)
            {
                Debug.LogError("Multiple instances of singleton detected!");
                Destroy(this.gameObject);
            }
        }
    }
}