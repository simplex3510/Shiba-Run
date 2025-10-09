using System.Collections;

using UnityEngine;
using Singleton;
using Unity.Collections;

namespace Manager
{
    public class PlatformManager : SingletonBase<PlatformManager>
    {
        public float Speed { get { return speed; } }

        [Header("Move Setting")]
        [SerializeField] private float speed = 5.0f;

    }
}